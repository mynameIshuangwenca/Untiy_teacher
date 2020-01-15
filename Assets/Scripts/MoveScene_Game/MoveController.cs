using QmDreamer.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class MoveController : MonoSingleton<MoveController>
{

    public GameState gameState;
    public GameObject virtualPlayerPrefab;
    private GameObject virtualPlayer;
    private EndDrogMess virtualMess;
    // 初始位置
    private int  originDirt; 
    private static float virtRotate = 0;
    public Sprite[] arrowSprite;
    public GameObject[] dirtPrefabs;
    public Transform arrowParent;

    public Sprite[] playerSprite;
    // 保存重置时的图片闪烁
   


    //动态加载需要的数据
    private Image image;
    LevelList levelCfgList;
    private LevelCfg levelCfg;

    [HideInInspector]
    public GameObject player;


    public GameObject tooltipUI;
    //声明委托
    public  Action<EndDrogMess> StartPosEnter;
   

    public EndDrogMess VirtualMess { get => virtualMess; set => virtualMess = value; }


    private void Awake()
    {
        virtRotate = 0;
      ;
        gameState = GameState.prepartion;
        InitGame();
    


    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

       
    }
  
       
            //设置间隔时间为10秒
          
        


 private void InitGame()
    {  

        image = transform.Find("/Canvas/P_Backgound/P_Map/Img_Map").GetComponent<Image>();
        levelCfg = GetLevelCfg();
    
            Tool.Instance.getSprite(levelCfg.mapSprite, image);
        
       
        
        
        



    }
    public  void EndDrog(EndDrogMess endDrogMess)
    {

        FirstPlayerInMap();
        Debug.Log(endDrogMess.Position.Go.name);
        gameState = GameState.start;
       
        if (MoveModel.Instance.moveCache.VirutalPlayer==null)
        {
            virtualPlayer = GenerateVirtualPlayer(virtualPlayerPrefab, endDrogMess.Position.Center, MoveView.Instance.CacheParent, endDrogMess.Dirt);
            // 加入管理 方便回收
            MoveModel.Instance.moveCache.VirutalPlayer = virtualPlayer;
            // 生成但是隐藏
            virtualPlayer.SetActive(false);
         
        }
        else
        {
            virtualPlayer.transform.position = endDrogMess.Position.Center;
        }
       
    
        //virtualPlayer.transform.SetAsLastSibling();
        // 作为开始的地址  委托
          StartPosEnter(endDrogMess);
        
         VirtualMess = endDrogMess;
        originDirt = endDrogMess.Dirt;
    }

    public  bool ArrowEndDrog(EndDrogMess endDrogMess)
    {

        FirstArrowInMap();
        Debug.Log("ArrowEndDrog" + endDrogMess.Dirt);
        Debug.Log(endDrogMess.Position.Go.name);
        // 没有越界 
        // dirt  PlayerNext中已经修改了
        int dirt = virtualMess.Dirt;//

        bool flag = PlayerNext(endDrogMess);
        if (flag)
        {
            // 显示箭头
            GetArrowPosition(endDrogMess, dirt);
        }
        return flag;



       


    }

    private void GetArrowPosition(EndDrogMess endDrogMess, int dirt)
    {
        GameObject newGo;
        Vector3 arrowPosition;
        int pos;
       
        if (MoveModel.Instance.IsBack())
        {
            pos = Util.bDirtToPos[endDrogMess.Dirt][dirt];
        }

        else
        {
            pos = Util.dirtToPos[endDrogMess.Dirt][dirt];
        }
       
        if (pos >= 4)
        {
            // 转向
            arrowPosition = MoveModel.mapData[endDrogMess.Position.Go.name].Center;
           

            newGo = ObjectPool.Instance.CreateObject(dirtPrefabs[0].name, dirtPrefabs[0], arrowPosition, arrowParent);
            // 使生成的名字一致 对象池
            newGo.name = dirtPrefabs[0].name;

        }
        else if (pos == 0)
        {
            arrowPosition = MoveModel.mapData[endDrogMess.Position.Go.name].Top;
            newGo = ObjectPool.Instance.CreateObject(dirtPrefabs[1].name, dirtPrefabs[1], arrowPosition, arrowParent);
            newGo.name = dirtPrefabs[1].name;
        }
        else if (pos == 1)
        {
            arrowPosition = MoveModel.mapData[endDrogMess.Position.Go.name].Bottom;
            newGo = ObjectPool.Instance.CreateObject(dirtPrefabs[1].name, dirtPrefabs[1], arrowPosition, arrowParent);
            newGo.name = dirtPrefabs[1].name;
        }
        else if (pos == 2)
        {
            arrowPosition = MoveModel.mapData[endDrogMess.Position.Go.name].Left;
            newGo = ObjectPool.Instance.CreateObject(dirtPrefabs[2].name, dirtPrefabs[2], arrowPosition, arrowParent);
            newGo.name = dirtPrefabs[2].name;
        }
        else
        {
            arrowPosition = MoveModel.mapData[endDrogMess.Position.Go.name].Right;
            newGo = ObjectPool.Instance.CreateObject(dirtPrefabs[2].name, dirtPrefabs[2], arrowPosition, arrowParent);
            newGo.name = dirtPrefabs[2].name;
        }
       
          newGo.GetComponent<Image>().sprite = arrowSprite[pos];

        // 存储新的箭头
        MoveModel.Instance.moveCache.ArrowMap.Add(newGo);
      
    }


    
    private GameObject GenerateVirtualPlayer(GameObject prefab, Vector3 postion, Transform parent, int dirt)
    {
        GameObject go = ObjectPool.Instance.CreateObject("virtual", prefab, postion, parent);
        //不能直接修改color.a
        // go.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        go.GetComponent<Image>().raycastTarget = false;
        go.name = "virtual";
        go.GetComponent<Image>().sprite = playerSprite[dirt];
      
        return go;


    }
    /// <summary>
    /// 根据箭头模拟下一步
    /// </summary>
    /// <param name="endDrogMess">箭头结束传过来的信息</param>
    /// <returns> true 可以有下一步  false：下一步是越界</returns>
    public bool PlayerNext(EndDrogMess endDrogMess)
    {
       int pos = Util.bDirtToPos[endDrogMess.Dirt][virtualMess.Dirt];
        if(pos<4)// 是 上下左右 不是拐弯
        {
            // 判断是否有障碍物  注意：有障碍物不会加入指令
            int type = MoveModel.Instance.obstacle.QuaryObstacle(endDrogMess.Position.Go.name, pos);
            if (type > 0)// 有障碍物或者其他类型==不能通过
            {
                Debug.Log("有障碍物");
                AudioManager.Instance.PlaySound(11);

                return false;
            }
        }
      


        // Debug.Log("PlayerNext      " + endDrogMess.Position.I + endDrogMess.Position.J);
        int i = endDrogMess.Position.I;
        int j = endDrogMess.Position.J;
        string name = Tool.Instance.IJToName(i, j);
        MoveModel.Instance.orderData.Order.Add(endDrogMess.Dirt);

        if (virtualMess.Dirt == 0)
        {
            if (endDrogMess.Dirt == 0 && ++i < TeachMapManager.Instance.Row)
            {

                name = Tool.Instance.IJToName(i, j);
            }
            else if (endDrogMess.Dirt == 1 && --i >= 0)
            {
                name = Tool.Instance.IJToName(i, j);
            }
            else if (endDrogMess.Dirt == 2)
            {
                virtualMess.Dirt = Tool.Instance.TurnLeft(virtualMess.Dirt);
                VTurnLeft();
                // virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                //  virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnRight();
            }

            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.orderData.Route.Add(null);
                return false;
            }



        }
        else if (virtualMess.Dirt == 1)
        {
            if (endDrogMess.Dirt == 0 && --i >= 0)
            {
                name = Tool.Instance.IJToName(i, j);
            }
            else if (endDrogMess.Dirt == 1 && ++i < TeachMapManager.Instance.Row)
            {
                name = Tool.Instance.IJToName(i, j);

            }
            else if (endDrogMess.Dirt == 2)
            {
                virtualMess.Dirt = Tool.Instance.TurnLeft(virtualMess.Dirt);
                // virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnLeft();
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                //virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnRight();
            }

            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.orderData.Route.Add(null);
                return false;
            }
        }
        else if (virtualMess.Dirt == 2)
        {
            if (endDrogMess.Dirt == 0 && ++j < TeachMapManager.Instance.Cow)
            {
                name = Tool.Instance.IJToName(i, j);

            }
            else if (endDrogMess.Dirt == 1 && --j >= 0)
            {
                name = Tool.Instance.IJToName(i, j);
            }
            else if (endDrogMess.Dirt == 2)
            {
                virtualMess.Dirt = Tool.Instance.TurnLeft(virtualMess.Dirt);
                // virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnLeft();
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                // virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnRight();
            }
            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.orderData.Route.Add(null);
                return false;
            }


        }
        else
        {
            if (endDrogMess.Dirt == 0 && --j >= 0)
            {
                name = Tool.Instance.IJToName(i, j);
            }
            else if (endDrogMess.Dirt == 1 && ++j < TeachMapManager.Instance.Cow)
            {
                name = Tool.Instance.IJToName(i, j);
            }
            else if (endDrogMess.Dirt == 2)
            {
                virtualMess.Dirt = Tool.Instance.TurnLeft(virtualMess.Dirt);
                //virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnLeft();
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                //virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                VTurnRight();
            }
            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.orderData.Route.Add(null);
                return false;
            }


        }

        
        MoveModel.Instance.orderData.Route.Add(MoveModel.Instance.Fingbyname(name));
        virtualMess.Position = MoveModel.Instance.Fingbyname(name);
        virtualPlayer.transform.position = virtualMess.Position.Center;
        return true;






    }

    private LevelCfg GetLevelCfg()
    {
        string Id;
        LevelCfg levelCfg;
        if ((Id = PlayerPrefs.GetString("Id", null)) == null)
        {
            Debug.Log("没有Id存储");
            return null;

        }

        MoveModel.Instance.sceneId = Id;

        levelCfg = Tool.Instance.GetLevelCfg(Id);
        if (levelCfg != null)
        {
            return levelCfg;
        }
        else
        {
            Debug.Log("没有此编号");
            return null;
        }
    }
    /// <summary>
    /// 虚拟player 左转
    /// </summary>
    private void VTurnLeft()
    {
       virtRotate += 90;
        virtualPlayer.transform.DOLocalRotate(new Vector3(0, 0, virtRotate), 3f);
    }
    /// <summary>
    /// 虚拟player 右转
    /// </summary>
    private void VTurnRight()
    {
        virtRotate -= 90;
        virtualPlayer.transform.DOLocalRotate(new Vector3(0, 0, virtRotate), 3f);
    }

    private void TurnLeft()
    {
       MoveModel.rotate += 90;
        player.transform.DOLocalRotate(new Vector3(0, 0, MoveModel.rotate), 3f);
    }

    private void TurnRight()
    {
        MoveModel.rotate -= 90;
        player.transform.DOLocalRotate(new Vector3(0, 0, MoveModel.rotate), 3f);
    }

    // player 开始walk
    public void Walk()
    {
        Walk(BeforeWalk);
    }

    public void Walk(Action BeforeAction = null, Action AfterAction = null)
    {
        if (BeforeAction != null)
        {
            BeforeAction();
        }
        // 在btn监听中不能关闭btn 
        //BeforeWalk();
        Sequence seq = DOTween.Sequence();
        for (int i=0;i<MoveModel.Instance.orderData.Order.Count;i++)
        {
            if(MoveModel.Instance.orderData.Route[i]==null)
            {
                continue;
            }
            if (MoveModel.Instance.orderData.Order[i] == 0 || MoveModel.Instance.orderData.Order[i] == 1)
                seq.Append(player.transform.DOMove(MoveModel.Instance.orderData.Route[i].Center, 2f).OnComplete(()=> { })).AppendInterval(0.5f);
            else if(MoveModel.Instance.orderData.Order[i] == 2)
            {
                seq.Append(player.transform.DOLocalRotate(new Vector3(0, 0, MoveModel.rotate += 90), 2f)).AppendInterval(0.5f);
               
            }
            else
            {
                seq.Append(player.transform.DOLocalRotate(new Vector3(0, 0, MoveModel.rotate -= 90), 2f)).AppendInterval(0.5f);
               
            }

            ReduceOrder();


        }

        seq.AppendCallback(()=> {
            AfterWalk();
        });
        if (AfterAction != null)
        {
            AfterAction();
        }
       // AfterWalk();
    }

   /// <summary>
   /// 物体开始走的时候 要干的事情
   /// </summary>
    private  void BeforeWalk()
    {
        MoveView.Instance.SetRestartBtn(false);

        gameState = GameState.Walking;
        if(MoveModel.Instance.orderData.Order.Count!=0)
        {
            AudioManager.Instance.PlayMusic(1);
        }

        StopCoroutine("HightLightOfArrow");
       
    }

