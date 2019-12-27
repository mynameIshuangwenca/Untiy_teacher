using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public enum GameState
{
    prepartion,
    start,
    pressing,// 按钮增加路线状态
    Walking,//人物走动状态
    Destination,// 到达终点的状态
    AnimationFlag,
    Fail,// 路劲走完了但是没有成功
}

[Serializable]
public class UnitPosition
{

    int i;
    int j;
    Vector3 center;
    Vector3 top;
    Vector3 bottom;
    Vector3 left;
    Vector3 right;
    Vector3 northEast;
    Vector3 southEast;
    Vector3 southWest;
    Vector3 northWest;
    private GameObject go;

    public  UnitPosition()
    {

    }



    public UnitPosition(int i, int j)
    {
        this.I = i;
        this.J = j;
    }

    public UnitPosition(int i, int j, Vector3 center) : this(i, j)
    {
        this.Center = center;
        this.Left = center + new Vector3(MapMamager.Instance.XOffset * 0.5f, 0,0);
        this.Right = center - new Vector3(MapMamager.Instance.XOffset * 0.5f, 0,0);
        this.Top = center + new Vector3(0,MapMamager.Instance.YOffset * 0.5f,0);
        this.Bottom = center - new Vector3(0,MapMamager.Instance.YOffset * 0.5f, 0);

    }

    public UnitPosition(int i, int j, Vector3 center, GameObject go) : this(i, j, center)
    {
        this.Go = go;
    }

    public UnitPosition(int i, int j, Vector3 center, GameObject go,float xoffset,float yoffset) : this(i, j)
    {
        this.Center = center;
        this.Left = center + new Vector3(xoffset * 0.5f, 0, 0);
        this.Right = center - new Vector3(xoffset * 0.5f, 0, 0);
        this.Top = center + new Vector3(0, yoffset * 0.5f, 0);
        this.Bottom = center - new Vector3(0, yoffset * 0.5f, 0);

        this.NorthEast = (this.center + new Vector3(right.x, top.y, 0)) / 2;
        SouthEast = ((this.center + new Vector3(right.x, bottom.y, 0)) / 2);
        SouthWest = ((this.center + new Vector3(left.x, bottom.y, 0)) / 2);
        NorthWest = ((this.center + new Vector3(left.x, top.y, 0)) / 2);
        this.Go = go;

    }

    public Vector3 Center { get => center; set => center = value; }
    public int I { get => i; set => i = value; }
    public int J { get => j; set => j = value; }
    public Vector3 Top { get => top; set => top = value; }
    public Vector3 Bottom { get => bottom; set => bottom = value; }
    public Vector3 Left { get => left; set => left = value; }
    public Vector3 Right { get => right; set => right = value; }
    public GameObject Go { get => go; set => go = value; }
    public Vector3 NorthEast { get => northEast; set => northEast = value; }
    public Vector3 SouthEast { get => southEast; set => southEast = value; }
    public Vector3 SouthWest { get => southWest; set => southWest = value; }
    public Vector3 NorthWest { get => northWest; set => northWest = value; }
}

public class ImportantPosition
{
   private  UnitPosition start;
   private   UnitPosition destination;

    public UnitPosition Start { get => start; set => start = value; }
    public UnitPosition Destination { get => destination; set => destination = value; }
}



[Serializable]
public  class EndDrogMess
{
    private int dirt;//方向
    private UnitPosition position;

    public EndDrogMess()
    {
    }

    public EndDrogMess(int dirt, UnitPosition position)
    {
        this.Dirt = dirt;
        this.Position = position;
    }

    public int Dirt { get => dirt; set => dirt = value; }
    public UnitPosition Position { get => position; set => position = value; }

   
}

[Serializable]
public class DirtionMess
{
    private string objName;//物体的名字
    private int  leftImgUrl;
    private int rightImgUrl;

    public DirtionMess(string objName, int leftImgUrl, int rightImgUrl)
    {
        this.ObjName = objName;
        this.LeftImgUrl = leftImgUrl;
        this.RightImgUrl = rightImgUrl;
    }

    public string ObjName { get => objName; set => objName = value; }
    public int LeftImgUrl { get => leftImgUrl; set => leftImgUrl = value; }
    public int RightImgUrl { get => rightImgUrl; set => rightImgUrl = value; }
}
public class GameData 
{
    
}
