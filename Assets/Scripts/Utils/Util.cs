using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoSingleton<Util>
{
    private const string couseListScene = "CouseListSene";
    private const string gameScene = "GameScene";
    private const string moveScene = "MoveScene";
    public  const  string showCourseScene = "ShowCourseSence";
    private const string loginScene = "LoginScene";
    private const string registerScene = "RegisterScene";
    private const string classList = "ClassListSence";

    private const string levelData = "LevelData.json";

    public static string CouseListScene => couseListScene;

    public static string GameScene => gameScene;

    public static string LevelData => levelData;

    public static string MoveScene => moveScene;

    public static string ShowCourseScene => showCourseScene;

    public static string LoginScene => loginScene;

    public static string RegisterScene => registerScene;

    public static string ClassList => classList;

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

    public static Dictionary<string, int> obstacleType = new Dictionary<string, int>
    {
        {"ObstacleUp",0 }, {"ObstacleLeft",1 }
    };

    public static Dictionary<string, int> flagType = new Dictionary<string, int> {
     {"Start",0 }, {"Destination",1 }, {"Middle",2 }
    };

    public static int[] obstacleDirt = { 1, 0, 3, 2 };


    public static int[] turnDirt = { 0, 2, 1, 3 };


    public static int[] virtTD = { 0, 180, 90, 270 };

    public static List<string> flagTip = new List<string> { " 起点 "," 终点 "," 中继点 "};
    public static List<string> obstacleTip = new List<string> {" 竖直障碍物 "," 水平障碍物 " };
    public static List<string> arrowTip = new List<string> {"前进","后退","左转","右转" };

    //public static Dictionary<int, DirtionMess> dirtion = new Dictionary<int, DirtionMess> { 
    //    { 0, new DirtionMess("Up", 2, 5) }, { 1, new DirtionMess("Down", 4, 7) },

    //     { 2, new DirtionMess("Left", 3, 6) }, { 3, new DirtionMess("Right", 1, 8) },
    //};




}
