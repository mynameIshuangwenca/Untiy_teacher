using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Move {
    public class MoveData
    {

    }


    public class MoveCache
    {
        private GameObject player;
        private GameObject virutalPlayer;
        private List<GameObject> arrowMap;
        private List<GameObject> arrowRoute;

        public MoveCache()
        {
            ArrowMap = new List<GameObject>();
            ArrowRoute = new List<GameObject>();
        }

        public GameObject Player { get => player; set => player = value; }
        public GameObject VirutalPlayer { get => virutalPlayer; set => virutalPlayer = value; }
        public List<GameObject> ArrowMap { get => arrowMap; set => arrowMap = value; }
        public List<GameObject> ArrowRoute { get => arrowRoute; set => arrowRoute = value; }

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



        public void BackWalk()
        {
            ObjectPool.Instance.CollectObject(arrowMap[arrowMap.Count - 1]);
            ObjectPool.Instance.CollectObject(arrowRoute[arrowRoute.Count - 1]);
            arrowMap.RemoveAt(arrowMap.Count - 1);
            arrowRoute.RemoveAt(arrowRoute.Count - 1);

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

}

