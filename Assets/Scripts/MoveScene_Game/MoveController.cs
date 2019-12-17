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
      
        PlayerDrog.EndDrog += this.EndDrog;
        ArrowDrog.ArrowEndDrog += ArrowEndDrog;
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
        virtualPlayer = GenerateVirtualPlayer(virtualPlayerPrefab, endDrogMess.Position.Center, endDrogMess.Position.Go.transform, endDrogMess.Dirt);

        VirtualMess = endDrogMess;
        // 加入管理 方便回收
        MoveModel.Instance.ArrowAndPlayerObj.Add(virtualPlayer);



    }

    public  void ArrowEndDrog(EndDrogMess endDrogMess)
    {

        Debug.Log("ArrowEndDrog" + endDrogMess.Dirt);
        Debug.Log(endDrogMess.Position.Go.name);
        // 没有越界 
        // dirt  PlayerNext中已经修改了
        int dirt = virtualMess.Dirt;//
        if (PlayerNext(endDrogMess))
        {
            // 显示箭头
            GetArrowPosition(endDrogMess, dirt);
        }



       


    }

    private void GetArrowPosition(EndDrogMess endDrogMess, int dirt)
    {
        GameObject newGo;
        Vector3 arrowPosition;
        Debug.Log("xxx" + Util.dirtToPos[endDrogMess.Dirt][dirt]);
        int pos = Util.dirtToPos[endDrogMess.Dirt][dirt];
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
        MoveModel.Instance.ArrowAndPlayerObj.Add(newGo);
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
        MoveModel.Instance.order.Add(endDrogMess.Dirt);

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
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
                // virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                //  virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }

            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.route.Add(null);
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
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                //virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }

            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.route.Add(null);
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
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                // virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }
            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.route.Add(null);
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
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }
            else if (endDrogMess.Dirt == 3)
            {
                virtualMess.Dirt = Tool.Instance.TurnRight(virtualMess.Dirt);
                //virtualPlayer.GetComponent<Image>().sprite = playerSprite[virtualMess.Dirt];
                virtualPlayer.transform.DORotate(new Vector3(0, 0, Util.virtTD[virtualMess.Dirt]), 2f);
            }
            else
            {
                Debug.Log("越界了");
                AudioManager.Instance.PlaySound(11);
                MoveModel.Instance.route.Add(null);
                return false;
            }


        }

        Debug.Log("dasddd" + i + j);
        MoveModel.Instance.route.Add(MoveModel.Instance.Fingbyname(name));
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
        for (int i=0;i<MoveModel.Instance.order.Count;i++)
        {
            if(MoveModel.Instance.route[i]==null)
            {
                continue;
            }
            if (MoveModel.Instance.order[i] == 0 || MoveModel.Instance.order[i] == 1)
                seq.Append(player.transform.DOMove(MoveModel.Instance.route[i].Center, 2f).OnComplete(()=> { })).AppendInterval(0.5f);
            else if(MoveModel.Instance.order[i] == 2)
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

   
    private  void BeforeWalk()
    {
        MoveView.Instance.SetRestartBtn(false);

       
    }

        
    private void AfterWalk()
    {
        MoveModel.Instance.CleanOrder();
        MoveView.Instance.SetRestartBtn(true);
    }


    public  void  SetVirtualPlayer(bool flag)
    {
        if (!virtualPlayer)
            return;
        virtualPlayer.SetActive(flag);
    }


    

}