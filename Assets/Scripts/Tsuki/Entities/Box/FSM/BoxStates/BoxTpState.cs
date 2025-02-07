// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Box.FSM.Interface;
using Tsuki.Entities.TPPoint;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxTpState : BoxState, IBoxState
    {
        private TpPoint _tpPoint;
        
        public BoxTpState(BoxEntity boxEntity) : base(boxEntity)
        {
        }
        
        public void OnEnter(Context context)
        {
            _tpPoint.Tp(BoxEntity.transform, BoxEntity.lastPushDirection);
            BoxEntity.StateMachine.SwitchState(BoxStateType.Idle);
        }
        
        public void OnUpdate(Context context)
        {
            
        }
        
        public void OnExit(Context context)
        {
            
        }
        
        public bool OnCheck(Context context)
        {
            Collider2D hit1 =
                Physics2D.OverlapPoint(BoxEntity.NewPos, BoxEntity.tpLayer);
            if (!hit1) return false;
            _tpPoint = hit1.GetComponent<TpPoint>();
            return true;
        }
    }
}
