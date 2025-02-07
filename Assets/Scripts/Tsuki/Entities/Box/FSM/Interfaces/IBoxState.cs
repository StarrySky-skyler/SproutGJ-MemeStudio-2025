// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 13:02
// @version: 1.0
// @description:
// *****************************************************************************

namespace Tsuki.Entities.Box.FSM.Interface
{
    public interface IBoxState
    {
        /// <summary>
        /// 进入状态调用
        /// </summary>
        public void OnEnter(Context context = null);

        /// <summary>
        /// 更新状态每帧调用
        /// </summary>
        public void OnUpdate(Context context = null);
        // public void OnLateUpdate();

        /// <summary>
        /// 退出状态调用
        /// </summary>
        public void OnExit(Context context = null);
        // public void OnFixedUpdate();
        
        public bool OnCheck(Context context = null);
    }
}
