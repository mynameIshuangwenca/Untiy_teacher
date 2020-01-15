using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Move;
using DG.Tweening;
namespace QmDreamer.UI
{
    public class CellObstacleDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象  
        private Transform goParent;
      
        private Transform onBeginParent;
        private Vector3 originScale;
        protected override void Awake()
        {
            onBeginParent = transform.Find("/Canvas/P_ArrowParent");
            originScale = transform.localScale;
        }
        protected override void Start()
        {
           
            base.Start();
            goParent = transform.parent;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {

            MoveController.Instance.FlagOnter(Input.mousePosition, "障碍物");
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
                transform.SetParent(onBeginParent);
                //transform.parent = onBeginParent;
                newGo.GetComponent<Image>().raycastTarget = true;
                //生成的回收 （在scroll 中）
               // newGo.transform.localScale = originScale;
                newGo.GetComponent<CellObstacleDrog>().Scale(0,0.8f);
            }

            beginParentTransform = transform.parent;
            if (MoveController.Instance.gameState == GameState.Walking)
            {
                DestroyCellObstacle();
            }


          
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
            if (go == null)// 出去canvas了
            {
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
                return;
            }
            if (go.tag == "Position") //如果当前拖动物体下是：格子 -（没有物品）时
            {
                Debug.Log("name :" + go.name + "postion" + Input.mousePosition);
                SetCellObstacle(go.transform);    
            }

            else if (go.tag == "Player")
            {
                if (go.transform.parent.tag == "Position")
                {
                    SetCellObstacle(go.transform.parent);    
                }
                else
                {
                    DestroyCellObstacle();
                }


            }


            else //其他任何情况，物体回归原始位置
            {
                DestroyCellObstacle();
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


        public void SetCellObstacle(Transform parent)
        {
            //播放音乐
            AudioManager.Instance.PlaySound(14);
            //设置位置
            SetPosAndParent(transform, parent);

            // 存储数据
            StoreCellObstacle(parent.name);
            // 障碍物 的物体的存储
            MoveModel.Instance.moveCache.Obstacle.Add(gameObject);
            // 存储指令类型 为了撤退
            MoveModel.Instance.moveCache.Drog.Add(3);

            //触发委托 
        }

        private void DestroyCellObstacle()
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
        /// 存储数据
        /// </summary>
        /// <param name="name"> cell 名字</param>
        public void StoreCellObstacle(string name)
        {
            //此名字的位置加上障碍物
            MoveModel.Instance.obstacle.OpCellObstacle(name, 1);
        }



        /// <summary>
        ///  销毁相应的障碍物的数据
        /// </summary>
        public void DestroyData()
        {
                
                MoveModel.Instance.obstacle.OpCellObstacle(transform.parent.name, 0);
            
        }
        /// <summary>
        /// 销毁这个障碍物
        /// </summary>
        public void GoDestroy()
        {
            // 销毁数据
            DestroyData();
            //恢复到原来的样子
          
            // 回收此物体
            ObjectPool.Instance.CollectObject(gameObject);
        }
        /// <summary>
        ///   缩放障碍物
        /// </summary>
        /// <param name="index"> 0：缩 1：放</param>
        public void Scale(int index,float duration=1.0f)
        {
            
                if (index == 0)
                {
                    transform.DOScale(originScale, duration);
                }
                else
                {
                    transform.DOScale(originScale + new Vector3(originScale.x+0.7f, originScale.y+0.7f, 0), duration);
                }
            

           

        }

       
    }





}
