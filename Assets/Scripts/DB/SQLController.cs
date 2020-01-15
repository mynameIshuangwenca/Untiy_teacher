
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum  ReRegister{
    Success,
    Fail,
    Have

}

public enum SqlState
{
    Success,
    Fail,
    Have,
    NotHave,
}

public class SQLController : MonoSingleton<SQLController>
{
   
        private SQLiteHelper sql;
        private string  SQLUrl;
        private string SQLUserInfo="UserInfo" ;
        private string SQLUserRoute = "UserRoute";
        private string[] UserRoute = new string[] { "Route_Id", "User_Id", "Scene_Id", "Start", "Middle", "Destination", "Route", "RouteOrder", "StepNum", "Duration", "fieldOne", "fieldTwo", "MiddleStep" };

        private string perUrl;
        

   
    private void Awake()
        {
        //  // 场景切换 只有一个
        //  if (Instance != null)
        //  {
        //      return;
        //  }  
        ////  DontDestroyOnLoad(gameObject);
        ///
        FirstApp.SqlUpdate += SqlUpdate;
        perUrl = Application.persistentDataPath + "/StudentData.db";
            SQLUrl = "URI=file:" + perUrl;
            Debug.Log(SQLUrl);
            sql = new SQLiteHelper(SQLUrl);
        try
        {
            CreateSqliteDatabse();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
           
        }
    // 数据库更新
    private void SqlUpdate()
    {
        InsertFeild("MiddleStep", SQLUserRoute);
       //插入字段
    }

    // Start is called before the first frame update
    void Start()
        {

            
        }

    // Update is called once per frame
    void Update()
    {
        
    }

        public void CreateSqliteDatabse()
        {

               try
                { 
                    sql.CreateTable(SQLUserInfo, new string[] { "User_Id", "Name","Password", "Age", "gender",  }, new string[] { "INTEGER PRIMARY KEY", "string", "string", "int", "string" });
                    sql.CreateTable(SQLUserRoute, UserRoute,new string[] { "INTEGER PRIMARY KEY", "INTEGER", "INTEGER", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)", "VARCHAR(255)" }, "User_Id", SQLUserInfo);
                    
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                   // sql.CloseConnection();
                    //Debug.Log("基础表创建完成");
                }

            
        }
       

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public void DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            sql.DeleteValuesAND(tableName, colNames, operations, colValues);
        }




        public List<UserInfo>  ReadALLData(string tableName)
        {
            List<UserInfo> allUserInfo = new List<UserInfo>();
            SqliteDataReader reader = sql.ReadFullTable(tableName);
            while (reader.Read())
            {
                UserInfo user = new UserInfo();
                user.Name= reader.GetString(reader.GetOrdinal("Name"));
                user.Age = reader.GetInt32(reader.GetOrdinal("Age"));
                user.Gender = reader.GetString(reader.GetOrdinal("Gender"));
               
                user.Password = reader.GetString(reader.GetOrdinal("Password"));
                allUserInfo.Add(user);
            }


            return allUserInfo;
        }

        /// <summary>
        /// 登录时的校验
        /// </summary>
        /// <param name="name">用户名字</param>
        /// <param name="password">用户密码</param>
        /// <returns>是否存在</returns>
        public bool  Login(string name ,string password)
        {
            
            SqliteDataReader reader  = sql.ReadTable(SQLUserInfo, new string[] { "*" }, new string[] { "Name","Password" }, new string[] {"=", "=" }, new string[] { name,password });  
            if(reader.Read())
            {
                return true;
            }
            else
            {
                return false;

            }
        }

    /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="age"></param>
    /// <param name="gender"></param>
               
        public ReRegister Register(string userName,string password, string age,string gender)
        {
           if(Login(userName,password))  return ReRegister.Have;
            SqliteDataReader reader=  sql.InsertValues(SQLUserInfo,new string[] { "Name", "Password", "Age", "gender"}, new string[] { userName, password, age,gender });
            return ReRegister.Success;
        }


        private void OnApplicationQuit()
        {
        if(sql!=null)
        {
            //关闭数据库连接
            sql.CloseConnection();
        }
  
        }

    private void OnDestroy()
    {
        if (sql != null)
        {
            //关闭数据库连接
            sql.CloseConnection();
        }
    }
    /// <summary>
    /// 通过名字得到id
    /// </summary>
    /// <param name="Username"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public SqlState  GetIdByUserName(string userName,out int userId)
    {
      
        SqliteDataReader reader = sql.ReadTable(SQLUserInfo, new string[] { "*" }, new string[] { "Name" }, new string[] {  "=" }, new string[] { userName });
        if (reader.Read())
        {
            userId = reader.GetInt32(reader.GetOrdinal("User_Id"));
            return SqlState.Success;
        }
        else
        {
            userId = -1;
            return SqlState.NotHave;

        }
        

    }
    /// <summary>
    /// 插入路径
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="route"></param>
    /// <returns></returns>
    public SqlState InsertRoute(String userName,string route)
    {    int userId;
        SqlState sqlState = GetIdByUserName(userName, out userId);
        if (sqlState != SqlState.Success) return sqlState;
        sql.ExecuteQuery("PRAGMA foreign_keys = ON; ");
        SqliteDataReader reader = sql.InsertValues(SQLUserRoute, new string[] { "User_Id", "Route" }, new string[] { string.Format("{0}", userId), route });
        return SqlState.Success;
    }

    //插入路径
    public SqlState InsertRoute(string UserName, RouteInfo routeInfo)
    {
        int userId;
        SqlState sqlState = GetIdByUserName(UserName, out userId);
        if (sqlState != SqlState.Success) return sqlState;
        routeInfo.UserId=userId;
        sql.ExecuteQuery("PRAGMA foreign_keys = ON; ");
        SqliteDataReader reader = sql.InsertValues(SQLUserRoute, new string[] { "User_Id", "Scene_Id", "Start", "Middle", "Destination", "Route", "RouteOrder", "StepNum", "Duration", "fieldOne", "fieldTwo" , "MiddleStep" }, routeInfo.GetStr());
        return SqlState.Success;
    }





    /// <summary>

    /// 判断表中某个字段是否存在

    /// </summary>

    /// <param name="fieldName">字段名</param>

    /// <param name="tableName">表名</param>

    /// <returns></returns>

   public  bool isExistField(string fieldName, string tableName)

    {

        string query = "select sql from sqlite_master where tbl_name = '" + tableName + "' and type = 'table'";

        SqliteDataReader reader= sql.ExecuteQuery(query);
        //开始查询数据库

        bool IsExist = false;

       

        while (reader.Read())

        {

            IsExist = reader["sql"].ToString().Contains(fieldName); 
        }


        return IsExist;

    }


    public SqlState InsertFeild(string fieldName, string tableName)
    {
        if (!isExistField( fieldName,  tableName))
         {

            String queryStr = string.Format(" alter table {0} add column {1} varchar(256) default '';", tableName, fieldName); ;
            SqliteDataReader reader = sql.ExecuteQuery(queryStr);
            if (reader.Read())
            {
                return SqlState.Success;
            }
            else
            {
                return SqlState.Fail;
            }

        }
        else
        {
            return SqlState.Have;
        }

    }




}
