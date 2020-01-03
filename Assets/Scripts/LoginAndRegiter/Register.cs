using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private InputField Account;//账号输入框
    private InputField Password;//密码输入框
    private InputField Gender;//性别
    private InputField Age;//年龄入框
   

    private Button Btn_Back;
    private Button Btn_Resigter;
    private void Awake()
    {
        InitObj();
    }

    private void InitObj()
    {

        Account = transform.Find("/Canvas/Bg/P_Register/UserName").GetComponent<InputField>();
        Password = transform.Find("/Canvas/Bg/P_Register/Password").GetComponent<InputField>();
        Gender = transform.Find("/Canvas/Bg/P_Register/Gender").GetComponent<InputField>();
        Age = transform.Find("/Canvas/Bg/P_Register/Age").GetComponent<InputField>();

       
        Btn_Resigter = transform.Find("/Canvas/Bg/P_Register/Btn_Regiter").GetComponent<Button>();
        Btn_Resigter.onClick.AddListener(delegate { BtnResigter(); });
    }

    private void BtnResigter()
    {
        Debug.Log("账号：" + Account.text + "  密码：" + Password.text+"性别"+ Gender.text+"年龄"+Age.text);
        if (!Check())
        {
            return;
        }
       ReRegister ret= SQLController.Instance.Register(Account.text, Password.text, Age.text, Gender.text);
        if (ret==ReRegister.Have)
        {

        }
        else if (ret==ReRegister.Fail)
        {

        }
        else if (ret==ReRegister.Success)
        {

        }
        else
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   private  bool CheckAge(string age,ref string str)
    {
        
        if (age=="")
        {
            str = "年龄为空";
            return false;
        }
        int ag = int.Parse(age);
        if (ag<0||ag>100)
        {
            str = "年龄范围不正确";
            return false;
        }
        return true;

    }

    /// <summary>
    /// 检测输入的数据是否正确
    /// </summary>
    /// <returns></returns>
    private bool Check()
    {
       
        string retStr="";
        //Account.text, Password.text, Gender.text, Class.text
        if (Account.text == "")
        {
            // 
            retStr="账号为空";
            return false;
        }
        if (Password.text == "")
        {
            retStr = "密码为空";
           
            return false;
        }
        if (Gender.text == "")
        {
            retStr = "性别为空";
            return false;
        }
        if (!CheckAge(Age.text,ref retStr))
        {
           
            return false;
        }
        return true;

    }

    /// <summary>
    /// 跳转到注册页面
    /// </summary>
    public void ToLogin()
    {
        Tool.Instance.LoadScene(Util.LoginScene);
    }
}
