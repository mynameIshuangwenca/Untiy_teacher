using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QmDreamer.UI
{
    public class PlayerDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象  
       // private Transform topOfUiT;

        private Transform goParent;
        private UnitPosition unitPosition;
        private int dirtion=0;
        private Transform originParet;
        //声明委托
        public static Action<EndDrogMess> EndDrog ;
        protected override void Start()
        {
            base.Start();
            goParent = transform.Find("/Canvas/P_Backgound/P_Controller/P_Show/P_Player");
            dirtion =Tool.Instance. DirtFindByName(transform.name);
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
               GameObject newgo= ObjectPool.Instance.CreateObject(transform.name, gameObject, transform.position, transform.parent);
                newgo.name = transform.name;

            }

            beginParentTransform = transform.parent;
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

                // 此处应该是监听
                SetPosAndParent(transform, go.transform);
                transform.GetComponent<Image>().raycastTarget = true;

                MoveModel.Instance.ArrowAndPlayerObj.Add(gameObject);
                MoveController.Instance.player = gameObject;
                UnitPosition StartPosition = MoveModel .Instance. Fingbyname(go.name);
                EndDrogMess endDrogMess = new EndDrogMess(dirtion, StartPosition);
                //触发委托 
                //EndDrog(endDrogMess);

                PlayEndDrog(endDrogMess);


            }
            
            else //其他任何情况，物体回归原始位置
            {
                if(transform.parent==originParet)
                {
                    ObjectPool.Instance.CollectObject(gameObject);
                }
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
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

        private void PlayEndDrog(EndDrogMess endDrogMess)
        {
            MoveController.Instance.EndDrog(endDrogMess);
            MoveView.Instance.EndDrog(endDrogMess);

        }






    }
}