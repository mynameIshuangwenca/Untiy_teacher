
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum GameState
{
    prepartion,
    start,
    pressing,// 按钮增加路线状态
    Walking,//人物走动状态
    Destination,// 到达终点的状态
    AnimationFlag,
    Fail,// 路劲走完了但是没有成功
}

public enum  ReRegister{
    Success,
    Fail,
    Have

}

public class SQLController : MonoSingleton<SQLController>
{
   
        private SQLiteHelper sql;
        private string  SQLUrl;
        private string SQLUserInfo="UserInfo" ;
        private string perUrl;

   
    private void Awake()
        {
          
          
            perUrl = Application.persistentDataPath + "/StudentData.db";
            SQLUrl = "URI=file:" + perUrl;
            Debug.Log(SQLUrl);
            sql = new SQLiteHelper(SQLUrl);
            CreateSqliteDatabse();
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
                    sql.CreateTable(SQLUserInfo, new string[] {  "Name","Password", "Age", "gender",  }, new string[] { "string", "string", "int", "string" });
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                   // sql.CloseConnection();
                    Debug.Log("基础表创建完成");
                }

            
        }
        /// <summary>
        /// 向指定数据表中插入数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="values">插入的数值</param>
        public void InsertValues(string tableName, string[] values)
        {
            sql.InsertValues(tableName, values);
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
            // 读取数据表中Age >= 25的所有记录的ID和Name
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
            SqliteDataReader reader=  sql.InsertValues(SQLUserInfo, new string[] { userName, password, age,gender });
            return ReRegister.Success;
        }


        private void OnApplicationQuit()
        {
            //关闭数据库连接
            sql.CloseConnection();
        }

    }
