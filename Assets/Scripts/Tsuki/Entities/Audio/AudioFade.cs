// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/01 17:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using DG.Tweening;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Game;
using UnityEngine;

namespace Tsuki.Entities.Audio
{
    public class AudioFade
    {
        private readonly GameModel _gameModel;

        public AudioFade(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        /// <summary>
        ///     淡入
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onCompleted"></param>
        public void FadeIn(AudioSource audioSource,
            [CanBeNull] Action onCompleted = null)
        {
            audioSource.Play();
            audioSource.DOFade(1, _gameModel.fadeInTime)
                .SetEase(_gameModel.fadeInEase)
                .OnComplete(() => { onCompleted?.Invoke(); });
        }

        /// <summary>
        ///     淡出
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onCompleted"></param>
        public void FadeOut(AudioSource audioSource,
            [CanBeNull] Action onCompleted = null)
        {
            audioSource.DOFade(0, _gameModel.fadeOutTime)
                .SetEase(_gameModel.fadeOutEase).OnComplete(() =>
                {
                    audioSource.Stop();
                    onCompleted?.Invoke();
                });
        }
    }
}
