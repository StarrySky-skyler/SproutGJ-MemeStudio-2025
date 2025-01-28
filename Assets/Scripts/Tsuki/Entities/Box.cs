// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// ********************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.MVC.Models;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Entities
{
    public class Box : MonoBehaviour
    {
        private Vector3 _newPos;
        private RaycastHit2D[] _hitsBuffer = new RaycastHit2D[10];

        /// <summary>
        /// 获取箱子是否可推动
        /// </summary>
        /// <param name="playerModel">玩家模型</param>
        /// <param name="direction">推动方向</param>
        /// <returns></returns>
        public bool GetPushable(PlayerModel playerModel, Vector2Int direction)
        {
            _newPos = transform.position +
                      new Vector3(direction.x * playerModel.girdSize, direction.y * playerModel.girdSize, 0);
            Debug.DrawRay(transform.position, (Vector2)direction, Color.green, 3);
            // 射线检测是否还有箱子或墙
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, direction, _hitsBuffer,
                Vector2.Distance(transform.position, _newPos),
                1 << 7 | 1 << 8);

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitsBuffer[i].collider != GetComponent<Collider2D>()) return false;
            }

            return Commons.GetMovable(playerModel, _newPos);
        }

        public void Move(PlayerModel playerModel, Vector2Int direction)
        {
            transform.DOMove(_newPos, playerModel.moveTime);
        }
    }
}
