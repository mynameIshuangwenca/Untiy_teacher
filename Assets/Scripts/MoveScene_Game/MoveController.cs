﻿using QmDreamer.UI;
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
  

    public EndDrogMess VirtualMess { get => virtualMess; set => virtualMess = value; }


    private void Awake()
    {
        virtRotate = 0;
        //PlayerDrog.EndDrog += this.EndDrog;
        //ArrowDrog.ArrowEndDrog += ArrowEndDrog;
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

    private void InitGame()
    {  

        image = transform.Find("/Canvas/P_Backgound/P_Map/Img_Map").GetComponent<Image>();
        levelCfg = GetLevelCfg();
    
            Tool.Instance.getSprite(levelCfg.mapSprite, image);
        
       
        
        
        



    }
    public  void EndDrog(EndDrogMess endDrogMess)
    {

        Debug.Log("EndDrog" + endDrogMess.Dirt);
        Debug.Log(endDrogMess.Position.Go.name);
        gameState = GameState.start;
       
        if (MoveModel.Instance.moveCache.VirutalPlayer==null)
        {
            virtualPlayer = GenerateVirtualPlayer(virtualPlayerPrefab, endDrogMess.Position.Center, endDrogMess.Position.Go.transform, endDrogMess.Dirt);
            // 加入管理 方便回收
            MoveModel.Instance.moveCache.VirutalPlayer = virtualPlayer;
            // 生成但是隐藏
            virtualPlayer.SetActive(false);
        }
        else
        {
            virtualPlayer.transform.position = endDrogMess.Position.Center;
        }
       

      // 
        //// MoveModel.Instance.ArrowAndPlayerObj.Add(virtualPlayer);
        VirtualMess = endDrogMess;
        originDirt = endDrogMess.Dirt;
        // 深度复制
        //originMess = originDirt.Copy<EndDrogMess>(endDrogMess); // 无法对vector3 序列号
        // originMess = Tool.Clone<EndDrogMess>(endDrogMess);
        //originMess = ETHotfix.DeepCopy.CopyOjbect(endDrogMess as object);









    }

    public  bool ArrowEndDrog(EndDrogMess endDrogMess)
    {

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
       // MoveModel.Instance.ArrowAndPlayerObj.Add(newGo);
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

    public bool PlayerNext(EndDrogMess endDrogMess)
    {
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

        Debug.Log("dasddd" + i + j);
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






    }


    public  void  SetVirtualPlayer(bool flag)
    {
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
    public void BackWalk()
    {
        int index = MoveModel.Instance.orderData.Order.Count;
        if (index <= 0) return;// 没有走动
        // 错误的步骤  
        if(MoveModel.Instance.orderData.Route[index - 1]==null)
        {
            // 2 数据的后退
            MoveModel.Instance.orderData.BackWalk();
            return;
        }

        //1 、 动作退回
        if (MoveModel.Instance.orderData.Order[index-1] == 0 || MoveModel.Instance.orderData.Order[index-1] == 1)
        {// 后退到 倒数第二步
            if (index==1)
            {
                // 只要一步 回到player位置

                virtualPlayer.transform.position = player.transform.position;
            }
            else
            {
                virtualPlayer.transform.position = MoveModel.Instance.orderData.Route[index - 2].Center;
            }
            
        }
            
        else if (MoveModel.Instance.orderData.Order[index-1] == 2)
        {// 向左转了
            VTurnRight();
        }
        else
        {// 向右转了;
            VTurnLeft();
        }
        // 4 vitural 的位置后退
        if(index ==1)
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

}