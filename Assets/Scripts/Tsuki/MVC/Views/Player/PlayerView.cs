// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Interface;
using Tsuki.Managers;
using Tsuki.MVC.Models.Player;
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
            ModelsManager.Instance.playerModel.onMoveStatusChanged.AddListener(
                _animationHandler.PlayAnimation);
            GameManager.Instance.onGamePause.AddListener(
                (_animationHandler as IPauseable).Pause);
            GameManager.Instance.onGameResume.AddListener(
                (_animationHandler as IPauseable).Resume);
        }

        private void OnDisable()
        {
            if (!ModelsManager.Instance.playerModel) return;
            // 注销事件
            ModelsManager.Instance.playerModel.onMoveStatusChanged
                .RemoveListener(_animationHandler.PlayAnimation);
            if (!GameManager.Instance) return;
            GameManager.Instance.onGamePause.RemoveListener(
                (_animationHandler as IPauseable).Pause);
            GameManager.Instance.onGameResume.RemoveListener(
                (_animationHandler as IPauseable).Resume);
        }
    }
}
