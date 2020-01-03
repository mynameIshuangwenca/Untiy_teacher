using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Move {
    public class MoveData
    {

    }

    /// <summary>
    /// 缓存的数据如在Map上的player virtualPlayer 箭头 障碍物 和route上的箭头
    /// </summary>
    public class MoveCache
    {
        private GameObject player;
        private GameObject virutalPlayer;
        private List<GameObject> arrowMap;
        private List<GameObject> arrowRoute;
        private List<GameObject> obstacle;
        private List<GameObject> flag;
        //拖动物体的指令  为了后撤指令
        private List<int> drog;

        public MoveCache()
        {
            ArrowMap = new List<GameObject>();
            ArrowRoute = new List<GameObject>();
            Obstacle = new List<GameObject>();
            Flag = new List<GameObject>();
            Drog = new List<int>();
        }

        public GameObject Player { get => player; set => player = value; }
        public GameObject VirutalPlayer { get => virutalPlayer; set => virutalPlayer = value; }
        public List<GameObject> ArrowMap { get => arrowMap; set => arrowMap = value; }
        public List<GameObject> ArrowRoute { get => arrowRoute; set => arrowRoute = value; }
        public List<GameObject> Obstacle { get => obstacle; set => obstacle = value; }
        public List<GameObject> Flag { get => flag; set => flag = value; }
        
        public List<int> Drog { get => drog; set => drog = value; }


        private void CleanDrog()
        {
            Drog.Clear();
        }


        /// <summary>
        ///  清除player
        /// </summary>
        private void CleanPlayer()
        {
            ObjectPool.Instance.CollectObject(player);
            player = null;

        }
        /// <summary>
        /// 清除 virutalPlayer
        /// </summary>
        private void CleanVirutalPlayer()
        {
            ObjectPool.Instance.CollectObject(virutalPlayer);
            virutalPlayer = null;

        }

        /// <summary>
        /// 清除ArrowMap 的内存
        /// </summary>
        private void CleanArrowMap()
        {
            foreach (GameObject obj in ArrowMap)
            {
                ObjectPool.Instance.CollectObject(obj);
            }
            ArrowMap.Clear();
        }
        /// <summary>
        ///清除 ArrowRoute 的内存
        /// </summary>
        private void CleanArrowRoute()
        {
            foreach (GameObject obj in ArrowRoute)
            {
                ObjectPool.Instance.CollectObject(obj);
            }
            ArrowRoute.Clear();
        }
        //清理障碍物
        private void Cleanobstacle()
        {
            foreach (GameObject obj in obstacle)
            {
                ObjectPool.Instance.CollectObject(obj);
            }
            obstacle.Clear();
        }


        private void CleanFlag()
        {
            foreach (GameObject obj in flag)
            {
                ObjectPool.Instance.CollectObject(obj);
            }
            flag.Clear();
        }
        /// <summary>
        ///  对缓存进行清除
        /// </summary>
        public void Clean()
        {
            CleanPlayer();
            CleanVirutalPlayer();

            //
            CleanArrowMap();
            CleanArrowRoute();
            Cleanobstacle();

            CleanFlag();
            CleanDrog();

        }
       /// <summary>
       /// route上的箭头变淡和恢复
       /// </summary>
       /// <param name="flag"> false : 变淡   true: 恢复</param>
        public void FadeArrowInRoute(bool flag)
        {
            for (int i = 0; i < ArrowRoute.Count; i++)
            {
                if (ArrowRoute[i].activeSelf)
                {
                    Tool.Instance.Fade(ArrowRoute[i].GetComponent<Image>(), flag);

                }

            }
        }
       /// <summary>
       ///  map 的 箭头变淡
       /// </summary>
       /// <param name="flag"></param>
        public void FadeArrowInMap(bool flag)
        {
            for (int i = 0; i < arrowMap.Count; i++)
            {

                if (arrowMap[i].activeSelf)
                {
                    Tool.Instance.Fade(arrowMap[i].GetComponent<Image>(), flag);

                }


            }
        }


        // 后退一步的工作
        public void BackWalk()
        {
            ObjectPool.Instance.CollectObject(arrowMap[arrowMap.Count - 1]);
            ObjectPool.Instance.CollectObject(arrowRoute[arrowRoute.Count - 1]);
            arrowMap.RemoveAt(arrowMap.Count - 1);
            arrowRoute.RemoveAt(arrowRoute.Count - 1);

        }


        /// <summary>
        /// 从拖动的的指令中 从后面减少箭头指令的
        /// </summary>
        /// <returns> 是否成功</returns>
        public bool RedureLastOrderInFlag()
        {
            bool flag = false;
            int index = drog.Count;
             for(int i =index-1;i>=0;i--)
            {
                //是指令
                if(drog[i]==0)
                {
                    drog.RemoveAt(i);
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }



    public class OrderData
    {

        private List<int> order;
        private List<UnitPosition> route;

        public List<int> Order { get => order; set => order = value; }
        public List<UnitPosition> Route { get => route; set => route = value; }

        public OrderData()
        {
            order = new List<int>();
            route = new List<UnitPosition>();
        }


        public void CleanOrder()
        {
            order.Clear();
        }

        public void CleanRoute()
        {
            route.Clear();

        }

        public void Clean()
        {
            CleanOrder();
            CleanRoute();
        }

        // 后退一步
        public void BackWalk()
        {
            route.RemoveAt(route.Count - 1);
            order.RemoveAt(order.Count - 1);
        }

    }


    
    public class ObstacleStore
    {
        private string name;
        private int index;

        public ObstacleStore()
        {
        }

        public ObstacleStore(string name, int index)
        {
            this.name = name;
            this.index = index;
        }

        public string Name { get => name; set => name = value; }
        public int Index { get => index; set => index = value; }
    }

    /// <summary>
    ///  存储相应的障碍物的位置
    /// </summary>
    public class Obstacles
    {
        private ObstacleStore[] obstacle;

        public Obstacles()
        {
            // 只用两个就好了
            obstacle = new ObstacleStore[2];
        }

        public ObstacleStore[] Obstacle { get => obstacle; set => obstacle = value; }
    }

}

