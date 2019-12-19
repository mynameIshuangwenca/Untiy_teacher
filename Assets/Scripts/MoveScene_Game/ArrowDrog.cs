using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QmDreamer.UI
{
    public class ArrowDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象  
        private Transform topOfUiT;

        private Transform goParent;
        private UnitPosition unitPosition;
        private int dirtion = 0;
        private Transform originParet;
        //声明委托
        public static Action<EndDrogMess> ArrowEndDrog;
        protected override void Start()
        {
            base.Start();
            goParent = transform.parent;
            dirtion = Util.arrowDirt[transform.name];
            originParet = transform.parent;
           

        }

        public void OnBeginDrag(PointerEventData _)
        {
            


            //  // 已经选择了player  除了player 其他的都不能拖动了
            //if(MoveController.Instance.havechosePlayer==1 && transform.parent==originParet)
            // {
            //     return;
            // }
            //  if (transform.parent == topOfUiT) return;

            if (transform.parent == goParent)
            {
               GameObject newGo= ObjectPool.Instance.CreateObject(transform.name, gameObject, transform.position, transform.parent);
                // 使生成的名字一致 对象池
                newGo.name = transform.name;
            }
           
            beginParentTransform = transform.parent;
            if (MoveController.Instance.gameState == GameState.Walking)
            {
                DestroyArrow();
            }

            // 移动就可以复制一个


            //  transform.SetParent(topOfUiT);
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
                if(MoveController.Instance.gameState<GameState.start || go.name != MoveController.Instance.VirtualMess.Position.Go.name)//
                {
                    // 增加提示
                    Debug.Log("箭头没有放对位置");
                    DestroyArrow();
                    return;

                }

                ArrowWalk(go.transform);
            }

            else if (go.tag == "Player")
            {
                if(go.transform.parent.tag== "Position")
                {
                    ArrowWalk(go.transform.parent);
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
            if (transform.parent == originParet)
            {
                ObjectPool.Instance.CollectObject(gameObject);
            }
            SetPosAndParent(transform, beginParentTransform);
            transform.GetComponent<Image>().raycastTarget = true;
        }


        private void ArrowWalk(Transform tran)
        {
            SetPosAndParent(transform, tran);
            transform.GetComponent<Image>().raycastTarget = true;


            UnitPosition StartPosition = MoveModel.Instance.Fingbyname(tran.name);
            EndDrogMess endDrogMess = new EndDrogMess(dirtion, StartPosition);
           
            //触发委托 
            //ArrowEndDrog(endDrogMess);
            EndDrog( endDrogMess);
            ObjectPool.Instance.CollectObject(gameObject);

        }

        // 代替委托
        private void EndDrog(EndDrogMess endDrogMess)
        {
            if (MoveController.Instance.ArrowEndDrog(endDrogMess))
            {
                MoveView.Instance.ArrowEndDrog(endDrogMess);
            }
            
            

        }
    }

    
}

