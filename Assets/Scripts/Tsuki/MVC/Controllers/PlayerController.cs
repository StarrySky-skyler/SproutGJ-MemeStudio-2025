// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

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
        private PlayerMoveHandler _playerMoveHandler;

        private void Awake()
        {
            // MVC 初始化
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            _playerMoveHandler = new PlayerMoveHandler(this, _playerModel);
        }

        private void Start()
        {
            _playerModel.Init();
        }

        public void OnMove(InputValue context)
        {
            _playerMoveHandler.GetLineMovable(out bool moveX, out bool moveY);
            _playerMoveHandler.Move(context.Get<Vector2>(), moveX, moveY);
        }
    }
}
