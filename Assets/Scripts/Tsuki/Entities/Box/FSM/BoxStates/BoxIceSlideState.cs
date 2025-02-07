// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using DG.Tweening;
using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM.Interface;
using Tsuki.Entities.IceLine;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxIceSlideState : BoxState, IBoxState
    {
        public BoxIceSlideState(BaseObj baseObj) : base(baseObj)
        {
        }
        
        public void OnEnter(Context context)
        {
            switch (context.IceType)
            {
                case IceType.Line:
                    // 冰线移动
                    BaseObj.MoveTween.onComplete += () =>
                    {
                        BaseObj.StateMachine.SwitchState(BoxStateType.PushMoving,
                            new Context { PushDirection = BaseObj.lastPushDirection });
                    };
                    break;
                case IceType.Grid:
                    BaseObj.MoveTween.onComplete += () =>
                    {
                        BaseObj.StateMachine.SwitchState(BoxStateType.PushMoving,
                            new Context
                                { PushDirection = BaseObj.lastPushDirection });
                    };
                    break;
                default:
                    Debug.LogError("OnEnter时冰类型grid/line错误");
                    break;
            }
        }

        public void OnUpdate(Context context)
        {
            
        }

        public void OnExit(Context context)
        {
            
        }

        public bool OnCheck(Context context)
        {
            switch (context.IceType)
            {
                case IceType.Line:
                    return CheckIceLineSlide();
                case IceType.Grid:
                    return CheckIceGridSlide();
                default:
                    Debug.LogError("冰类型grid/line错误");
                    return false;
            }
        }
        
        /// <summary>
        /// 处理单格冰滑动
        /// </summary>
        private bool CheckIceGridSlide()
        {
            // 冰层移动
            Collider2D hit =
                Physics2D.OverlapPoint(BaseObj.NewPos,
                    ModelsManager.Instance.GameMod.groundIceLayer);
            return hit;
        }

        /// <summary>
        /// 处理冰线滑动
        /// </summary>
        private bool CheckIceLineSlide()
        {
            Debug.Log("开始检测冰线滑动");
            Collider2D hit =
                Physics2D.OverlapPoint(BaseObj.NewPos,
                    ModelsManager.Instance.GameMod.groundIceLineLayer);
            if (!hit) return false;
            IceSingleLine iceLine = hit.GetComponent<IceSingleLine>();
            Debug.Log("检测到冰线");
            return iceLine.AllowSlide(BaseObj.lastPushDirection);
        }
    }
}
