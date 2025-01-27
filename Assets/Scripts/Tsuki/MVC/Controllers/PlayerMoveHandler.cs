// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 23:01
// @version: 1.0
// @description:
// ********************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.Entities;
using Tsuki.MVC.Models;
using UnityEngine;

namespace Tsuki.MVC.Controllers
{
    public class PlayerMoveHandler
    {
        private readonly PlayerController _playerController;
        private readonly PlayerModel _playerModel;
        private bool _isMoving;
        
        public PlayerMoveHandler(PlayerController playerController, PlayerModel playerModel)
        {
            _playerController = playerController;
            _playerModel = playerModel;
        }
        
        /// <summary>
        /// 获取是否在线上，用于判断是否可以移动
        /// </summary>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        /// <returns></returns>
        public void GetLineMovable(out bool movableX, out bool movableY)
        {
            movableY = _playerController.transform.position.x % 1 == 0;
            movableX = _playerController.transform.position.y % 1 == 0;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        public void Move(Vector2 vector, bool movableX, bool movableY)
        {
            if (_isMoving || vector == Vector2.zero) return;
            Vector2Int direction = Vector2Int.RoundToInt(vector);
            Vector2 scaledDirection = (Vector2)direction * _playerModel.girdSize;
            Vector3 newPos = _playerController.transform.position;

            // 获取新位置
            if (movableX) newPos += new Vector3(scaledDirection.x, 0, 0);
            if (movableY) newPos += new Vector3(0, scaledDirection.y, 0);

            // 检测是否在地图范围内
            if (newPos.x % 1 != 0 && newPos.y % 1 != 0) return;

            if (!Commons.GetMovable(_playerModel, newPos)) return;

            // 检测是否有箱子
            bool canMove = true;
            Debug.DrawRay(_playerController.transform.position, scaledDirection, Color.red, 3f);
            RaycastHit2D hit = Physics2D.Raycast(_playerController.transform.position, scaledDirection,
                Vector2.Distance(_playerController.transform.position, newPos),
                1 << 7 | 1 << 8);
            if (hit.collider)
            {
                if (hit.collider.gameObject.layer == 8) return;
                Box box = hit.collider.GetComponent<Box>();
                canMove = box.GetPushable(_playerModel, direction);
                if (canMove)
                {
                    box.Move(_playerModel, direction);
                }
            }

            if (!canMove) return;
            _isMoving = true;
            _playerController.transform.DOMove(newPos, 0.2f).OnComplete(() => { _isMoving = false; });
        }
    }
}
