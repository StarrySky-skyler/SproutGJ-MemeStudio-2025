// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using DG.Tweening;
using JetBrains.Annotations;
using Tsuki.Base;
using Tsuki.Entities;
using Tsuki.MVC.Models;
using Tsuki.MVC.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tsuki.MVC.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerView playerView;

        private PlayerModel _playerModel;
        private bool _isMoving;

        private void Awake()
        {
            // MVC 初始化
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
        }

        private void Start()
        {
            _playerModel.Init();
        }

        public void OnMove(InputValue context)
        {
            bool moveX = false;
            bool moveY = false;
            GetLineMovable(ref moveX, ref moveY);
            Move(context.Get<Vector2>(), moveX, moveY);
        }

        /// <summary>
        /// 获取是否在线上，用于判断是否可以移动
        /// </summary>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        /// <returns></returns>
        private void GetLineMovable(ref bool movableX, ref bool movableY)
        {
            movableY = transform.position.x == (int)transform.position.x;
            movableX = transform.position.y == (int)transform.position.y;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        private void Move(Vector2 vector, bool movableX, bool movableY)
        {
            if (_isMoving) return;
            if (vector == Vector2.zero) return;
            Vector2Int moves = Vector2Int.RoundToInt(vector);
            Vector2 move = moves;

            // 每次移动设置格数
            move.x *= _playerModel.moveStep;
            move.y *= _playerModel.moveStep;
            Vector3 pos = transform.position;

            // 获取新位置
            if (movableX) pos += new Vector3(move.x, 0, 0);
            if (movableY) pos += new Vector3(0, move.y, 0);

            // 检测是否在地图范围内
            if (pos.x != (int)pos.x && pos.y != (int)pos.y) return;
            if (!Commons.GetMovable(_playerModel, pos)) return;

            // 检测是否有箱子
            bool canMove = true;
            Debug.DrawRay(transform.position, move, Color.red, 3f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, move, Vector2.Distance(transform.position, pos),
                1 << 7);
            if (hit.collider != null)
            {
                Box box = hit.collider.GetComponent<Box>();
                canMove = box.GetPushable(_playerModel, moves);
                if (canMove)
                {
                    box.Move(_playerModel, moves);
                }
            }

            if (canMove)
            {
                _isMoving = true;
                transform.DOMove(pos, 0.2f).OnComplete(() => { _isMoving = false; });
            }
        }
    }
}
