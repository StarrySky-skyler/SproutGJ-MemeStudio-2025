// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 18:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Box.FSM;
using Tsuki.Entities.Box.FSM.BoxStates;
using UnityEngine;

namespace Tsuki.Entities.Box.Base
{
    public class NormalObj : BaseObj
    {
        protected override void Awake()
        {
            base.Awake();
            StateMachine.AddState(BoxStateType.Tp, new BoxTpState(this));
            StateMachine.AddState(BoxStateType.IceSlide,
                new BoxIceSlideState(this));
        }
    }
}
