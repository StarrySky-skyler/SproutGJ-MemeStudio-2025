// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 13:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Box.FSM.Interface;

namespace Tsuki.Entities.Box.FSM
{
    public abstract class BoxState
    {
        protected BoxEntity BoxEntity;

        protected BoxState(BoxEntity boxEntity)
        {
            BoxEntity = boxEntity;
        }
    }
}
