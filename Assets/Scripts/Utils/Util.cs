using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoSingleton<Util>
{
    private const string couseListScene = "CouseListSene";
    private const string gameScene = "GameScene";
    private const string moveScene = "MoveScene";
    private const string showCourseScene = "ShowCourseSence";

    private const string levelData = "LevelData.json";

    public static string CouseListScene => couseListScene;

    public static string GameScene => gameScene;

    public static string LevelData => levelData;

    public static string MoveScene => moveScene;

    public static string ShowCourseScene => showCourseScene;

    public static List<DirtionMess> dirtion = new List<DirtionMess> { new DirtionMess("Up", 2, 5), new DirtionMess("Down", 4, 7), new DirtionMess("Left", 3, 6), new DirtionMess("Right", 1, 8) };
    public static List<List<int>>dirtToPos =new List<List<int>>{
        new List<int>{0,1,2,3}, new List<int>{1,0,3,2}, new List<int>{5,7,6,4}, new List<int>{8,10,11,9}

};

    public static List<List<int>> bDirtToPos = new List<List<int>>
    {
         new List<int>{0,1,2,3}, new List<int>{1,0,3,2}, new List<int>{10,8,9,11}, new List<int>{7,5,4,6}
    };

    public static Dictionary<string, int> arrowDirt = new Dictionary<string, int> {
        {"ArrowUp",0 }, {"ArrowDown",1 }, {"ArrowLeft",2}, {"ArrowRight",3 }
    };


    public static int[] turnDirt = { 0, 2, 1, 3 };


    public static int[] virtTD = { 0, 180, 90, 270 };

    //public static Dictionary<int, DirtionMess> dirtion = new Dictionary<int, DirtionMess> { 
    //    { 0, new DirtionMess("Up", 2, 5) }, { 1, new DirtionMess("Down", 4, 7) },

    //     { 2, new DirtionMess("Left", 3, 6) }, { 3, new DirtionMess("Right", 1, 8) },
    //};




}
