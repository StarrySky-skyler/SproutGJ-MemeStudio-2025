// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using Tsuki.MVC.Models.Player;
using Tsuki.MVC.Views.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tsuki.MVC.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]
        public PlayerModel playerModel;
        [HideInInspector]
        public PlayerView playerView;
        
        private PlayerMoveHandler _moveHandler;

        private void Awake()
        {
            // MVC 初始化
            playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            playerView = GetComponent<PlayerView>();
            // 初始化处理器
            _moveHandler = new PlayerMoveHandler(this);
        }

        private void Start()
        {
            playerModel.Init();
        }

        public void OnMove(InputValue context)
        {
            // _playerMoveHandler.GetLineMovable(out bool moveX, out bool moveY);
            _moveHandler.Move(context.Get<Vector2>());
        }
    }
}
