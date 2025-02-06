// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 21:01
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Interface;
using Tsuki.Managers;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.MVC.Views.Player
{
    public class PlayerAnimationHandler : IPauseable
    {
        private static readonly int Move = Animator.StringToHash("Move");
        private readonly PlayerView _playerView;
        private readonly Animator _animator;
        private readonly SpriteRenderer _spriteRenderer;
        private bool _allowFlip = true;

        public PlayerAnimationHandler(PlayerView playerView)
        {
            _playerView = playerView;
            _animator = _playerView.GetComponent<Animator>();
            _spriteRenderer = _playerView.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="moveStatus"></param>
        public void PlayAnimation(bool moveStatus)
        {
            if (ModelsManager.Instance.PlayerMod.LastDirection.x != 0 && _allowFlip)
            {
                _spriteRenderer.flipX = ModelsManager.Instance.PlayerMod.LastDirection.x < 0;
            }

            _animator.SetBool(Move, moveStatus);
        }

        public void Pause()
        {
            _allowFlip = false;
        }

        public void Resume()
        {
            _allowFlip = true;
        }
    }
}
