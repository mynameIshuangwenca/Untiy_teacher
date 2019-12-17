using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMamager : MonoSingleton<MapMamager>
{

    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public GameObject positionPrefab;
    private GameObject positionParent;
    private float xOffset;
    private float yOffset;
    private Dictionary<string, UnitPosition> mapData;
    private int row = 4;
    private int cow = 4;
    

  
 
    public int Row { get => row; set => row = value; }
    public int Cow { get => cow; set => cow = value; }
    public float XOffset { get => xOffset; set => xOffset = value; }
    public float YOffset { get => yOffset; set => yOffset = value; }
    public Dictionary<string, UnitPosition> MapData { get => mapData; set => mapData = value; }

    private void Awake()
    {
        mapData = new Dictionary<string, UnitPosition>();
        initObj();
        initMap();
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
        positionParent =  transform.Find("/Canvas/P_Background/P_Map").gameObject;

    }
    /// <summary>
    /// 初始化地图
    /// </summary>
    private void  initMap()
    {
        string name;
        xOffset = Pos2.position.x - Pos1.position.x;
        yOffset = Pos3.position.y - Pos1.position.y;
        Vector3 currentStd = Pos1.position;

        for (int i = 0; i < Row; i++)
        {
            currentStd = Pos1.position + new Vector3(0, i * yOffset, 0);

            for (int j = 0; j < Cow; j++)
            {
                Vector3 position = currentStd + new Vector3(j * XOffset, 0, 0);
                name = positionParent.name +"_" +i + "_" + j;
               
               GameObject go= Instantiate(positionPrefab, position, new Quaternion(0, 0, 0, 0), positionParent.transform); //根据传入的预设物，生成一个新物体
               go.name = name;
                UnitPosition unitpositon = new UnitPosition(i, j, position,go);
                mapData.Add(name, unitpositon);
            }
        }


    }
    /// <summary>
    /// 通过名字找到位置（中心位置）
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
}
