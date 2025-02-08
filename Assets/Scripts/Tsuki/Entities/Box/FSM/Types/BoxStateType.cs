// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 13:02
// @version: 1.0
// @description:
// *****************************************************************************

namespace Tsuki.Entities.Box.FSM.Types
{
    /// <summary>
    ///     箱子状态类型
    /// </summary>
    public enum BoxStateType
    {
        Idle = 1,
        PushMoving,
        IceSlide,
        Tp
    }
}
