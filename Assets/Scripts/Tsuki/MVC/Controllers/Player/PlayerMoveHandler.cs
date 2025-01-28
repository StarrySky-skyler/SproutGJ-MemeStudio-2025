// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 23:01
// @version: 1.0
// @description:
// ********************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.Interface;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.MVC.Controllers.Player
{
    public class PlayerMoveHandler : IUndoable
    {
        private readonly PlayerController _playerController;
        private readonly PlayerModel _playerModel;
        
        // 移动
        private Vector2 _scaledDirection;   // 移动方向向量 * 格子大小
        private Vector3 _originalPos;           // 原始位置
        private Vector3 _newPos;                // 新位置
        private bool _movableX;
        private bool _movableY;

        public PlayerMoveHandler(PlayerController playerController)
        {
            _playerController = playerController;
            _playerModel = playerController.playerModel;
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
        /// <param name="inputV2"></param>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        public void Move(Vector2 inputV2, bool movableX = true, bool movableY = true)
        {
            if (_playerModel.IsMoving || inputV2 == Vector2.zero) return;
            _movableX = movableX;
            _movableY = movableY;
            // 获取移动方向
            SetDirection(inputV2);
            // 获取新位置
            SetNewPos();

            // 检测是否在地图范围内
            // if (newPos.x % 1 != 0 && newPos.y % 1 != 0) return;

            if (!Commons.GetMovable(_playerModel, _newPos)) return;

            if (!CanMoveAfterDetect()) return;
            StartMove();
        }

        /// <summary>
        /// 设置移动方向
        /// </summary>
        /// <param name="input"></param>
        private void SetDirection(Vector2 input)
        {
            _playerModel.moveDirection = Vector2Int.RoundToInt(input);
            _scaledDirection = (Vector2)_playerModel.moveDirection * _playerModel.girdSize;
        }

        /// <summary>
        /// 设置新位置
        /// </summary>
        private void SetNewPos()
        {
            _originalPos = _playerController.transform.position;
            _newPos = _originalPos;

            // 获取新位置
            if (_movableX) _newPos += new Vector3(_scaledDirection.x, 0, 0);
            if (_movableY) _newPos += new Vector3(0, _scaledDirection.y, 0);
        }

        /// <summary>
        /// 开始移动
        /// </summary>
        private void StartMove()
        {
            _playerModel.IsMoving = true;
            _playerModel.lastPos = _newPos;
            _playerController.transform.DOMove(_newPos, _playerModel.moveTime)
                .OnComplete(() => { _playerModel.IsMoving = false; });
        }

        /// <summary>
        /// 检测箱子和墙后是否可以移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveAfterDetect()
        {
            bool canMove = true;
            Debug.DrawRay(_playerController.transform.position, _scaledDirection, Color.red, 3f);
            RaycastHit2D hit = Physics2D.Raycast(_playerController.transform.position, _scaledDirection,
                Vector2.Distance(_playerController.transform.position, _newPos),
                _playerModel.obstacleLayer);
            if (hit.collider)
            {
                if (hit.collider.gameObject.layer == 8) return false;
                IPushable box = hit.collider.GetComponent<IPushable>();
                canMove = box.TryPushBox();
            }

            return canMove;
        }

        /// <summary>
        /// 撤销移动操作
        /// </summary>
        public void Undo()
        {
            _playerController.transform.position = _playerModel.lastPos;
        }
    }
}
