// ========================================================
// 描述：Demo 02 —— 通过继承 + 接口实现 图片拖动替换位置。
// 作者：Chinar 
// 创建时间：2019-04-29 16:39:11
// 版 本：1.0
// ========================================================

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace QmDreamer.UI
{
    /// <summary>
    /// 管理UI元素排序：使UI可通过拖动进行位置互换
    /// </summary>
    public class ChinarDragSwapImage : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private static ChinarDragSwapImage instance;

        private Transform beginParentTransform; //记录开始拖动时的父级对象        
        /// <summary>
        /// UI界面的顶层，这里我用的是 Canvas
        /// (这个变量在开发中设置到单例中较好，不然每一个物品都会初始化查找
        /// GameObject.Find("Canvas").transform;)
        /// </summary>
        /// 
        // 目前的父亲（为了他的位置）
        private Transform nowParent;
        private Transform topOfUiT;

        public Transform NowParent { get => nowParent; set => nowParent = value; }
        public static ChinarDragSwapImage Instance { get => instance; set => instance = value; }
        protected override void Awake()
        {
            instance = this;
        }
        protected override void Start()
        {
            base.Start();
            topOfUiT = GameObject.Find("Canvas").transform;
        }


        public void OnBeginDrag(PointerEventData _)
        {
            if (transform.parent == topOfUiT) return;
            beginParentTransform = transform.parent;
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
            if(go==null)// 出去canvas了
            {
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
                return;
            }
            if (go.tag == "Position") //如果当前拖动物体下是：格子 -（没有物品）时
            {
                SetPosAndParent(transform, go.transform);
                NowParent = go.transform;
                transform.GetComponent<Image>().raycastTarget = true;
                // 设置开始位置
                GameManager.Instance.NeedPosition.Start= MapMamager.Instance.Fingbyname(go.name);
                  

            }
           
            else //其他任何情况，物体回归原始位置
            {
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
    }
}