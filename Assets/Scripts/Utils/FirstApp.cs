using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstApp : MonoBehaviour
{
    
    // 数据库的委托
    public static Action SqlUpdate;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("FirstApp"))
        {
            GameUpdate();

            //第一次进入

        }
    }

    /// <summary>
    /// 这是是实现App第一次打开  App 路径存在
    /// LaterUpdate在start 后出现
    /// </summary>
    private void LateUpdate()
    {
        //如果来过就存储一个字符串
        PlayerPrefs.SetInt("FirstApp", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 游戏升级
    public void GameUpdate()
    {
        //获取此时的版本号
        string version = Application.version;
        float newVersion;
        if (float.TryParse(version, out newVersion))
        {
            if (newVersion >= 0.1)
            {
                SqlUpdate();

            }
        }


    }
}
