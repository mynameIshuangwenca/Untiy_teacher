using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseMap : MonoSingleton<CourseMap>
{
    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public GameObject coursePrefab;
    private GameObject courseParent;
    private float xOffset;
    private float yOffset;
    private int row = 4;
    private int cow = 5;
    LevelList levelCfgList;


    private void Awake()
    {
        initObj();
        InitCOurseMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void initObj()
    {
        courseParent = transform.Find("/Canvas/P_Background/").gameObject;

    }

    private void InitCOurseMap()
    {
        bool endFlag = false;
        int index = 0;
        levelCfgList = Tool.Instance.getConfig();
        xOffset = Pos2.position.x - Pos1.position.x;
        yOffset = Pos3.position.y - Pos1.position.y;
        Vector3 currentStd = Pos1.position;

        for (int i = 0; i < row && !endFlag; i++)
        {
            
            currentStd = Pos1.position + new Vector3(0, i * yOffset, 0);

            for (int j = 0; j < cow; j++)
            {
                Debug.Log(index);
                if (index > levelCfgList.LevelCfgTable.Count-1)
                {
                    endFlag=true;
                    break;
                }
                Vector3 position = currentStd + new Vector3(j * xOffset, 0, 0);
                GameObject go = Instantiate(coursePrefab, position, new Quaternion(0, 0, 0, 0), courseParent.transform); //根据传入的预设物，生成一个新物体
                go.name = levelCfgList.LevelCfgTable[i * cow + j].Id;
                GetButton(go);
                SetCourse(go, levelCfgList.LevelCfgTable[i * cow + j].mapSprite, levelCfgList.LevelCfgTable[i * cow + j].title);
                index++;




            }
        }
    }


    private void rediretToGame(string Id)
    {
        Debug.Log(Id);
        PlayerPrefs.SetString("Id", Id);
         Tool.Instance.LoadScene(Util.MoveScene);
       
    }


    public void GetButton(GameObject go)
    {

        string Id = go.name;
        Button[] btn = go.GetComponentsInChildren<Button>();

        foreach (Button b in btn)
        {
            b.onClick.AddListener(delegate { rediretToGame(Id); });
        }
    }

    public  void SetCourse(GameObject go, string imgUrl,string tittle)
    {
       
       Tool.Instance.getSprite(imgUrl, go.transform.Find("Img").GetComponent<Image>());
        go.transform.Find("T").GetComponent<Text>().text = tittle;


    }

}
