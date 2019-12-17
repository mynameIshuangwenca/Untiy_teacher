using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
   
    TextAsset jsonText;
    private Image image;

    LevelList levelCfgList;
    private LevelCfg levelCfg;
    private ImportantPosition needPosition = new ImportantPosition();

    private Button btn_Return;

    public ImportantPosition NeedPosition { get => needPosition; set => needPosition = value; }

    private void Awake()
    {
        InitObj();

        InitGame();

    }

    private void InitObj()
    {
        btn_Return = transform.Find("/Canvas/P_Background/P_Return").GetComponent<Button>();
        btn_Return.onClick.AddListener(delegate { Tool.Instance.ReturnScene(); });
       
    }

    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
      

      
    }

 private void    InitGame()
    {
        image = transform.Find("/Canvas/P_Background/P_Map/Img_Map").GetComponent<Image>();
        levelCfg = GetLevelCfg();
        Tool.Instance.getSprite(levelCfg.mapSprite, image);
       


    }

    private   LevelCfg  GetLevelCfg()
    {
        string Id;
        LevelCfg levelCfg;
        if ((Id=PlayerPrefs.GetString("Id", null))==null)
        {
            Debug.Log("没有Id存储");
            return null;

        }

        levelCfg = Tool.Instance.GetLevelCfg(Id);
         if(levelCfg!=null)
          {
            return levelCfg;        
        }
        else
        {
            Debug.Log("没有此编号");
            return null;
        }




       
    }

}
