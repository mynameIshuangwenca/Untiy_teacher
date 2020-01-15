using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QmDreamer.UI
{
    public class RouteArrowDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Transform onBeginParent;
        



        private int dirtion = 0;

       protected override void OnEnable()
        {
            onBeginParent = transform.Find("/Canvas/P_ArrowParent");
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
           
            MoveController.Instance.FlagOnter(Input.mousePosition, Util.arrowTip[dirtion]);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            MoveController.Instance.FlagExit();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            //if (transform.GetComponent<Image>().raycastTarget) transform.GetComponent<Image>().raycastTarget = false;
        }

       

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go == null)// 出去canvas了
            {
                SetPosAndParent(transform, transform.parent);
                return;
            }
            if (go.tag == "Trash")
            {
                DestroyArrow();
            }

            else
            {
                SetPosAndParent(transform, transform.parent);
            }

        }


      private void   DestroyArrow()
        {
            //换一个父母
            SetPosAndParent(transform, transform.parent);
            SetPosAndParent(transform.parent, onBeginParent);

            // 物体销毁
            ObjectPool.Instance.CollectObject(transform.parent.gameObject);
           
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
    }
}