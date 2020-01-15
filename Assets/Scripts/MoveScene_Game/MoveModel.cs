using Move;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    prepartion,
    start,
    pressing,// 按钮增加路线状态
    Walking,//人物走动状态
    ArrowHightLight,
    Destination,// 到达终点的状态
    AnimationFlag,
    Fail,// 路劲走完了但是没有成功
}
public class MoveModel : MonoSingleton<MoveModel>
{
    //指令集
    //public  List<int> order = new List<int>();
    // 路线集
   // public List<UnitPosition> route = new List<UnitPosition>();
    public static Dictionary<string, UnitPosition> mapData = new Dictionary<string, UnitPosition>();
    // 在route中的箭头的回收
  //  private List<GameObject> routeObj = new List<GameObject>();
    // 在map上的箭头的回收
   // private List<GameObject> arrowAndPlayerObj = new List<GameObject>();
    
 //   public List<GameObject> RouteObj { get => routeObj; set => routeObj = value; }
    
   // public List<GameObject> ArrowAndPlayerObj { get => arrowAndPlayerObj; set => arrowAndPlayerObj = value; }
    //此时player的方向角度
    public static float rotate = 0f;
    //起点 终点 中继点存储
    public ImportantPosition importantPosition;




    // 数据类
    public MoveCache moveCache= new MoveCache();
    public OrderData orderData = new OrderData();
    // 障碍物的位置的存储
    public  Obstacle obstacle ;
    // 在controller位置 旗子的存储
    private GameObject[] flag;

    [HideInInspector]
    public string sceneId;

    public GameObject[] Flag { get => flag; set => flag = value; }

    private void Awake()
    {
        //  mapData = new Dictionary<string, UnitPosition>();
        obstacle = new Obstacle();

        Flag = new GameObject[3];
        importantPosition = new ImportantPosition();
        
        MoveController.Instance.StartPosEnter += StartPosEnter;
    }

    //开始的点
    private void StartPosEnter(EndDrogMess obj)
    {
        orderData.StoreRoute.Insert(0,obj.Position);
        MoveView.Instance.FadeFlag(3, false);

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 通过名字找到位置（）
    /// </summary>
    /// <param name="name">名字（地图块）</param>
    /// <returns></returns>
    public UnitPosition Fingbyname(string name)
    {
        return mapData.ContainsKey(name) ? mapData[name] : null;
    }
    /// <summary>
    /// 通过位置(中心位置) 找到物体
    /// /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject FindByPosition(Vector3 position)
    {

        foreach (UnitPosition data in mapData.Values)
        {
            if (position == data.Center)
                return data.Go;

        }
        Debug.Log("没有找到物体");
        return null;
    }

    public void ClearData()
    {
        mapData.Clear();
    }

    //public  void cleanRouteObj()
    //{
    //    foreach (GameObject obj in RouteObj)
    //    {
    //        ObjectPool.Instance.CollectObject(obj);
    //    }
    //    routeObj.Clear();
    //}
    //public void cleanArrowAndPlayerObj()
    //{
    //    foreach (GameObject obj in ArrowAndPlayerObj)
    //    {
    //        ObjectPool.Instance.CollectObject(obj);
    //    }
    //    ArrowAndPlayerObj.Clear();

    //}

    public void CleanOrder()
    {
        orderData.CleanCacheOder();
        //order.Clear();
        //route.Clear();
    }

    /// <summary>
    /// 清除需要重新开始的数据
    /// </summary>
    public void RestartTData()
    {
        //cleanRouteObj();
        //cleanArrowAndPlayerObj();
        moveCache.Clean();
        orderData.Clean();
        //order.Clear();
        //route.Clear();
        rotate = 0f;

        // controller 的箭头恢复
        MoveView.Instance.FadeFlag(4, true);
    }


    public  int getLast()
    {
        //第一次的不能算进去
        return orderData.Order.Count >= 2 ? orderData.Order[orderData.Order.Count - 2] : 0;


        //return order[order.Count - 2];// 最后一次的上一次
    }

    /// <summary>
    /// 是不是后退
    /// </summary>
    /// <returns></returns>
    public bool IsBack()
    {
      //  int b = getLast();
        return getLast() == 1 ? true : false;
       
    }


    //public  void FadeArrowInRoute(bool flag)
    //{
    //    for (int i=0;i< routeObj.Count;i++)
    //    {
    //        if(routeObj[i].activeSelf)
    //        {
    //            Tool.Instance.Fade(routeObj[i].GetComponent<Image>(), flag);
               
    //        }
           
    //    }
    //}

    //public void FadeArrowInMap(bool flag)
    //{
    //    for (int i = 0; i < arrowAndPlayerObj.Count; i++)
    //    {
            
    //        if (arrowAndPlayerObj[i].tag!="Player"&& arrowAndPlayerObj[i].activeSelf)
    //        {
    //            Tool.Instance.Fade(arrowAndPlayerObj[i].GetComponent<Image>(), flag);
                
    //        }

    //    }
    //}


    public void FadeArrow(bool flag)
    {
        Debug.Log("FadeArrow");
       moveCache. FadeArrowInRoute(flag);
        moveCache.FadeArrowInMap( flag);
    }

    public void  FlagInControllerIsDorg(bool isDrog)
    {
        foreach (GameObject obj in Flag)
        {
            obj.GetComponent<Image>().raycastTarget = isDrog;
        }
    }






}
