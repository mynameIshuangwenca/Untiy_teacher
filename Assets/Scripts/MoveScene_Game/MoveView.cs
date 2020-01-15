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
    public float BtnScrollDistance=120;

    private Transform cacheParent;
    public Transform CacheParent { get => cacheParent; set => cacheParent = value; }

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

        if(MoveController.Instance.gameState ==GameState.Walking)
        {
            Tool.Instance.FadeAndX(btn_Back.GetComponent<Image>(), false);
            Tool.Instance.FadeAndX(btn_Restart.GetComponent<Image>(), false);
        }
        else
        {
            Tool.Instance.FadeAndX(btn_Back.GetComponent<Image>(), true);
            Tool.Instance.FadeAndX(btn_Restart.GetComponent<Image>(), true);
        }
    }
    private void InitFun()
    {

        btn_Reload.onClick.AddListener(delegate { ReturnScene(); });
        btn_Start.onClick.AddListener(delegate { Btn_Start(); });
        btn_Back.onClick.AddListener(delegate { Btn_Back(); });
        btn_Restart.onClick.AddListener(delegate { ReloadScene(); });
        btn_Tip.onClick.AddListener(delegate { BtnTip(); });
        btn_Music.onClick.AddListener(delegate { BtnMusic(); });
        btn_LeftOfScroll.onClick.AddListener(delegate { Btn_RightOfScroll(); });
        btn_RightOfScroll.onClick.AddListener(delegate { Btn_LeftOfScroll(); }); 







    }
    /// <summary>
    /// scroll 右按钮
    /// </summary>
    private void Btn_RightOfScroll()
    {
        Debug.Log("Btn_RightOfScroll");
        scrollContext.DOMove(new Vector3(BtnScrollDistance, 0,0), 1).SetRelative();
    }
/// <summary>
/// scroll左按钮
/// </summary>
    private void Btn_LeftOfScroll()
    {
        Debug.Log("Btn_LeftOfScroll");
        scrollContext.DOMove(new Vector3(-BtnScrollDistance, 0, 0), 1).SetRelative();
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

        CacheParent = transform.Find("/Canvas/VirtualPlayerparent");
        MoveModel.Instance.Flag[0] = transform.Find("/Canvas/P_Backgound/P_Controller/Scroll/View/Context/P_Start/Start").gameObject;
        MoveModel.Instance.Flag[1] = transform.Find("/Canvas/P_Backgound/P_Controller/Scroll/View/Context/P_Destination/Destination").gameObject;
        MoveModel.Instance.Flag[2] = transform.Find("/Canvas/P_Backgound/P_Controller/Scroll/View/Context/P_Middle/Middle").gameObject;
    }
    
    private void  Btn_Start()
    {
        MoveModel.Instance.moveCache.GetFadeIndex();
        PlayerWalk();

        
    }

    private void PlayerWalk()
    {
        Debug.Log("PlayerWalk");

        MoveController.Instance.SetVirtualPlayer(false);
        MoveController.Instance.Walk();
        // 缓存的指令加到存储指令中
        MoveModel.Instance.orderData.AddOrderRange();
       
    }
//  
    public void Btn_Back()
    {
      
        int index = MoveModel.Instance.moveCache.Drog.Count;
        if (index <= 0) return;
        // 后退的类型Drog[index-1]
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
        // 生成的是高亮的状态
        Tool.Instance.Fade(newGo.GetComponent<Image>(), true);
       


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
        // 存储到数据库
        try
        {
            MoveController.Instance.StoreRouteToDB();
        }catch(Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            // view 的复原
            SetDrog(true);
            MoveModel.Instance.FadeArrow(true);
            // viutual 范围归Null
            MoveController.Instance.VirtualToNull();



            // 数据处理
            MoveModel.Instance.importantPosition.Clean();
            MoveModel.Instance.RestartTData();
            MoveController.Instance.CleanData();
            // 障碍物的处理
            MoveModel.Instance.obstacle.Clean();

            // 旗子的恢复
           // FadeFlag(3,true);
        }
       

    }

   


    public void  SetRestartBtn(bool flag)
    {
        btn_Start.interactable = flag;
    }


    public void BtnTip()
    {
        // TimerManager.Instance.StartTimer();
        // MoveController.Instance.StoreRoute();
        MoveController.Instance.LoopHightLightOfRouteArrow();
    }


    public void BtnMusic()
    {
        //TimerManager.Instance.StopTimer();
        MoveController.Instance.LoopHightLightOfArrow();
    }

    /// <summary>
    /// 旗子变淡和恢复  +不能推动
    /// </summary>
    /// <param name="type">  0，1，2 绿 红 蓝旗子 变淡 和恢复 其他 恢复</param>
    public void FadeFlag(int type,  bool flag =false)
    {  
        if (type >=3)
        {
            foreach (GameObject obj in MoveModel.Instance.Flag)
            {
                if (obj == null) continue;
                Tool.Instance.Fade(obj.GetComponent<Image>(), flag);
                obj.GetComponent<Image>().raycastTarget = flag;
            }
        }
        else
        {

            Tool.Instance.Fade(MoveModel.Instance.Flag[type].GetComponent<Image>(), flag);
            MoveModel.Instance.Flag[type].GetComponent<Image>().raycastTarget = flag;
        }
       
    }


  


    

   
}
