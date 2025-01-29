// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using DG.Tweening;
using Tsuki.Base;
using Tsuki.Interface;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Entities
{
    public class Box : MonoBehaviour, IPushable, IUndoable
    {
        public bool OnCorrectPos { get; private set; }

        private Vector3 _newPos;
        private Vector3 _originalPos;
        private readonly RaycastHit2D[] _hitsBuffer = new RaycastHit2D[10];
        private PlayerModel _playerModel;
        private Vector3 _correctPos;

        private void Awake()
        {
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
        }

        private void Start()
        {
            _correctPos = transform.Find("CorrectPos").position;
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
            Debug.DrawRay(transform.position, (Vector2)_playerModel.moveDirection, Color.green, 3);
            // 射线检测是否还有箱子或墙
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, _playerModel.moveDirection, _hitsBuffer,
                Vector2.Distance(transform.position, _newPos),
                _playerModel.obstacleLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitsBuffer[i].collider != GetComponent<Collider2D>()) return false;
            }

            return Commons.GetMovable(_playerModel, _newPos);
        }

        private void SetNewPos()
        {
            _newPos = transform.position +
                      new Vector3(_playerModel.moveDirection.x * _playerModel.girdSize,
                          _playerModel.moveDirection.y * _playerModel.girdSize, 0);
        }

        private void Move()
        {
            _originalPos = transform.position;
            transform.DOMove(_newPos, _playerModel.moveTime).OnComplete(() =>
            {
                OnCorrectPos = transform.position == _correctPos;
            });
        }

        public void Undo()
        {
            transform.position = _originalPos;
        }
    }
}
