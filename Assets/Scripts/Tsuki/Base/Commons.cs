// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/29 22:01
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Base
{
    public static class Commons
    {
        /// <summary>
        /// 获取newpos位置是否在地图内
        /// </summary>
        /// <param name="playerModel"></param>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public static bool IsOnMap(PlayerModel playerModel, Vector3 newPos)
        {
            Collider2D hit =
                Physics2D.OverlapPoint(newPos, playerModel.groundLayer);
            return hit;
        }
    }
}
