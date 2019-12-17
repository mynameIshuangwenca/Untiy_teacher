using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QmDreamer.UI
{
  public class DragDirt : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
{
        private Transform beginParentTransform; //记录开始拖动时的父级对象  
        private Transform topOfUiT;
       
        private Transform goParent;
        private UnitPosition unitPosition;

        protected override void Start()
        {
            base.Start();
            topOfUiT = GameObject.Find("Canvas").transform;
            goParent = transform.parent;

        }

        public void OnBeginDrag(PointerEventData _)
        {
           
            if (transform.parent == topOfUiT) return;

            if (transform.parent==goParent)
            {
                ObjectPool.Instance.CreateObject(transform.name, gameObject, transform.position, transform.parent);
            }
           
            beginParentTransform = transform.parent;
            // 移动就可以复制一个
            
          
            transform.SetParent(topOfUiT);
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






                
                SetPosAndParent(transform, go.transform, GetPosition(go.name));
                transform.GetComponent<Image>().raycastTarget = true;
            }
            else if (go.tag == "Map")
            {
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
            }
            else if (go.tag == "Player")
            {
                Transform nowParent = ChinarDragSwapImage.Instance.NowParent;
                SetPosAndParent(transform, nowParent, GetPosition(nowParent.name));
                transform.GetComponent<Image>().raycastTarget = true;

            }



            else //其他任何情况，物体回归原始位置
            {
              
                ObjectPool.Instance.CollectObject(gameObject);
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


        private void SetPosAndParent(Transform t, Transform parent,Vector3 position)
        {
            t.SetParent(parent);
            t.position = position;

        }

        


       /// <summary>
       /// 通过名字得到位置 （内含有对Tag 判断）
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
        private  Vector3 GetPosition(string name)
        {
            Vector3 position = new Vector3();
            unitPosition = MapMamager.Instance.Fingbyname(name);
            if(transform.tag=="Up")
            {
                position = unitPosition.Top;

            } else if (transform.tag == "Down")
            {
                position = unitPosition.Bottom;
            }
            else if (transform.tag == "Left")
            {
                position = unitPosition.Left;
            }
            else
            {
                position = unitPosition.Right;
            }
         return position;
        }
    }
}
