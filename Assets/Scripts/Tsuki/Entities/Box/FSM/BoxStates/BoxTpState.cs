// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
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
            BoxEntity.MoveTween.OnKill(() =>
            {
                context.Tp(BoxEntity.transform);
                BoxEntity.StateMachine.SwitchState(BoxStateType.Idle);
            });
            BoxEntity.MoveTween.Kill();
            //_tpPoint.Tp(BoxEntity.transform, BoxEntity.lastPushDirection);
            
        }
        
        public void OnUpdate(Context context)
        {
            
        }
        
        public void OnExit(Context context)
        {
            
        }
        
        public bool OnCheck(Context context)
        {
            return context.CheckTp(BoxEntity.lastPushDirection);
        }
    }
}
