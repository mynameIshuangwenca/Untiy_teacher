using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private InputField Account;//账号输入框
    private InputField Password;//密码输入框
    
    private InputField Age;//年龄入框
   

    private Button Btn_Login;
    private Button Btn_Resigter;
    public GameObject errorTip;
    private GameObject ToggleValue;//性别
    private String genderValue;
    private void Awake()
    {
        InitObj();
    }

    private void InitObj()
    {

        Account = transform.Find("/Canvas/Bg/P_Register/UserName").GetComponent<InputField>();
        Password = transform.Find("/Canvas/Bg/P_Register/Password").GetComponent<InputField>();
         ToggleValue = transform.Find("/Canvas/Bg/P_Register/ToggleValue").gameObject;
        Age = transform.Find("/Canvas/Bg/P_Register/Age").GetComponent<InputField>();

       
        Btn_Resigter = transform.Find("/Canvas/Bg/P_Register/Btn_Regiter").GetComponent<Button>();
        Btn_Login = transform.Find("/Canvas/Bg/P_Register/Btn_Cancel").GetComponent<Button>();
        Btn_Resigter.onClick.AddListener(delegate { BtnResigter(); });
        Btn_Login.onClick.AddListener(delegate { BtnLogin(); });
    }

    private void BtnLogin()
    {
        ToLogin();
    }

    private void BtnResigter()
    {
        genderValue = ToggleValue.GetComponent<ToggleGroupValue>().GetToggleValue();


        Debug.Log("账号：" + Account.text + "  密码：" + Password.text+"性别"+ genderValue + "年龄"+Age.text);
        if (!Check())
        {
            return;
        }
       ReRegister ret= SQLController.Instance.Register(Account.text.Trim(), Password.text.Trim(), Age.text.Trim(), genderValue.Trim());
        if (ret==ReRegister.Have)
        {
            RegisterHave();
        }
        else if (ret==ReRegister.Fail)
        {
            RegisterFail();
        }
        else if (ret==ReRegister.Success)
        {

            StartCoroutine(RegisterSuccess("注册成功"));

        }
        else
        {

        }
    }


    IEnumerator RegisterSuccess(string message)
    {
        ShowTip(message);
        yield return new WaitForSeconds(2.0f);
        // 跳转到登录
        ToLogin();
    }
   

    private void RegisterFail()
    {
        ShowTip("注册失败");
    }

    private void RegisterHave()
    {
        ShowTip("用户已经存在");
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
        int ag = 0;
        bool isInt = int.TryParse(age, out ag);
        if (age=="")
        {
            str = "年龄为空";
           
        } 
        else if (!int.TryParse(age, out ag))
        {
            str = "年龄的格式不正确应该为 0--100 中的中的数字";
        }
        else if (ag<0||ag>100)
        {
            str = "年龄范围不正确";
           
        }
        else
        {
            return true;
        }
        
        return false;

    }

    private bool CheckGender(string gender,ref string retStr)
    {
        
        if(gender == "")
        {
            retStr = "性别为空";
        }
        else
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 检测输入的数据是否正确
    /// </summary>
    /// <returns></returns>
    private bool Check()
    {
       
        string retStr=string.Empty;
        bool IsCheck = false;
        //Account.text, Password.text, Gender.text, Class.text
        if (Account.text == "")
        {
            // 
            retStr="姓名为空";
            IsCheck= false;
        }
        else if (Password.text == "")
        {
            retStr = "密码为空";
            IsCheck= false;
        }
       
        else if (!CheckAge(Age.text,ref retStr))
        {

            IsCheck= false;
        }
        else
        {
            // 成功
            IsCheck = true;
            return IsCheck;
        }
        // 提示框
        ShowTip(retStr);
        return IsCheck;

    }

    /// <summary>
    /// 跳转到注册页面
    /// </summary>
    public void ToLogin()
    {
        Tool.Instance.LoadScene(Util.LoginScene);
    }

    //显示提示框
    private void ShowTip(string message)
    {
        errorTip.GetComponent<ErrorTip>().Show(message);
    }
}

