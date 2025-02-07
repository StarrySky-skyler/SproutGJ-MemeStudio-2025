// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Box.FSM.Interface;
using Tsuki.Entities.IceLine;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxIceSlideState : BoxState, IBoxState
    {
        public BoxIceSlideState(BoxEntity boxEntity) : base(boxEntity)
        {
        }
        
        public void OnEnter(Context context)
        {
        }

        public void OnUpdate(Context context)
        {
            
        }

        public void OnExit(Context context)
        {
            
        }

        public bool OnCheck(Context context)
        {
            return HandleIceLineSlide() || HandleIceGridSlide();
        }
        
        /// <summary>
        /// 处理单格冰滑动
        /// </summary>
        private bool HandleIceGridSlide()
        {
            // 冰层移动
            Collider2D hit =
                Physics2D.OverlapPoint(BoxEntity.NewPos,
                    BoxEntity.groundIceLayer);
            if (!hit) return false;
            BoxEntity.StateMachine.SwitchState(BoxStateType.PushMoving,
                new Context { PushDirection = BoxEntity.lastPushDirection });
            return true;
        }

        /// <summary>
        /// 处理冰线滑动
        /// </summary>
        private bool HandleIceLineSlide()
        {
            Debug.Log("开始检测冰线滑动");
            Collider2D hit =
                Physics2D.OverlapPoint(BoxEntity.NewPos,
                    BoxEntity.groundIceLineLayer);
            if (!hit) return false;
            IceSingleLine iceLine = hit.GetComponent<IceSingleLine>();
            Debug.Log("检测到冰线");
            if (!iceLine.AllowSlide(BoxEntity.lastPushDirection)) return false;
            // 冰线移动
            BoxEntity.StateMachine.SwitchState(BoxStateType.PushMoving,
                new Context { PushDirection = BoxEntity.lastPushDirection });
            return true;
        }
    }
}
