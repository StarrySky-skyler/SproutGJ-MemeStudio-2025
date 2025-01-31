// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 22:01
// @version: 1.0
// @description:
// *****************************************************************************

namespace Tsuki.Interface
{
    public interface IUndoable
    {
        /// <summary>
        /// 撤销操作
        /// </summary>
        public void Undo();
    }
}
