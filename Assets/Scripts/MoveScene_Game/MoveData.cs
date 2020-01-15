using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
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
        // 跑马灯功能中的跑的指令 （变淡的箭头）
        private int fadeIndex;
       
        public MoveCache()
        {
            ArrowMap = new List<GameObject>();
            ArrowRoute = new List<GameObject>();
            Obstacle = new List<GameObject>();
            Flag = new List<GameObject>();
            Drog = new List<int>();
            FadeIndex = 0;

        }

        public GameObject Player { get => player; set => player = value; }
        public GameObject VirutalPlayer { get => virutalPlayer; set => virutalPlayer = value; }
        public List<GameObject> ArrowMap { get => arrowMap; set => arrowMap = value; }
        public List<GameObject> ArrowRoute { get => arrowRoute; set => arrowRoute = value; }
        public List<GameObject> Obstacle { get => obstacle; set => obstacle = value; }
        public List<GameObject> Flag { get => flag; set => flag = value; }
        
        public List<int> Drog { get => drog; set => drog = value; }
        public int FadeIndex { get => fadeIndex; set => fadeIndex = value; }

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

            CleanFadeIndex();

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

        /// <summary>
        /// Map上 停止或激活旗子的拖动
        /// </summary>
        /// <param name="IsDrog">true ：激活拖动 false：停止拖动</param>
        public void IsDrogFlag(bool IsDrog)
        {
            for(int i=0;i<flag.Count;i++)
            {
                if (flag[i] == null) continue;
                flag[i].GetComponent<Image>().raycastTarget = IsDrog;
            }
        }

        public void IsDrogPlayer(bool IsDrog)
        {
            player.GetComponent<Image>().raycastTarget = IsDrog;
        }


        /// <summary>
        /// fadeIndex 在开始按钮时的值的改变
        /// </summary>
        public void  GetFadeIndex()
        {
            fadeIndex = arrowMap.Count;
        }
        /// <summary>
        /// fadeIndex 清零
        /// </summary>
        public void CleanFadeIndex()
        {
            fadeIndex = 0;
        }
        
    }

    



    public class OrderData
    {
        
        // 指令  一次跑完之后会清空  
        private List<int> order;
        private List<UnitPosition> route;

        //指令  区别是 这个会一直保持到存储   
        private List<int> routeOrder;
        // 存储的位置图
        private List<UnitPosition> storeRoute;
        // 
        private  string startPostion;

        private List<int> middleStep;




       
        public List<int> Order { get => order; set => order = value; }
        public List<UnitPosition> Route { get => route; set => route = value; }
        public List<int> RouteOrder { get => routeOrder; set => routeOrder = value; }
        public List<UnitPosition> StoreRoute { get => storeRoute; set => storeRoute = value; }
        public List<int> MiddleStep { get => middleStep; set => middleStep = value; }

        public OrderData()
        {
            order = new List<int>();
            route = new List<UnitPosition>();
            RouteOrder = new List<int>();
            storeRoute = new List<UnitPosition>();
            MiddleStep = new List<int>();
        }
        /// <summary>
        /// 点击开始按钮时增加
        /// </summary>
        public void  AddOrderRange()
        {
            RouteOrder.AddRange(order);
            storeRoute.AddRange(route);
            if (middleStep.Count==0)
            {
                middleStep.Insert(0,order.Count);
            }
            else
            {
                //加上最后一个
                middleStep.Add(order.Count + middleStep[middleStep.Count - 1]);
            }
           

        }

        /// <summary>
        /// list(int) 类型变成 2+4+5  
        /// </summary>
        /// <param name="listDoc"></param>
        /// <returns></returns>
        private string  ListToStr(List<int> listDoc)
        {
            StringBuilder retStr = new StringBuilder();
            if (listDoc.Count>0)
            {
                retStr.Insert(0, listDoc[0].ToString());
                for(int i=1;i<listDoc.Count;i++)
                {
                    retStr.Append(string.Format("+{0}", listDoc[i]));
                }
            }
            return retStr.ToString();

        }

        /// <summary>
        /// middlestep 变成2+3+4形式
        /// </summary>
        /// <returns></returns>
        public string GetMiddleStepStr()
        {
            return ListToStr(middleStep);
        }

   /// <summary>
   /// 指令变成字符串 返回
   /// </summary>
   /// <returns></returns>
        public string  OrderToStr()
        {
            string reStr = string.Empty;
            if (RouteOrder.Count>0)
            {
                reStr += string.Format("{0}", routeOrder[0]);
                for(int i=1;i<routeOrder.Count;i++)
                {
                    reStr += string.Format("+{0}", routeOrder[i]);
                }
            }
            return reStr;
        }

        public void CleanOrder()
        {
            order.Clear();
        }

        public void CleanRoute()
        {
            route.Clear();

        }
        public void CleanRouteOrder()
        {
            RouteOrder.Clear();
        }

        public void CleanStoreRoute()
        {
            storeRoute.Clear();
        }

        public void CleanMiddleStep()
        {
            middleStep.Clear();
        }

        /// <summary>
        /// 一次开始结束后的清空
        /// </summary>
        public void  CleanCacheOder()
        {
            CleanOrder();
            CleanRoute();
        }

        
        /// <summary>
        /// 在重置的时候清空
        /// </summary>
        public void Clean()
        {
            CleanMiddleStep();
            CleanOrder();
            CleanRoute();

            CleanRouteOrder();
            CleanStoreRoute();

        }



        // 后退一步
        public void BackWalk()
        {
            route.RemoveAt(route.Count - 1);
            order.RemoveAt(order.Count - 1);
        }


        public string RouteToStr(List<UnitPosition> myRoute)
        {
            string reRstr = string.Empty;
            if (myRoute.Count != 0 && myRoute[0] != null)
            {
                reRstr += getIJFormPosition(myRoute[0]);
            }
            else
            {
                return string.Empty;
            }

            for (int i = 1; i < myRoute.Count; i++)
            {
                if (myRoute[i] == null) continue;
                reRstr += "+" + getIJFormPosition(myRoute[i]);
            }

            return reRstr;
        }



        /// <summary>
        /// 得到路径
        /// </summary>
        /// <returns> 路径  0,0+ 0,1+1,1</returns>
        public string GetRoute()
        {
            return RouteToStr(route);
        }

        public string GetStoreRoute()
        {
            return RouteToStr(storeRoute);
        }
        /// <summary>
        /// 从位置中得到i j  然后组成i,j形式
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public  string getIJFormPosition(UnitPosition position)
        {
            string reRstr = string.Empty;
            reRstr += Tool.Instance.CoordinateToStr(position.I, position.J);
            return reRstr;

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

