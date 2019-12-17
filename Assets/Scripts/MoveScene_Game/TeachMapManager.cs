using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachMapManager : MonoSingleton<TeachMapManager>
{
   
    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public GameObject positionPrefab;
    private GameObject positionParent;
    private float xOffset;
    private float yOffset;
   
    private int row = 4;
    private int cow = 4;




    public int Row { get => row; set => row = value; }
    public int Cow { get => cow; set => cow = value; }
   

    private void Awake()
    {
      
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
        positionParent = transform.Find("/Canvas/P_Backgound/P_Map/Img_Map").gameObject;

    }
    /// <summary>
    /// 初始化地图
    /// </summary>
    private void initMap()
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
                Vector3 position = currentStd + new Vector3(j * xOffset, 0, 0);
                name = Tool.Instance.IJToName(i, j);
                    

                GameObject go = Instantiate(positionPrefab, position, new Quaternion(0, 0, 0, 0), positionParent.transform); //根据传入的预设物，生成一个新物体
                go.name = name;
                UnitPosition unitpositon = new UnitPosition(i, j, position, go,xOffset,yOffset);
               
                if (!MoveModel.mapData.ContainsKey(name))
                {
                    MoveModel.mapData.Add(name, unitpositon);
                }

            }
        }


    }
   
   
}
