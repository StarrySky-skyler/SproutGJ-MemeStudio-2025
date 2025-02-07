// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM.Interface;
using Tsuki.Entities.TPPoint;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxTpState : BoxState, IBoxState
    {
        private TpPoint _tpPoint;
        
        public BoxTpState(BaseObj baseObj) : base(baseObj)
        {
        }
        
        public void OnEnter(Context context)
        {
            BaseObj.MoveTween.OnKill(() =>
            {
                context.Tp(BaseObj.transform);
                BaseObj.StateMachine.SwitchState(BoxStateType.Idle);
            });
            BaseObj.MoveTween.Kill();
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
            return context.CheckTp(BaseObj.lastPushDirection);
        }
    }
}
