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
using UnityEngine;

namespace Tsuki.MVC.Views.Player
{
    public class PlayerView : MonoBehaviour
    {
        [HideInInspector]
        public PlayerModel playerModel;

        private PlayerAnimationHandler _animationHandler;

        private void Awake()
        {
            // MVC 初始化
            playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            // 初始化处理器
            _animationHandler = new PlayerAnimationHandler(this);
        }

        private void OnEnable()
        {
            // 注册事件
            playerModel.OnMoveStatusChanged += _animationHandler.PlayAnimation;
            GameManager.Instance.OnGamePause += (_animationHandler as IPauseable).Pause;
            GameManager.Instance.OnGameResume += (_animationHandler as IPauseable).Resume;
        }
        
        private void OnDisable()
        {
            // 注销事件
            playerModel.OnMoveStatusChanged -= _animationHandler.PlayAnimation;
            GameManager.Instance.OnGamePause -= (_animationHandler as IPauseable).Pause;
            GameManager.Instance.OnGameResume -= (_animationHandler as IPauseable).Resume;
        }
    }
}
