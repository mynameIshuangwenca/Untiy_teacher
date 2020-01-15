
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    private static Login instance;
    private InputField Account;//账号输入框
    private InputField Password;//密码输入框
    private Button Btn_SignIn;
    private Button Btn_Register;

    public GameObject errorTip;



    public static Login Instance { get => instance; set => instance = value; }
    void Awake()
    {
        instance = this;
        InitObj();


    }
   
    private void InitObj()
    {
        Account = transform.Find("/Canvas/BackGround/P_Login/UserName").GetComponent<InputField>();
        Password = transform.Find("/Canvas/BackGround/P_Login/Password").GetComponent<InputField>();
        Btn_SignIn = transform.Find("/Canvas/BackGround/P_Login/Btn_Login").GetComponent<Button>();
        Btn_Register = transform.Find("/Canvas/BackGround/P_Login/Btn_Register").GetComponent<Button>();
        Btn_SignIn.onClick.AddListener(delegate { LoginIn(); });
        Btn_Register.onClick.AddListener(delegate { ToRegister(); });
    }
    // Start is called before the first frame update
    void Start()
    {
       
        StartLogin();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    /// <summary>
    /// 绑定到登录按钮上
    /// </summary>
    public void LoginIn()
    {
        bool result = false;
        Debug.Log("账号：" + Account.text + "  密码：" + Password.text);
        if (!LoginCheck()) return;
        result = SQLController.Instance.Login(Account.text.Trim(), Password.text.Trim());
        if (!result)
        {
            LoginFail();
        }
        else
        {
            LoginSeccess(Account.text.Trim(), Password.text.Trim());


        }


    }
/// <summary>
/// 开始时进行一次登录
/// </summary>
    public void StartLogin()
    {
        bool result = false;
        string userName = PlayerPrefs.GetString("userName", string.Empty);
        string password = PlayerPrefs.GetString("password", string.Empty);
        
        result = SQLController.Instance.Login(userName, password);
        if (result)
        {
            LoginSeccess(userName, password);
        }
    }




    /// <summary>
    /// 跳转到注册页面
    /// </summary>
    public void ToRegister()
    {
        Tool.Instance.LoadScene(Util.RegisterScene);
    }


    public bool  LoginCheck()
    {
        string retStr = string.Empty;
        bool IsCheck = false;

        if (Account.text == "")
        {
            // 
            retStr = "账号为空";
            IsCheck = false;
        }
        else if (Password.text == "")
        {
            retStr = "密码为空";
            IsCheck = false;
        }
        else
        {
            IsCheck = true;
            return IsCheck;
        }
        // 提示框
        ShowTip(retStr);
        return IsCheck;
    }

    //显示提示框
    private void ShowTip(string message)
    {
        errorTip.GetComponent<ErrorTip>().Show(message);
    }

    /// <summary>
    /// 登录失败
    /// </summary>
    private void LoginFail()
    {
        ShowTip("账号或密码错误");
        Account.text = string.Empty;
        Password.text = string.Empty;

    }
    /// <summary>
    /// 登录成功
    /// </summary>
    private void LoginSeccess(string userName,string password)
    {
        // 登录成功
        //记录是谁登陆的
        PlayerPrefs.SetString("userName", userName);
        PlayerPrefs.SetString("password", password);
        Tool.Instance.LoadScene(Util.ClassList);
    }




}