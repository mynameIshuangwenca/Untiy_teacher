using QmDreamer.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveView : MonoSingleton<MoveView>
{

    private GameObject playerParent;
    private GameObject GridLayer;
    public  Button btn_Reload;
    public GameObject routePrefab;
    public Sprite[] routeDirtSpite;
    private Button btn_Start;
    private Button btn_End;
    private Button btn_Restart;
    

    private List<GameObject> myGo=new List<GameObject>();


    
    private void Awake()
    {
        PlayerDrog.EndDrog += EndDrog;
        ArrowDrog.ArrowEndDrog += ArrowEndDrog;
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
       
    }
    private void InitFun()
    {

        btn_Reload.onClick.AddListener(delegate { ReturnScene(); });
        btn_Start.onClick.AddListener(delegate { PlayerWalk(); });
        btn_End.onClick.AddListener(delegate { Btn_End(); });
        btn_Restart.onClick.AddListener(delegate { ReloadScene(); });
         
       
        
           

    }

   
    private void InitObj()
    {
        playerParent = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Player").gameObject;
        GridLayer = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Route/Img_Route").gameObject;
        btn_Start = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Btn/Img_Start").GetComponent<Button>();
        btn_End = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Btn/Img_End").GetComponent<Button>();
        btn_Restart = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Btn/Img_Restart").GetComponent<Button>();
        myGo.Add(playerParent);
        myGo.Add(GridLayer);




    }
    private void PlayerWalk()
    {
        Debug.Log("PlayerWalk");

        MoveController.Instance.SetVirtualPlayer(false);
        MoveController.Instance.Walk();
       
    }

    public void Btn_End()
    {
        MoveController.Instance.SetVirtualPlayer(false);
    }
    public void EndDrog(EndDrogMess endDrogMess)
    {
        SetDrog(false);
    }


    private void SetDrog(bool flag)
    {
        
        Color color;
        Image[] images = playerParent.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            color = img.color;
            if (flag)
            {
                img.color = new Color(color.r, color.g, color.b, color.a * 2);
            }
            else
            {
                img.color = new Color(color.r, color.g, color.b, color.a / 2);
            }
           
            img.raycastTarget = flag;
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
        MoveModel.Instance.RouteObj.Add(newGo);
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
        // 数据处理
        MoveModel.Instance.RestartTData();
      //  MoveModel.Instance.ClearData();

        // view 的复原
        SetDrog(true);
        //Tool.Instance.Reload();

        //MoveModel.Instance.ClearData();



        //SetDrog(true);
        //MoveModel.Instance.RestartTData();

    }


    public void  SetRestartBtn(bool flag)
    {
        btn_Start.interactable = flag;
    }



   
}
