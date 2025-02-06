// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using System.Collections.Generic;
using DG.Tweening;
using Tsuki.Base;
using Tsuki.Entities.TPPoint;
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
        private bool _added;
        private Vector2Int _lastPushDirection;
        public Tween MoveTween { get; private set; }

        private void Awake()
        {
            _lastPosStack = new Stack<Vector3>();
        }

        private void Start()
        {
            _startPos = transform.position;
            _newPos = transform.position;
        }

        private void OnEnable()
        {
            GameManager.Instance.onGameUndo.AddListener(Undo);
        }

        private void OnDisable()
        {
            if (!GameManager.Instance) return;
            GameManager.Instance.onGameUndo.RemoveListener(Undo);
        }

        /// <summary>
        /// 推动箱子
        /// </summary>
        /// <returns></returns>
        public bool TryPushBox(Vector2Int pushDirection)
        {
            if (!GetPushable(pushDirection)) return false;
            Move();
            return true;
        }

        /// <summary>
        /// 获取箱子是否可推动
        /// </summary>
        /// <returns></returns>
        private bool GetPushable(Vector2Int pushDirection)
        {
            SetNewPos(pushDirection);
            Debug.DrawRay(transform.position,
                (Vector2)ModelsManager.Instance.PlayerMod.LastDirection,
                Color.green, 3);
            // 射线检测是否还有箱子或墙
            int hitCount = Physics2D.RaycastNonAlloc(transform.position,
                ModelsManager.Instance.PlayerMod.LastDirection, _hitsBuffer,
                Vector2.Distance(transform.position, _newPos),
                ModelsManager.Instance.PlayerMod.obstacleLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitsBuffer[i].collider != GetComponent<Collider2D>())
                    return false;
            }

            return Commons.IsOnMap(ModelsManager.Instance.PlayerMod, _newPos);
        }

        /// <summary>
        /// 设置新位置
        /// </summary>
        private void SetNewPos(Vector2Int pushDirection)
        {
            _lastPushDirection = pushDirection;
            _newPos = transform.position +
                      new Vector3(
                          pushDirection.x *
                          ModelsManager.Instance.PlayerMod.girdSize,
                          pushDirection.y *
                          ModelsManager.Instance.PlayerMod.girdSize,
                          0);
        }

        /// <summary>
        /// 移动箱子
        /// </summary>
        private void Move()
        {
            MoveTween = transform.DOMove(_newPos, ModelsManager.Instance.PlayerMod.moveTime)
                .OnComplete(
                    () =>
                    {
                        // TP检测
                        Collider2D hit1 =
                            Physics2D.OverlapPoint(_newPos, 1 << 9);
                        if (hit1)
                        {
                            hit1.GetComponent<TpPoint>().Tp(transform);
                            return;
                        }
                        // 冰层移动
                        Collider2D hit =
                            Physics2D.OverlapPoint(_newPos, 1 << 3);
                        if (!hit) return;
                        if (!TryPushBox(_lastPushDirection)) return;
                    });
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
