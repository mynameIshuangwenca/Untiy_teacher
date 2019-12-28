using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QmDreamer.UI
{
    public class ObstacleDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象  
        private Transform topOfUiT;

        private Transform goParent;
        private UnitPosition unitPosition;
        private int type = 0;
        private Transform originParet;
       
        protected override void Start()
        {
            base.Start();
            goParent = transform.parent;
           
            originParet = transform.parent;
            if(Util.obstacleType.ContainsKey(transform.name))
            {
                type = Util.obstacleType[transform.name];
            }
           
         


        }

        public void OnBeginDrag(PointerEventData _)
        {

            if (transform.parent == goParent)
            {
                GameObject newGo = ObjectPool.Instance.CreateObject(transform.name, gameObject, transform.position, transform.parent);
                // 使生成的名字一致 对象池
                newGo.name = transform.name;
            }

            beginParentTransform = transform.parent;
            if (MoveController.Instance.gameState == GameState.Walking)
            {
                DestroyArrow();
            }

         
        }


        public void OnDrag(PointerEventData _)
        {



            transform.position = Input.mousePosition;
            if (transform.GetComponent<Image>().raycastTarget) transform.GetComponent<Image>().raycastTarget = false;
        }


        public void OnEndDrag(PointerEventData _)
        {
            GameObject go = _.pointerCurrentRaycast.gameObject;
            if (go == null)// 出去canvas了
            {
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
                return;
            }
            if (go.tag == "Position") //如果当前拖动物体下是：格子 -（没有物品）时
            {
                Debug.Log("name :" + go.name + "postion" + Input.mousePosition);
                Debug.Log("Centerpostion" +  MoveModel.Instance.Fingbyname(go.name).Center);

                SetObstacle(go.transform, Input.mousePosition);
            }

            else if (go.tag == "Player")
            {
                if (go.transform.parent.tag == "Position")
                {



                    SetObstacle(go.transform.parent, Input.mousePosition);
                }
                else
                {
                    DestroyArrow();
                }


            }


            else //其他任何情况，物体回归原始位置
            {
                DestroyArrow();
            }
        }


        /// <summary>
        /// 设置父物体，UI位置归正
        /// </summary>
        /// <param name="t">对象Transform</param>
        /// <param name="parent">要设置到的父级</param>
        private void SetPosAndParent(Transform t, Transform parent)
        {
            t.SetParent(parent);
            t.position = parent.position;
        }


        private void SetPosAndParent(Transform t, Transform parent, Vector3 position)
        {
            t.SetParent(parent);
            t.position = position;

        }


        private void DestroyArrow()
        {
            //初始父类   回收
            if (transform.parent == originParet)
            {
                ObjectPool.Instance.CollectObject(gameObject);
            }
            SetPosAndParent(transform, beginParentTransform);
            transform.GetComponent<Image>().raycastTarget = true;
        }

        /// <summary>
        /// 成功时 设置障碍物的位置
        /// </summary>
        /// <param name="tran">Tag 对应的mopPos </param>
        /// <param name="mousePosition">鼠标的位置 --停止的位置</param>
        private void  SetObstacle(Transform tran, Vector3 mousePosition)
        {

            int index = 0;
            UnitPosition ObstaclePosition = MoveModel.Instance.Fingbyname(tran.name);
            // 设置位置
            index= SetObstaclePostion(tran.name, type, mousePosition, tran);
            // 存储数据
            StoreObstacle(index, ObstaclePosition);
            // 物体的存储
            MoveModel.Instance.moveCache.Obstacle.Add(gameObject);


            //触发委托 

            //EndDrog(endDrogMess);
            // ObjectPool.Instance.CollectObject(gameObject);

        }


        // 障碍物应该存放的位置
        private Vector3 GetPostion(int dirt ,Vector3 mousePosition, UnitPosition postion ,ref int index )
        {
            Vector3 retPostion=new Vector3(0,0,0);
            if(dirt==0)
            {
                if (mousePosition.x<=postion.Center.x)
                {
                    retPostion = postion.Left;
                    index = 2;
                }
                else
                {
                    retPostion = postion.Right;
                    index = 3;
                }
            }
            else if (dirt ==1)
            {
                if(mousePosition.y<=postion.Center.y)
                {
                    retPostion = postion.Bottom;
                    index = 1;
                }
                else
                {
                    retPostion = postion.Top;
                    index = 0;
                }
            }
            else
            {

            }

            return retPostion;
        }
        /// <summary>
        /// 设置障碍物的位置
        /// </summary>
        /// <param name="name"> mapPos 的名字</param>
        /// <param name="dirt">类型 0-竖 1-横</param>
        /// <param name="mousePosition">停止的位置</param>
        /// <param name="parent">设置障碍物的父类</param>
        public int  SetObstaclePostion(string name,int dirt, Vector3 mousePosition,Transform parent)
        {
            int index = 0;
            // 得到此tag 的位置
            UnitPosition ObstaclePosition = MoveModel.Instance.Fingbyname(name);
            // 得到障碍物的位置
            Vector3 position =  GetPostion(dirt, mousePosition, ObstaclePosition,ref index);
            // 放置障碍物的位置
            SetPosAndParent(transform, parent, position);
            //设置为不能拖动了
            transform.GetComponent<Image>().raycastTarget = false;
            Debug.Log("SetObstaclePostion" + index);
            return index;
        }



        public void  StoreObstacle( int index, UnitPosition ObstaclePosition)
        {
            //此名字的位置加上障碍物
            MoveModel.Instance.obstacle.OpObstacle(ObstaclePosition.Go.name, index, 1);
            // 得到index对应的name
            string posName=Tool.Instance.AroundName(ObstaclePosition.Go.name, index);
            if (!MoveModel.mapData.ContainsKey(posName))
            {
                //对应的index操作得到得到Pos 越界了 
                return;
            }
            MoveModel.Instance.obstacle.OpObstacle(posName, Util.obstacleDirt[index], 1);





        }


    }


}
