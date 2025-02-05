// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 23:01
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.Interface;
using Tsuki.Managers;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.MVC.Controllers.Player
{
    public class PlayerMoveHandler : IUndoable, IPauseable
    {
        private readonly PlayerController _playerController;

        // 移动
        private Vector2 _scaledDirection; // 移动方向向量 * 格子大小
        private Vector3 _originalPos; // 原始位置
        private Vector3 _newPos; // 新位置
        private bool _movableX;
        private bool _movableY;
        private bool _allowMove = true;

        public PlayerMoveHandler(PlayerController playerController)
        {
            _playerController = playerController;
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
        public void Move(Vector2 inputV2, bool movableX = true,
            bool movableY = true)
        {
            if (ModelsManager.Instance.playerModel.IsMoving ||
                inputV2 == Vector2.zero ||
                !_allowMove) return;
            _movableX = movableX;
            _movableY = movableY;
            // 获取移动方向
            SetDirection(inputV2);
            // 获取新位置
            SetNewPos();

            // 检测是否在地图范围内
            // if (newPos.x % 1 != 0 && newPos.y % 1 != 0) return;

            if (!Commons.IsOnMap(ModelsManager.Instance.playerModel, _newPos))
                return;

            if (!CanMoveAfterDetect()) return;
            StartMove();
        }

        /// <summary>
        /// 设置移动方向
        /// </summary>
        /// <param name="input"></param>
        private void SetDirection(Vector2 input)
        {
            ModelsManager.Instance.playerModel.LastDirection =
                Vector2Int.RoundToInt(input);
            _scaledDirection =
                (Vector2)ModelsManager.Instance.playerModel.LastDirection *
                ModelsManager.Instance.playerModel.girdSize;
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
            if (_playerController.transform.position == _newPos) return;
            ModelsManager.Instance.playerModel.LastPosStack.Push(_originalPos);
            ModelsManager.Instance.playerModel.IsMoving = true;
            _playerController.transform.DOMove(_newPos,
                    ModelsManager.Instance.playerModel.moveTime)
                .OnComplete(() =>
                {
                    ModelsManager.Instance.playerModel.IsMoving = false;
                    ModelsManager.Instance.playerModel.CurrentPos = _newPos;
                });
        }

        /// <summary>
        /// 检测是否可以移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveAfterDetect()
        {
            return DetectObstacle() &&
                   Commons.IsOnMap(ModelsManager.Instance.playerModel, _newPos);
        }

        /// <summary>
        /// 检测墙和箱子
        /// </summary>
        /// <returns></returns>
        private bool DetectObstacle()
        {
            bool canMove = true;
            Debug.DrawRay(_playerController.transform.position,
                _scaledDirection, Color.red, 3f);
            RaycastHit2D hit = Physics2D.Raycast(
                _playerController.transform.position, _scaledDirection,
                Vector2.Distance(_playerController.transform.position, _newPos),
                ModelsManager.Instance.playerModel.obstacleLayer);
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
            // 回到上一个位置
            if (ModelsManager.Instance.playerModel.LastPosStack.TryPop(
                    out Vector3 result))
                _playerController.transform.position = result;
        }

        public void Pause()
        {
            _allowMove = false;
        }

        public void Resume()
        {
            _allowMove = true;
        }
    }
}
