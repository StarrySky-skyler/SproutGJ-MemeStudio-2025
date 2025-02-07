// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 12:02
// @version: 1.0
// @description:
// *****************************************************************************

using UnityEngine;

namespace Tsuki.Entities.IceLine
{
    /// <summary>
    /// 冰弦方向类型
    /// </summary>
    public enum IceLineType
    {
        None,
        Horizontal,
        Vertical
    }

    public class IceSingleLine : MonoBehaviour
    {
        public IceLineType iceLineType;

        /// <summary>
        /// 获取是否允许滑动
        /// </summary>
        /// <param name="enterDirection"></param>
        /// <returns></returns>
        public bool AllowSlide(Vector2Int enterDirection)
        {
            Debug.Log("冰线类型为：" + iceLineType);
            switch (iceLineType)
            {
                case IceLineType.None:
                    Debug.LogError("冰线类型为空");
                    break;
                case IceLineType.Horizontal:
                    // 水平冰线
                    if (enterDirection.y != 0) return false;
                    break;
                case IceLineType.Vertical:
                    // 垂直冰线
                    if (enterDirection.x != 0) return false;

                    break;
                default:
                    Debug.LogError("冰线类型错误");
                    break;
            }

            return true;
        }
    }
}
