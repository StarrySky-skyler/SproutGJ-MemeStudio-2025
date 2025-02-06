// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 22:01
// @version: 1.0
// @description:
// *****************************************************************************


using UnityEngine;

namespace Tsuki.Interface
{
    public interface IPushable
    {
        public bool TryPushBox(Vector2Int pushDirection);
    }
}
