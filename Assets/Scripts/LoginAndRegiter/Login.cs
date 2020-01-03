
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
        Btn_SignIn.onClick.AddListener(delegate { LoginIn(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        //SQLController.Instance.Regiter("huangwencai", "123456", "男", 25);
        //LoginIn();
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

        Tool.Instance.LoadScene(Util.ShowCourseScene);
        //result = SQLController.Instance.Login(Account.text, Password.text);
        //if (!result)
        //{

        //}
        //else
        //{
        //    // 登录成功
        //    //记录是谁登陆的
        //    PlayerPrefs.SetString("userName", Account.text);
        //    Tool.Instance.LoadScene(Util.ShowCourseScene);


        //}


    }

    /// <summary>
    /// 跳转到注册页面
    /// </summary>
    public void ToRegister()
    {
        Tool.Instance.LoadScene(Util.RegisterScene);
    }
}