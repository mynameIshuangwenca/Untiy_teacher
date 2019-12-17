using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 单例模式基类，不继承于 MonoBehaviour，随场景切换而销毁
/// 挂载 T 类继承 DDOLSingleton<T>，重载 Init 函数即可
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : class, new()
{
    protected static T _instance = null;

    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    protected Singleton()
    {
        if (null != _instance)
        {
            Debug.LogError("This " + typeof(T).ToString() + " Singleton Instance is not null !!!");
        }
        Init();
    }

    public virtual void Init()
    {

    }
}