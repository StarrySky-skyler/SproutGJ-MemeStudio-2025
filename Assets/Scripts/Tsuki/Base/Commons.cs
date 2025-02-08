// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/29 22:01
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.MVC.Models.Game;
using UnityEngine;

namespace Tsuki.Base
{
    public static class Commons
    {
        /// <summary>
        ///     获取newpos位置是否在地图内
        /// </summary>
        /// <param name="gameModel"></param>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public static bool IsOnMap(GameModel gameModel, Vector3 newPos)
        {
            Collider2D hit =
                Physics2D.OverlapPoint(newPos, gameModel.groundLayer);
            return hit;
        }

        /// <summary>
        ///     获取相机修正后的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector3 GetModifiedPos(Vector3 pos)
        {
            Vector3 modifiedPos = pos;
            modifiedPos.z = -10;
            return modifiedPos;
        }

        /// <summary>
        ///     获取相机修正后的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector3 GetModifiedPos(Vector2 pos)
        {
            return new Vector3(pos.x, pos.y, -10);
        }
    }
}
