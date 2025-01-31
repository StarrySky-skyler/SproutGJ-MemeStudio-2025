// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using Tsuki.Interface;
using Tsuki.Managers;
using Tsuki.MVC.Models.Player;
using Tsuki.MVC.Views.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tsuki.MVC.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        // TODO: 使用状态机管理玩家流程
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
            _moveHandler.GetLineMovable(out bool moveX, out bool moveY);
            _moveHandler.Move(context.Get<Vector2>(), moveX, moveY);
        }

        private void OnEnable()
        {
            // 注册事件
            GameManager.Instance.OnGamePause += (_moveHandler as IPauseable).Pause;
            GameManager.Instance.OnGameResume += (_moveHandler as IPauseable).Resume;
            GameManager.Instance.OnGameUndo += (_moveHandler as IUndoable).Undo;
        }

        private void OnDisable()
        {
            // 注销事件
            GameManager.Instance.OnGamePause -= (_moveHandler as IPauseable).Pause;
            GameManager.Instance.OnGameResume -= (_moveHandler as IPauseable).Resume;
            GameManager.Instance.OnGameUndo -= (_moveHandler as IUndoable).Undo;
        }
    }
}
