//using UnityEngine;
//using System.Collections;
//using System.IO;
//using Mono.Data.Sqlite;
//using UnityEngine.UI;

//public class SQLiteDemo : MonoBehaviour
//{
//    /// <summary>
//    /// SQLite数据库辅助类
//    /// </summary>
//    private SQLiteHelper sql;
//    public Text sqlText;
//    public Text routeText;
//    void Start()
//    {
//        //各平台下数据库存储的绝对路径(通用)
//        //PC：("data source=" + Application.dataPath + "/幻世界.db");
//        //Mac：("data source=" + Application.dataPath + "/幻世界.db");
//        //Android：("URI=file:" + Application.persistentDataPath + "/幻世界.db");
//        //iOS：("data source=" + Application.persistentDataPath + "/幻世界.db");

//        Debug.Log(Application.persistentDataPath);
//        sql = new SQLiteHelper("URI=file:" + Application.persistentDataPath + "/幻世界.db");
//        routeText.text = Application.persistentDataPath;
//        //1.创建名为table1的数据表
//        sql.CreateTable("UserInfo", new string[] { "UniqueID", "Name", "Age", "PhoneNum" }, new string[] { "int", "string", "int", "string" });

//        //插入两条数据
//        sql.InsertValues("UserInfo", new string[] { "'1'", "'xxx'", "'22'", "'18'" });
//        sql.InsertValues("UserInfo", new string[] { "'2'", "'xxx'", "'25'", "'1683245892'" });

//        //更新数据，将Name="张乐"的记录中的Name改为"zhangle"
//        sql.UpdateValues("UserInfo", new string[] { "Name" }, new string[] { "'zhangle'" }, "Name", "=", "'张乐'");

//        //插入3条数据
//        sql.InsertValues("UserInfo", new string[] { "3", "'xxx'", "25", "'18565789511'" });
//        sql.InsertValues("UserInfo", new string[] { "4", "'xxx'", "26", "'17745898567'" });
//        sql.InsertValues("UserInfo", new string[] { "5", "'xxx'", "27", "'13965235489'" });

//        //删除Name="王天科"且Age=26的记录,DeleteValuesOR方法类似
//        sql.DeleteValuesAND("UserInfo", new string[] { "Name", "Age" }, new string[] { "=", "=" }, new string[] { "'王天科'", "'26'" });

//        //读取整张表
//        SqliteDataReader reader = sql.ReadFullTable("UserInfo");
//        while (reader.Read())
//        {
//            sqlText.text = reader.GetString(reader.GetOrdinal("Name"));
//            //读取ID
//            Debug.Log(reader.GetInt32(reader.GetOrdinal("UniqueID")));
//            //读取Name
//            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
//            //读取Age
//            Debug.Log(reader.GetInt32(reader.GetOrdinal("Age")));
//            //读取Email
//            // Debug.Log(reader.GetString(reader.GetOrdinal("PhoneNum")));
//               Debug.Log(reader.GetInt32(reader.GetOrdinal("PhoneNum")));
//        }

//        //读取数据表中Age>=25的所有记录的ID和Name
//        reader = sql.ReadTable("UserInfo", new string[] { "UniqueID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
//        while (reader.Read())
//        {
//            //读取ID
//            Debug.Log(reader.GetInt32(reader.GetOrdinal("UniqueID")));
//            //读取Name
//            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
//        }

//        //自定义SQL,删除数据表中所有Name="王天科"的记录
//    //   sql.ExecuteQuery("DELETE FROM 幻世界.db WHERE NAME='xxx'");

//        //关闭数据库连接
//        sql.CloseConnection();
//    }
//}