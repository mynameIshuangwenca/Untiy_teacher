
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
namespace QmDreamer.UI
{

    public class FlagDrog : Button, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象                                             
        private UnitPosition unitPosition;
        private int type = 0;
        private Transform originParet;
        private Transform onBeginParent;
        
        protected override void Awake()
        {
            onBeginParent = transform.Find("/Canvas/P_ArrowParent");
        }

        protected override void Start()
        {
            base.Start(); 
            originParet = transform.parent;
            if (Util.flagType.ContainsKey(transform.name))
            {
                type = Util.flagType[transform.name];
            }
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter"+ Input.mousePosition);
            MoveController.Instance.FlagOnter(Input.mousePosition, Util.flagTip[type]);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {


            MoveController.Instance.FlagExit();
        }

        public void OnBeginDrag(PointerEventData _)
        {
            //第一次
            if (transform.parent == originParet)
            {
                GameObject  newgo = ObjectPool.Instance.CreateObject(transform.name, gameObject, transform.position, transform.parent);
                newgo.name = transform.name;
                // 激活状态的+ controller下的旗子的存储
                MoveModel.Instance.Flag[type] = newgo;
                
                transform.parent = onBeginParent;
            }
          
            beginParentTransform = transform.parent;

           
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
                ObjectPool.Instance.CollectObject(gameObject);
                return;
            }
            if (go.tag == "Position") //如果当前拖动物体下是：格子 -（没有物品）时
            {


                PutSuccess(go);

            }

            else //其他任何情况，物体回归原始位置
            {
                // 如果是还是原来的父类 则回收
                if (transform.parent == onBeginParent)
                {
                    ObjectPool.Instance.CollectObject(gameObject);
                }
                // 恢复到原来的父类
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


       

        private void PutSuccess(GameObject  go)
        {
            // 此处应该是监听
            SetPosAndParent(transform, go.transform);
            transform.GetComponent<Image>().raycastTarget = true;
            MoveModel.Instance.moveCache.Flag.Add(gameObject);
            //播放音乐
            AudioManager.Instance.PlaySound(14);

            // controller 的旗子变淡  中继站可以多个
            if(type!=2)
            {
                MoveView.Instance.FadeFlag(type);
            }
          
            // 存储指令类型 为了撤退
            MoveModel.Instance.moveCache.Drog.Add(1);
            // 存储重要位置的点 
            unitPosition = MoveModel.Instance.Fingbyname(go.name);
            StorePoistion();



        }


        /// <summary>
        /// // 存入重要的点
        /// </summary>
        /// <param name="type">旗子的类型</param>
        /// <param name="position">位置</param>
        public void   StorePoistion (  )
        {

            if (type ==0)
            {
                MoveModel.Instance.importantPosition.Start = unitPosition;
            }
            else if (type==1)
            {
                MoveModel.Instance.importantPosition.Destination = unitPosition;
            }
            else
            {
                MoveModel.Instance.importantPosition.Middle.Add(unitPosition);
            }
        }



    }
}