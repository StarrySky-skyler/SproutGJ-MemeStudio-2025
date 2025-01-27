// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// ********************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.MVC.Models;
using UnityEngine;

namespace Tsuki.Entities
{
    public class Box : MonoBehaviour
    {
        /// <summary>
        /// 获取箱子是否可推动
        /// </summary>
        /// <param name="playerModel">玩家模型</param>
        /// <param name="direction">推动方向</param>
        /// <returns></returns>
        public bool GetPushable(PlayerModel playerModel, Vector2Int direction)
        {
            Vector3 newPos = transform.position +
                             new Vector3(direction.x * playerModel.moveStep, direction.y * playerModel.moveStep, 0);
            Debug.DrawRay(transform.position, (Vector2)direction, Color.green, 3);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, Vector2.Distance(transform.position, newPos),
                1 << 7 | 1 << 8);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != GetComponent<Collider2D>()) return false;
            }

            return Commons.GetMovable(playerModel, newPos);
        }

        public void Move(PlayerModel playerModel, Vector2Int direction)
        {
            Vector3 newPos = transform.position +
                             new Vector3(direction.x * playerModel.moveStep, direction.y * playerModel.moveStep, 0);
            transform.DOMove(newPos, 0.2f);
        }
    }
}
