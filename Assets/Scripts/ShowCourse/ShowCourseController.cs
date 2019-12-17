using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCourseController : MonoSingleton<ShowCourseController>
{
    public GameObject coursePrefab;
    private GameObject courseParent;
    private  LevelList levelCfgList;
    // Start is called before the first frame update


    private void Awake()
    {
        initObj();
       
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initObj()
    {
        levelCfgList = Tool.Instance.getConfig();
        courseParent = transform.Find("/Canvas/P_Background/P_Course").gameObject;
        for (int i=0;i< levelCfgList.LevelCfgTable.Count;i++)
        {
            GameObject go = Instantiate(coursePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), courseParent.transform); //根据传入的预设物，生成一个新物体
            go.name = levelCfgList.LevelCfgTable[i].Id;
            
            GetButton(go);
            SetCourse(go, levelCfgList.LevelCfgTable[i].mapSprite, levelCfgList.LevelCfgTable[i].title);
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

    public void SetCourse(GameObject go, string imgUrl, string tittle)
    {

        Tool.Instance.getSprite(imgUrl, go.transform.Find("Img").GetComponent<Image>());
        go.transform.Find("T").GetComponent<Text>().text = tittle;


    }

}
