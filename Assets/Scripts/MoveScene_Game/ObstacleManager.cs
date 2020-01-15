using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Move
{
    public class ObstacleManager
    {


    }
    public enum ObstacleState
    {
        Nothing,//什么都没有
        Obstacle,//有障碍物
        Wall,// 有墙
        NoName,//没有此名字的cell
        Cell//障碍物时在cell位置上

    }
    public class Obstacle
    {
        private Dictionary<string, List<int>> obstacle;
        private int DirtNum=4;
        private int CellObstacle = 4;// 块状障碍物
        public Obstacle()
        {
            obstacle = new Dictionary<string, List<int>>
            {
              {  "Img_Map_0_0",new List<int>{0,0,0,0,0}}, {  "Img_Map_0_1",new List<int>{0,0,0,0,0}},
              {  "Img_Map_0_2",new List<int>{0,0,0,0,0}}, {  "Img_Map_0_3",new List<int>{0,0,0,0,0}},
              {  "Img_Map_1_0",new List<int>{0,0,0,0,0}}, {  "Img_Map_1_1",new List<int>{0,0,0,0,0}},
              {  "Img_Map_1_2",new List<int>{0,0,0,0,0}}, {  "Img_Map_1_3",new List<int>{0,0,0,0,0}},
              {  "Img_Map_2_0",new List<int>{0,0,0,0,0}}, {  "Img_Map_2_1",new List<int>{0,0,0,0,0}},
              {  "Img_Map_2_2",new List<int>{0,0,0,0,0}}, {  "Img_Map_2_3",new List<int>{0,0,0,0,0}},
              {  "Img_Map_3_0",new List<int>{0,0,0,0,0}}, {  "Img_Map_3_1",new List<int>{0,0,0,0,0}},
              {  "Img_Map_3_2",new List<int>{0,0,0,0,0}}, {  "Img_Map_3_3",new List<int>{0,0,0,0,0}},

            } ;

        }
        /// <summary>
        /// 查询障碍物
        /// </summary>
        /// <param name="name"> mapPos的名字</param>
        /// <param name="dirt">位置  0-3 代表 上下左右</param>
        /// <returns> 0:没有障碍物 1：有障碍物 2 ： 墙  4：没有此name 3：下一步会有cell的障碍物</returns>
        public int  QuaryObstacle(string name ,int dirt )
        {
            // 如果没有存在这个name 
            int value = 4;
            ObstacleState obstacleState = IsObstacleofNext(name, dirt);
            if (obstacleState!= ObstacleState.Nothing)
            {
                return 1; // 有障碍
            }
           if (obstacle.ContainsKey(name) && dirt < DirtNum)// 防止溢出
            {
                value = obstacle[name][dirt];
            }

           
            return value;

        }
        /// <summary>
        /// 下一步是否为cell 障碍
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dirt"></param>
        /// <returns> ObstacleState.Nothing 则表示没有</returns>
        public ObstacleState IsObstacleofNext(string name, int dirt)
        {
            // 得到index对应的name
            string posName = Tool.Instance.AroundName(name, dirt);
            if (!MoveModel.mapData.ContainsKey(posName))
            {
                //对应的index操作得到的Pos 越界了   不存在
                return ObstacleState.Wall;
            }
             if (obstacle[posName][CellObstacle]==0)
            {
                return ObstacleState.Nothing;
            }
            else
            {
                return ObstacleState.Cell;
            }
          
        }

        /// <summary>
        /// 增加/删除 障碍物
        /// </summary>
        /// <param name="name"> mapPos的名字</param>
        /// <param name="dirt">位置  0-3 代表 上下左右</param>
        /// <param name="value"> 0:没有障碍物 1：有障碍物 2 ： 墙</param>
        /// <returns>true  修改成功</returns>
        public bool  OpObstacle(string name, int dirt, int value)// 防止溢出
        {
            if (obstacle.ContainsKey(name)&& dirt< DirtNum)
            {
                obstacle[name][dirt] = value;
                return true;
            }
            else
            {
                return false;
            }

        }

/// <summary>
/// 增加/删除 cell障碍物
/// </summary>
/// <param name="name"></param>
/// <param name="value"></param>
/// <returns></returns>
        public bool   OpCellObstacle(string name,int value)
        {
            if (obstacle.ContainsKey(name))
            {
                obstacle[name][CellObstacle] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        //清除数据重新为0
        public void Clean()
        {
            foreach (var item in obstacle)
            {
                for(int i=0;i<item.Value.Count;i++)
                {
                    item.Value[i] = 0;
                }
            }
        }


      

        ///// <summary>
        /////  // 增加/删除 障碍
        ///// </summary>
        ///// <param name="name">mappos的名字</param>
        ///// <param name="dirt">位置  0-3 代表 上下左右</param>
        ///// <param name="value">值 0 :没有 1：有  </param>
        /////    return  :  false: 已经存在了 true ; 添加成功
        //public bool   AddObstacle(string name ,int dirt,int value)
        //{
        //    bool flag = false;

        //    // 如果已经存在则直接加1
        //    if(obstacle.ContainsKey(name))
        //    {
        //        if(obstacle[name][dirt]==value)
        //        {
        //            flag = false;
        //        }
        //        else
        //        {
        //            obstacle[name][dirt] = value;
        //            flag = true;
        //        }
        //    }

        //    else
        //    {
        //        // 没有的话要增加
        //        List<int> cache = new List<int> { 0, 0, 0, 0 };
        //        cache[dirt] = value;
        //        obstacle.Add(name, cache);
        //        flag = true;
        //    }
        //    return flag;

        //}

        ////

        //public int QueryObstacle(string name ,int dirt)
        //{
        //     if(obstacle.ContainsKey(name))
        //    {
        //        return obstacle[name][dirt];
        //    }
        //     else
        //    {
        //        // 如果没有的话就是没有障碍
        //        return 0;
        //    }

        //}

    }
}