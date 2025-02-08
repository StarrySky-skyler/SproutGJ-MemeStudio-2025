// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Interface;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.MVC.Views.Player
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerAnimationHandler _animationHandler;

        private void Awake()
        {
            // MVC 初始化
            // 初始化处理器
            _animationHandler = new PlayerAnimationHandler(this);
        }

        private void OnEnable()
        {
            // 注册事件
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged.AddListener(
                _animationHandler.PlayAnimation);
            GameManager.Instance.RegisterEvent(GameManagerEventType.OnGamePause,
                (_animationHandler as IPauseable).Pause);
            GameManager.Instance.RegisterEvent(
                GameManagerEventType.OnGameResume,
                (_animationHandler as IPauseable).Resume);
        }

        private void OnDisable()
        {
            if (!ModelsManager.Instance.PlayerMod) return;
            // 注销事件
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged
                .RemoveListener(_animationHandler.PlayAnimation);
            GameManager.Instance.UnregisterEvent(
                GameManagerEventType.OnGamePause,
                (_animationHandler as IPauseable).Pause);
            GameManager.Instance.UnregisterEvent(
                GameManagerEventType.OnGameResume,
                (_animationHandler as IPauseable).Resume);
        }
    }
}
