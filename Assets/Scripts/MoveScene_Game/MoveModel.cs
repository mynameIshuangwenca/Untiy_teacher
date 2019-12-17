using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModel : MonoSingleton<MoveModel>
{
    //指令集
    public  List<int> order = new List<int>();
    // 路线集
    public List<UnitPosition> route = new List<UnitPosition>();
    public static Dictionary<string, UnitPosition> mapData = new Dictionary<string, UnitPosition>();
    // 在route中的箭头的回收
    private List<GameObject> routeObj = new List<GameObject>();
    private List<GameObject> arrowAndPlayerObj = new List<GameObject>();

    public List<GameObject> RouteObj { get => routeObj; set => routeObj = value; }
    public List<GameObject> ArrowAndPlayerObj { get => arrowAndPlayerObj; set => arrowAndPlayerObj = value; }
    //此时player的方向角度
    public static float rotate = 0f;

    private void Awake()
    {
      //  mapData = new Dictionary<string, UnitPosition>();
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

    public  void cleanRouteObj()
    {
        foreach (GameObject obj in RouteObj)
        {
            ObjectPool.Instance.CollectObject(obj);
        }

    }
    public void cleanArrowAndPlayerObj()
    {
        foreach (GameObject obj in ArrowAndPlayerObj)
        {
            ObjectPool.Instance.CollectObject(obj);
        }

    }

    public void CleanOrder()
    {
        order.Clear();
        route.Clear();
    }

    /// <summary>
    /// 清除需要重新开始的数据
    /// </summary>
    public void RestartTData()
    {
        cleanRouteObj();
        cleanArrowAndPlayerObj();
        order.Clear();
        route.Clear();
        rotate = 0f;

    }



}
