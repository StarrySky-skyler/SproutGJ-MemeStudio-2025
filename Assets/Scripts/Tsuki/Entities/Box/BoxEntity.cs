// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using System.Collections.Generic;
using DG.Tweening;
using Tsuki.Base;
using Tsuki.Interface;
using Tsuki.Managers;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Entities.Box
{ 
    public class BoxEntity : MonoBehaviour, IPushable, IUndoable
    {
        public BoxType boxType;
        
        private Vector3 _newPos;
        private Vector3 _startPos;
        private Stack<Vector3> _lastPosStack;
        private readonly RaycastHit2D[] _hitsBuffer = new RaycastHit2D[10];
        private PlayerModel _playerModel;
        private bool _added;

        private void Awake()
        {
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            _lastPosStack = new Stack<Vector3>();
        }

        private void Start()
        {
            _startPos = transform.position;
            _newPos = transform.position;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGameUndo += Undo;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameUndo -= Undo;
        }

        /// <summary>
        /// 推动箱子
        /// </summary>
        /// <returns></returns>
        public bool TryPushBox()
        {
            if (!GetPushable()) return false;
            Move();
            return true;
        }

        /// <summary>
        /// 获取箱子是否可推动
        /// </summary>
        /// <returns></returns>
        private bool GetPushable()
        {
            SetNewPos();
            Debug.DrawRay(transform.position, (Vector2)_playerModel.LastDirection, Color.green, 3);
            // 射线检测是否还有箱子或墙
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, _playerModel.LastDirection, _hitsBuffer,
                Vector2.Distance(transform.position, _newPos),
                _playerModel.obstacleLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitsBuffer[i].collider != GetComponent<Collider2D>()) return false;
            }

            return Commons.IsOnMap(_playerModel, _newPos);
        }

        /// <summary>
        /// 设置新位置
        /// </summary>
        private void SetNewPos()
        {
            _newPos = transform.position +
                      new Vector3(_playerModel.LastDirection.x * _playerModel.girdSize,
                          _playerModel.LastDirection.y * _playerModel.girdSize, 0);
        }

        /// <summary>
        /// 移动箱子
        /// </summary>
        private void Move()
        {
            transform.DOMove(_newPos, _playerModel.moveTime);
        }

        public void Undo()
        {
            // 回到上一个位置
            if (!_lastPosStack.TryPop(out Vector3 result)) return;
            transform.position = result;
        }

        /// <summary>
        /// 重复上一个位置
        /// </summary>
        public void RepeatPos()
        {
            if (!_lastPosStack.TryPeek(out Vector3 result))
            {
                _lastPosStack.Push(_startPos);
                return;
            }
            _lastPosStack.Push(transform.position);
        }
    }
}