/* 
 walk 结束后的动作
         */   
    private void AfterWalk()
    {
        Debug.Log("AfterWalk");
        MoveView.Instance.SetRestartBtn(true);
        // 返回按键的界面
        gameState = GameState.pressing;
        int index = MoveModel.Instance.orderData.Route.Count;
       // 不能最后为Null   

        if (index>0 )
            {
                int i = 0;

            //  找到不为null 做为父位置
                while (i<=index && MoveModel.Instance.orderData.Route[index - i-1]==null)
                {
                    i++;
                }

                if (MoveModel.Instance.orderData.Route[index - i-1]!=null )
                {
                // 把player 的parent 设置为最后的位置
                player.transform.parent = MoveModel.Instance.orderData.Route[index - i - 1].Go.transform;
               }
           

        }
        MoveModel.Instance.CleanOrder();
        MoveModel.Instance.FadeArrow(false);
        //关闭音乐
        AudioManager.Instance.StopMusic();


        if (gameState == GameState.pressing && MoveModel.Instance.moveCache.FadeIndex != 0)
        {
            LoopHightLightOfArrow();
        }

    }

   /// <summary>
   ///  player 走后从后面减少箭头指令  为了 后退功能
   /// </summary>
    public void ReduceOrder()
    {
        MoveModel.Instance.moveCache.RedureLastOrderInFlag();
    }


    public void SetVirtualPlayer(bool flag)
    {
        virtualPlayer.transform.SetAsLastSibling();

        if (!virtualPlayer)
            return;
        virtualPlayer.SetActive(flag);

    }

    public  void CleanData()
    {
        virtRotate = 0;
    }


    /// <summary>
    /// 后退一步
    /// </summary>
    public void BackWalk(int index=0)
    {
        
       if (index ==0)
        {
            OrderBack();
        }
        else if (index==1)
        {
            FlagBack();
        }
       else if (index ==2)
        {
            ObstacleBack();
        }
       else if(index==3)
        {
            CellObstacleBack();
        }
       else 
        {

        }
      
    }

   

    // 指令后退
    public void  OrderBack()
    {
        int index = MoveModel.Instance.orderData.Order.Count;
        if (index <= 0) return;// 没有走动
        // 错误的步骤  
        if (MoveModel.Instance.orderData.Route[index - 1] == null)
        {
            // 2 数据的后退
            MoveModel.Instance.orderData.BackWalk();
            return;
        }

        //1 、 动作退回
        if (MoveModel.Instance.orderData.Order[index - 1] == 0 || MoveModel.Instance.orderData.Order[index - 1] == 1)
        {// 后退到 倒数第二步
            if (index == 1)
            {
                // 只要一步 回到player位置

                virtualPlayer.transform.position = player.transform.position;
            }
            else
            {
                virtualPlayer.transform.position = MoveModel.Instance.orderData.Route[index - 2].Center;
            }

        }

        else if (MoveModel.Instance.orderData.Order[index - 1] == 2)
        {// 向左转了
            VTurnRight();
        }
        else
        {// 向右转了;
            VTurnLeft();
        }
        // 4 vitural 的位置后退
        if (index == 1)
        {
            virtualMess.Dirt = originDirt;
        }
        else
        {
            virtualMess.Dirt = MoveModel.Instance.orderData.Order[index - 2];
            virtualMess.Position = MoveModel.Instance.orderData.Route[index - 2];
        }
        //virtualMess.Dirt =

        // 2 数据的后退
        MoveModel.Instance.orderData.BackWalk();

        // 3 箭头的后退
        MoveModel.Instance.moveCache.BackWalk();

    }

    //旗子后退
    public void FlagBack()
    {
        int index = MoveModel.Instance.moveCache.Flag.Count;
        if (index <= 0) return;
        //1、 controller上的恢复
        GameObject go = MoveModel.Instance.moveCache.Flag[index - 1];
        MoveView.Instance.FadeFlag(Util.flagType[go.name], true);

            //2、map上的物体去掉
        MoveModel.Instance.moveCache.Flag.RemoveAt(index - 1);
        // 回收 
        ObjectPool.Instance.CollectObject(go);
        
    }



    public void  ObstacleBack()
    {
        int index = MoveModel.Instance.moveCache.Obstacle.Count;
        if (index <= 0) return;

        // 障碍物的存放清除
        GameObject go = MoveModel.Instance.moveCache.Obstacle[index - 1];
        MoveModel.Instance.moveCache.Obstacle.RemoveAt(index - 1);

        // 销毁此物体
        go.GetComponent<ObstacleDrog>().GoDestroy();
    }

