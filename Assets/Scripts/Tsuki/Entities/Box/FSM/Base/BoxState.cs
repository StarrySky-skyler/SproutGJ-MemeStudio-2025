// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 13:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM.Interface;

namespace Tsuki.Entities.Box.FSM
{
    public abstract class BoxState
    {
        protected BaseObj BaseObj;

        protected BoxState(BaseObj baseObj)
        {
            BaseObj = baseObj;
        }
    }
}
