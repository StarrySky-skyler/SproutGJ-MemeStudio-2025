// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 21:01
// @version: 1.0
// @description:
// ********************************************************************************

using Tsuki.Interface;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.MVC.Views.Player
{
    public class PlayerAnimationHandler : IPauseable
    {
        private static readonly int Move = Animator.StringToHash("Move");
        private readonly PlayerView _playerView;
        private readonly PlayerModel _playerModel;
        private readonly Animator _animator;
        private readonly SpriteRenderer _spriteRenderer;
        private bool _allowFlip = true;

        public PlayerAnimationHandler(PlayerView playerView)
        {
            _playerView = playerView;
            _playerModel = _playerView.playerModel;
            _animator = _playerView.GetComponent<Animator>();
            _spriteRenderer = _playerView.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="moveState"></param>
        public void PlayAnimation(bool moveState)
        {
            if (_playerModel.LastDirection.x != 0 && _allowFlip)
            {
                _spriteRenderer.flipX = _playerModel.LastDirection.x < 0;
            }
            _animator.SetBool(Move, moveState);
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