/// <summary>
/// cell障碍物的后退
/// </summary>
    private void CellObstacleBack()
    {
        int index = MoveModel.Instance.moveCache.Obstacle.Count;
        if (index <= 0) return;
        // 障碍物的存放清除
        GameObject go = MoveModel.Instance.moveCache.Obstacle[index - 1];
        MoveModel.Instance.moveCache.Obstacle.RemoveAt(index - 1);// 销毁此物体
        go.GetComponent<CellObstacleDrog>().GoDestroy();
    }

    public void OutIndex()
    {
        //提示和播放音乐
        Debug.Log("越界了");
        AudioManager.Instance.PlaySound(11);
    }

    // 提示框出现
    public void  FlagOnter(Vector3 postion ,string mess)
    {
        tooltipUI.GetComponent<TooltipUI>().Show(postion, mess);
    }
    /// <summary>
    /// /提示框消失
    /// </summary>
    public void FlagExit()
    {
        tooltipUI.GetComponent<TooltipUI>().Hide();
    }


    public void VirtualToNull()
    {
        virtualMess = null;
    }

    public  bool IsVirtual(string name)
    {
        if (virtualMess == null) return false;
        if (name==virtualMess.Position.Go.name)
        {
            return true;
        }
        return false;
    }



    public void StoreRoute()
    {
        //判断路径是否大于1
        if (MoveModel.Instance.orderData.Route.Count <= 1)
        {
            Debug.Log("没有路径");
            return;
        }
        //获取使用此app 的用户
          string userName=  PlayerPrefs.GetString("userName", string.Empty);
        if (userName == string.Empty)
        {
            Debug.Log("用户不存在");
            return;
        }
        //得到路径的字符串
        string route = MoveModel.Instance.orderData.GetRoute();
        //保存到数据库
        SQLController.Instance.InsertRoute(userName,route);

    }

   
    public void StoreRouteToDB()
    {
        //判断路径是否大于1
        if (MoveModel.Instance.orderData.RouteOrder.Count <1)
        {
            Debug.Log("没有路径");
            return;
        }
        //获取使用此app 的用户
        string userName = PlayerPrefs.GetString("userName", string.Empty);
        if (userName == string.Empty)
        {
            Debug.Log("用户不存在");
            return;
        }
        RouteInfo routeInfo = new RouteInfo(0, MoveModel.Instance.sceneId, MoveModel.Instance.importantPosition.GetStartStr(), MoveModel.Instance.importantPosition.GetMiddleStr(), MoveModel.Instance.importantPosition.GetDestinationStr(), MoveModel.Instance.orderData.GetStoreRoute(), MoveModel.Instance.orderData.OrderToStr(),MoveModel.Instance.orderData.GetMiddleStepStr());
        SQLController.Instance.InsertRoute(userName, routeInfo);
    }

    /// <summary>
    /// route上的箭头高亮一遍 如跑马灯
    /// </summary>
    public void LoopHightLightOfRouteArrow()
    {
        StartCoroutine(HightLightOfRouteArrow());
    }

    IEnumerator HightLightOfRouteArrow()
    {
        GameObject preObj=null;
        foreach (GameObject obj in MoveModel.Instance.moveCache.ArrowRoute)
        {
            if (obj == null) continue;
            if  (preObj!=null && preObj.activeSelf)
            {
                Tool.Instance.Fade(preObj.GetComponent<Image>(), false);
            }
            if (obj.activeSelf)
            {
                Tool.Instance.Fade(obj.GetComponent<Image>(), true);
                preObj = obj;
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        // 最后一个
        if(preObj  != null)
        {
            Tool.Instance.Fade(preObj.GetComponent<Image>(), false);
        }
       

    }

    /// <summary>
    /// 跑马灯开始的操作
    /// </summary>
    private void BeforeArrowLight()
    {
       
        gameState = GameState.ArrowHightLight;
    }
    /// <summary>
    /// 跑马灯结束时的操作
    /// </summary>
    private void AfterArrowLight()
    {
        gameState = GameState.pressing;
    }

    /// <summary>
    /// map 和route 上的箭头一起高亮 跑马灯
    /// </summary>
    /// <param name="BeforeAction">开始的时的方法</param>
    /// <param name="AfterAction">结束时的方法</param>
    public void LoopHightLightOfArrow()
    {
        StartCoroutine("HightLightOfArrow");
    }

    /// <summary>
    /// map 和route 上的箭头一起高亮 跑马灯
    /// </summary>
    /// <param name="mapArrow"></param>
    /// <param name="routeArrow"></param>
    /// <param name="BeforeAction"></param>
    /// <param name="AfterAction"></param>
    /// <returns></returns>

    IEnumerator HightLightOfArrow()
    {
        GameObject preMapArrow = null;
        GameObject preRouteArrow = null;
        List<GameObject> mapArrow = MoveModel.Instance.moveCache.ArrowMap;
        List<GameObject> routeArrow = MoveModel.Instance.moveCache.ArrowRoute;
        int fadeIndex = MoveModel.Instance.moveCache.FadeIndex;
        BeforeArrowLight();



        while (MoveModel.Instance.moveCache.FadeIndex!=0)
        {
            for (int i = 0; i < fadeIndex; i++)
            {
                if (MoveModel.Instance.moveCache.FadeIndex != fadeIndex) break;
                if (mapArrow[i] == null || routeArrow[i] == null) continue;
                if (preMapArrow != null && preMapArrow.activeSelf)
                {
                    Tool.Instance.Fade(preMapArrow.GetComponent<Image>(), false);
                }
                if (preRouteArrow != null && preRouteArrow.activeSelf)
                {
                    Tool.Instance.Fade(preRouteArrow.GetComponent<Image>(), false);
                }
                if (mapArrow[i].activeSelf)
                {
                    Tool.Instance.Fade(mapArrow[i].GetComponent<Image>(), true);
                    preMapArrow = mapArrow[i];
                }
                if (routeArrow[i].activeSelf)
                {
                    Tool.Instance.Fade(routeArrow[i].GetComponent<Image>(), true);
                    preRouteArrow = routeArrow[i];
                }
                yield return new WaitForSeconds(0.5f);

            }
            // 最后一个
            if (preMapArrow != null || preRouteArrow != null)
            {
                Tool.Instance.Fade(preMapArrow.GetComponent<Image>(), false);
                Tool.Instance.Fade(preRouteArrow.GetComponent<Image>(), false);
            }
            yield return new WaitForSeconds(1f);
        }




        //if (AfterAction!=null)
        //{
        //    AfterAction();
        //}
        AfterArrowLight();

    }


    /// <summary>
    /// player放到map 中的时候的操作
    /// </summary>
    public void FirstPlayerInMap()
    {

        //1、 其他的player变淡

        // 旗子不能拖动了
        MoveModel.Instance.moveCache.IsDrogFlag(false);
       
        //

    }

    /// <summary>
    /// 第一个箭头放到Map 中
    /// </summary>
    public void FirstArrowInMap()
    {
        if(MoveModel.Instance.moveCache.ArrowMap.Count==0)
        {
            //1 在map中的player 不能拖动了 只能dotween

            MoveModel.Instance.moveCache.IsDrogPlayer(false);
        }
       


    }



  

}