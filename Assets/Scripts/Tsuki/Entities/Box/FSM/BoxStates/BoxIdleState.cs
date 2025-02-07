// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM.Interface;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxIdleState : BoxState, IBoxState
    {
        public BoxIdleState(BaseObj baseObj) : base(baseObj)
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
            return true;
        }
    }
}
