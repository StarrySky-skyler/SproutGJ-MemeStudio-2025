// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// ********************************************************************************

using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Base
{
    public static class Commons
    {
        /// <summary>
        /// 获取是否在地图范围内可移动
        /// </summary>
        /// <param name="playerModel"></param>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public static bool GetMovable(PlayerModel playerModel, Vector3 newPos)
        {
            bool tag = newPos.x >= 0;
            tag = tag && newPos.x <= playerModel.moveRange.x;
            tag = tag && newPos.y >= 0;
            tag = tag && newPos.y <= playerModel.moveRange.y;
            return tag;
        }
    }
}
