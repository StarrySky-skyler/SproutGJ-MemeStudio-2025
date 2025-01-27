// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using JetBrains.Annotations;
using Tsuki.MVC.Models;
using Tsuki.MVC.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tsuki.MVC.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _playerModel;
        [CanBeNull] private PlayerView _playerView;

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
        /// 获取是否在地图范围内可移动
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool GetMovable(Vector3 pos)
        {
            bool tag = pos.x >= 0;
            tag = tag && pos.x <= _playerModel.moveRange.x;
            tag = tag && pos.y >= 0;
            tag = tag && pos.y <= _playerModel.moveRange.y;
            return tag;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="movableX"></param>
        /// <param name="movableY"></param>
        private void Move(Vector2 vector, bool movableX, bool movableY)
        {
            Vector2Int moves = Vector2Int.RoundToInt(vector);
            Vector2 move = moves;
            // 每次移动设置格数
            move.x *= _playerModel.moveStep;
            move.y *= _playerModel.moveStep;
            Vector3 pos = transform.position;
            // 移动
            if (movableX) pos += new Vector3(move.x, 0, 0);
            if (movableY) pos += new Vector3(0, move.y, 0);
            if (pos.x != (int)pos.x && pos.y != (int)pos.y) return;
            if (!GetMovable(pos)) return;
            transform.position = pos;
        }
    }
}
