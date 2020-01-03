using QmDreamer.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MoveView : MonoSingleton<MoveView>
{

    private GameObject playerParent;
    private GameObject GridLayer;
    public  Button btn_Reload;
    public GameObject routePrefab;
    public Sprite[] routeDirtSpite;
    private Button btn_Start;
    private Button btn_Back;
    private Button btn_Restart;
    private Button btn_Tip;
    private Button btn_Music;
    private Button btn_LeftOfScroll;
    private Button btn_RightOfScroll;
    private Transform scrollContext;
    //private GameObject redFlag;
    //private GameObject blueFlag;
    //private GameObject greenFlag;

    

  


    
    private void Awake()
    {
        //PlayerDrog.EndDrog += EndDrog;
        //ArrowDrog.ArrowEndDrog += ArrowEndDrog;
        // DontDestroyOnLoad(this.gameObject);
        InitObj();
        InitFun();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (MoveModel.Instance.moveCache.Drog.Count > 0)
        {
            Tool.Instance.FadeAndX(btn_Back.GetComponent<Image>(), true);
        }
        else
        {
            Tool.Instance.FadeAndX(btn_Back.GetComponent<Image>(), false);
        }
    }
    private void InitFun()
    {

        btn_Reload.onClick.AddListener(delegate { ReturnScene(); });
        btn_Start.onClick.AddListener(delegate { PlayerWalk(); });
        btn_Back.onClick.AddListener(delegate { Btn_Back(); });
        btn_Restart.onClick.AddListener(delegate { ReloadScene(); });
        btn_Tip.onClick.AddListener(delegate { BtnTip(); });
        btn_Music.onClick.AddListener(delegate { BtnMusic(); });
        btn_LeftOfScroll.onClick.AddListener(delegate { Btn_LeftOfScroll(); });
        btn_RightOfScroll.onClick.AddListener(delegate { Btn_RightOfScroll(); });
         
       
        
           

    }
    /// <summary>
    /// scroll 右按钮
    /// </summary>
    private void Btn_RightOfScroll()
    {
        Debug.Log("Btn_RightOfScroll");
        scrollContext.DOMove(new Vector3(100,0,0), 1).SetRelative();
    }
/// <summary>
/// scroll左按钮
/// </summary>
    private void Btn_LeftOfScroll()
    {
        Debug.Log("Btn_LeftOfScroll");
        scrollContext.DOMove(new Vector3(-100, 0, 0), 1).SetRelative();
    }

    private void InitObj()
    {
        playerParent = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Player").gameObject;
        GridLayer = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Route/Img_Route").gameObject;
        btn_Start = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Btn/Img_Start").GetComponent<Button>();
        btn_Back = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Btn/Img_Back").GetComponent<Button>();
        btn_Restart = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Btn/Img_Restart").GetComponent<Button>();
        btn_Tip = transform.Find("/Canvas/P_Backgound/P_Funtion/Img_Tip").GetComponent<Button>();
        btn_Music = transform.Find("/Canvas/P_Backgound/P_Funtion/Img_Voice").GetComponent<Button>();
        btn_LeftOfScroll=transform.Find("/Canvas/P_Backgound/P_Controller/Scroll/Btn_LeftOfScroll").GetComponent<Button>();
        btn_RightOfScroll = transform.Find("/Canvas/P_Backgound/P_Controller/Scroll/Btn_RightOfScroll").GetComponent<Button>();
        scrollContext = transform.Find("/Canvas/P_Backgound/P_Controller/Scroll/View/Context");
        //redFlag = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Destination/Desination").gameObject;
        //blueFlag = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Destination/Middle").gameObject;
        //greenFlag = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Destination/Start").gameObject;

    }
    private void PlayerWalk()
    {
        Debug.Log("PlayerWalk");

        MoveController.Instance.SetVirtualPlayer(false);
        MoveController.Instance.Walk();
       
    }
//  
    public void Btn_Back()
    {

        int index = MoveModel.Instance.moveCache.Drog.Count;
        if (index <= 0) return;
        MoveController.Instance.BackWalk(MoveModel.Instance.moveCache.Drog[index-1]);
        MoveModel.Instance.moveCache.Drog.RemoveAt(index - 1);
        AudioManager.Instance.PlaySound(13);
        // MoveController.Instance.SetVirtualPlayer(false);
        //  MoveModel.Instance.FadeArrow(false);
    }
    public void EndDrog(EndDrogMess endDrogMess)
    {
        SetDrog(false);
    }


    private void SetDrog(bool flag)
    {

        //  GetComponentsInChildren 会把自己也加进去
        Transform[] trans = playerParent.GetComponentsInChildren<Transform>();
      
        foreach (Transform tran in trans )
        {
            if (tran.name == "P_Player")  continue;
            Tool.Instance.Fade(tran.GetComponent<Image>(), flag);
            tran.GetComponent<Image>().raycastTarget = flag;
        }       
    }
    public void ArrowEndDrog(EndDrogMess endDrogMess)
    {
        MoveController.Instance.SetVirtualPlayer(true);
        GenerateArrowRoute(endDrogMess.Dirt);
    }

    private void  GenerateArrowRoute(int dirt)
    {

        GameObject newGo = ObjectPool.Instance.CreateObject(routePrefab.name, routePrefab, new Vector3(0, 0, 0), GridLayer.transform);
        newGo.transform.parent = GridLayer.transform;
        // 使生成的名字一致 对象池
        newGo.name = routePrefab.name;
        newGo.GetComponent<Image>().sprite = routeDirtSpite[dirt];
        MoveModel.Instance.moveCache.ArrowRoute.Add(newGo);
       // MoveModel.Instance.RouteObj.Add(newGo);
    }


    
    public void ReturnScene()
    {
        MoveModel.Instance.RestartTData();
        MoveModel.Instance.ClearData();
        Tool.Instance.LoadScene(Util.ShowCourseScene);

    }





    public void ReloadScene()
    {
       
        Debug.Log("ReloadScene");

        // view 的复原
        SetDrog(true);
        MoveModel.Instance.FadeArrow(true);

        // 数据处理
        MoveModel.Instance.RestartTData();
        MoveController.Instance.CleanData();
        // 障碍物的处理
        MoveModel.Instance.obstacle.Clean();

        // 旗子的恢复

        FadeFlag(3);




    }


    public void  SetRestartBtn(bool flag)
    {
        btn_Start.interactable = flag;
    }


    public void BtnTip()
    {
        TimerManager.Instance.StartTimer();
    }


    public void BtnMusic()
    {
        TimerManager.Instance.StopTimer();
    }

    /// <summary>
    /// 旗子变淡和恢复  +不能推动
    /// </summary>
    /// <param name="type">  0，1，2 绿 红 蓝旗子 变淡 和恢复 其他 恢复</param>
    public void FadeFlag(int type,  bool flag =false)
    {  
        if (type >=3)
        {
            foreach (GameObject obj in MoveModel.Instance.flag)
            {
                if (obj == null) continue;
                Tool.Instance.Fade(obj.GetComponent<Image>(), true);
                obj.GetComponent<Image>().raycastTarget = true;
            }
        }
        else
        {

            Tool.Instance.Fade(MoveModel.Instance.flag[type].GetComponent<Image>(), flag);
            MoveModel.Instance.flag[type].GetComponent<Image>().raycastTarget = flag;
        }
       
    }


    

   
}
