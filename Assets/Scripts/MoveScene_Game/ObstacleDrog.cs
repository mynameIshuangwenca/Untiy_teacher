using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using Move;
using DG.Tweening;
namespace QmDreamer.UI
{
    public class ObstacleDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象  
        private Transform topOfUiT;

        private Transform goParent;
        private UnitPosition unitPosition;
        private int type = 0;
        private Transform originParet;
        private Obstacles obstacles;
        private Transform onBeginParent;
        private Vector3 originScale;
        protected override void Awake()
        {
            onBeginParent = transform.Find("/Canvas/P_ArrowParent");
            originScale = transform.localScale;
        }
        protected override void Start()
        {
            obstacles = new Obstacles();
            base.Start();
            goParent = transform.parent;
           
            originParet = transform.parent;
            if(Util.obstacleType.ContainsKey(transform.name))
            {
                type = Util.obstacleType[transform.name];
            }
           
         


        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
          
            MoveController.Instance.FlagOnter(Input.mousePosition, Util.obstacleTip[type]);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {


            MoveController.Instance.FlagExit();
        }

        public void OnBeginDrag(PointerEventData _)
        {

            if (transform.parent == goParent)
            {
                GameObject newGo = ObjectPool.Instance.CreateObject(transform.name, gameObject, transform.position, transform.parent);
                // 使生成的名字一致 对象池
                newGo.name = transform.name;
                transform.parent = onBeginParent;
                newGo.GetComponent<Image>().raycastTarget = true;
                newGo.GetComponent<ObstacleDrog>().Scale(0,0.8f);
            }

            beginParentTransform = transform.parent;
            if (MoveController.Instance.gameState == GameState.Walking)
            {
                DestroyArrow();
            }


            Debug.Log("flagDrag" + transform.localScale);
            //物体增长1倍
            Scale(1);

        }


        public void OnDrag(PointerEventData _)
        {



            transform.position = Input.mousePosition;
            if (transform.GetComponent<Image>().raycastTarget) transform.GetComponent<Image>().raycastTarget = false;
        }


        public void OnEndDrag(PointerEventData _)
        {
            GameObject go = _.pointerCurrentRaycast.gameObject;
            Debug.Log(go.name);
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
            // 这个是回收  回收后 false   重新生成的也会是这样的
            transform.GetComponent<Image>().raycastTarget = true;
            //初始父类   回收
            if (transform.parent == onBeginParent)
            {
                ObjectPool.Instance.CollectObject(gameObject);
            }
            SetPosAndParent(transform, beginParentTransform);
           
        }

        /// <summary>
        /// 成功时 设置障碍物的位置
        /// </summary>
        /// <param name="tran">Tag 对应的mopPos </param>
        /// <param name="mousePosition">鼠标的位置 --停止的位置</param>
        private void  SetObstacle(Transform tran, Vector3 mousePosition)
        {
            //播放音乐
            AudioManager.Instance.PlaySound(14);

            int index = 0;
            UnitPosition ObstaclePosition = MoveModel.Instance.Fingbyname(tran.name);
            // 设置位置
            index= SetObstaclePostion(tran.name, type, mousePosition, tran);
            // 存储数据
            StoreObstacle(index, ObstaclePosition);
            // 障碍物 的物体的存储
            MoveModel.Instance.moveCache.Obstacle.Add(gameObject);
            // 存储指令类型 为了撤退
            MoveModel.Instance.moveCache.Drog.Add(2);

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

        
        /// <summary>
        /// 对障碍物数据进行存储
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ObstaclePosition"></param>
        public void  StoreObstacle( int index, UnitPosition ObstaclePosition)
        {
            //此名字的位置加上障碍物
            MoveModel.Instance.obstacle.OpObstacle(ObstaclePosition.Go.name, index, 1);
            thisObstacle(ObstaclePosition.Go.name, index, 0);
            // 得到index对应的name
            string posName=Tool.Instance.AroundName(ObstaclePosition.Go.name, index);
            if (!MoveModel.mapData.ContainsKey(posName))
            {
                //对应的index操作得到的Pos 越界了   不存在
                return;
            }
            MoveModel.Instance.obstacle.OpObstacle(posName, Util.obstacleDirt[index], 1);
            thisObstacle(posName, Util.obstacleDirt[index], 1);
        }


        /// <summary>
        /// 存储此障碍物的在数据存储中的位置
        /// </summary>
        /// <param name="name">数据存储中名字</param>
        /// <param name="index">数据存储中下标</param>
        /// <param name="index1">对应的 0-第一个 1-第二个</param>
        public void thisObstacle(string name,int index,int index1)
        {
            ObstacleStore obstacleStore = new ObstacleStore(name, index);
            obstacles.Obstacle[index1]= obstacleStore;

        }
        /// <summary>
        ///  销毁相应的障碍物的数据
        /// </summary>
        public  void DestroyData()
        {
            // 让对应的障碍物的数据归0
            foreach(ObstacleStore item in obstacles.Obstacle)
            {
                if (item == null) return;
                MoveModel.Instance.obstacle.OpObstacle(item.Name, item.Index, 0);
            }
        }
        /// <summary>
        /// 销毁这个障碍物
        /// </summary>
        public void GoDestroy()
        {
            // 销毁数据
            DestroyData();
            //恢复到原来的样子
            Scale(0, 1f);
            // 回收此物体
            ObjectPool.Instance.CollectObject(gameObject);
        }
        /// <summary>
        ///   缩放障碍物
        /// </summary>
        /// <param name="index"> 0：缩 1：放</param>
        public void Scale( int index,float duration=1f)
        {
            if (index == 0)
            {
                transform.DOScale(originScale, duration);
            }
            else if(type==0 )
            {
                transform.DOScale(originScale + new Vector3(originScale.x, originScale.y+0.08f, 0), duration);
            }
            else 
            {
                transform.DOScale(originScale + new Vector3(originScale.x+0.08f, originScale.y , 0), duration);
            }


        }

       

    }





}
