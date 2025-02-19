﻿// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 23:01
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM;
using Tsuki.Entities.Box.FSM.Types;
using Tsuki.Interface;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.MVC.Controllers.Player
{
    public class PlayerMoveHandler : IUndoable, IPauseable
    {
        private readonly PlayerController _playerController;
        private bool _allowMove = true;
        private bool _movableX;
        private bool _movableY;
        private Vector3 _newPos; // 新位置
        private Vector3 _originalPos; // 原始位置

        // 移动
        private Vector2 _scaledDirection; // 移动方向向量 * 格子大小

        public PlayerMoveHandler(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Pause()
        {
            _allowMove = false;
        }

        public void Resume()
        {
            _allowMove = true;
        }

        /// <summary>
        ///     撤销移动操作
        /// </summary>
        public void Undo()
        {
            // 回到上一个位置
            if (ModelsManager.Instance.PlayerMod.LastPosStack.TryPop(
                    out Vector3 result))
                _playerController.transform.position = result;
            // 增加步数
            ModelsManager.Instance.PlayerMod.AddStep();
        }

        /// <summary>
        ///     获取是否在线上，用于判断是否可以移动
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
        ///     移动
        /// </summary>
        /// <param name="inputV2"></param>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        public void Move(Vector2 inputV2, bool movableX = true,
            bool movableY = true)
        {
            if (ModelsManager.Instance.PlayerMod.CurrentLeftStep == 0) return;
            if (ModelsManager.Instance.PlayerMod.IsMoving ||
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

            if (!Commons.IsOnMap(ModelsManager.Instance.GameMod, _newPos))
                return;

            if (!CanMoveAfterDetect()) return;
            StartMove();
        }

        /// <summary>
        ///     设置移动方向
        /// </summary>
        /// <param name="input"></param>
        private void SetDirection(Vector2 input)
        {
            ModelsManager.Instance.PlayerMod.LastDirection =
                Vector2Int.RoundToInt(input);
            _scaledDirection =
                (Vector2)ModelsManager.Instance.PlayerMod.LastDirection *
                ModelsManager.Instance.GameMod.girdSize;
        }

        /// <summary>
        ///     设置新位置
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
        ///     开始移动
        /// </summary>
        private void StartMove()
        {
            if (_playerController.transform.position == _newPos) return;
            ModelsManager.Instance.PlayerMod.ReduceStep();
            ModelsManager.Instance.PlayerMod.LastPosStack.Push(_originalPos);
            ModelsManager.Instance.PlayerMod.IsMoving = true;
            _playerController.transform.DOMove(_newPos,
                    ModelsManager.Instance.PlayerMod.moveTime)
                .OnComplete(() =>
                {
                    ModelsManager.Instance.PlayerMod.IsMoving = false;
                    ModelsManager.Instance.PlayerMod.CurrentPos = _newPos;
                });
        }

        /// <summary>
        ///     检测是否可以移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveAfterDetect()
        {
            return DetectObstacle() &&
                   Commons.IsOnMap(ModelsManager.Instance.GameMod, _newPos);
        }

        /// <summary>
        ///     检测墙和箱子
        /// </summary>
        /// <returns></returns>
        private bool DetectObstacle()
        {
            Debug.DrawRay(_playerController.transform.position,
                _scaledDirection, Color.red, 3f);
            RaycastHit2D hit = Physics2D.Raycast(
                _playerController.transform.position, _scaledDirection,
                Vector2.Distance(_playerController.transform.position, _newPos),
                ModelsManager.Instance.GameMod.obstacleLayer);
            if (!hit.collider) return true;
            if (((1 << hit.collider.gameObject.layer) &
                 (_playerController.wallLayer |
                  _playerController.grassLayer)) != 0)
                return false;

            // IPushable box = hit.collider.GetComponent<IPushable>();
            // return box.TryPushBox(ModelsManager.Instance.PlayerMod
            //     .LastDirection);
            BaseObj obj = hit.collider.GetComponent<BaseObj>();
            BoxStateMachine box = obj
                .StateMachine;
            return box.SwitchState(BoxStateType.PushMoving,
                new Context
                {
                    PushDirection =
                        ModelsManager.Instance.PlayerMod.LastDirection,
                    BoxType = obj.boxType
                });
        }
    }
}
