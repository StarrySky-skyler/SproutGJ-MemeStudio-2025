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
using UnityEngine;

namespace Tsuki.Managers
{
    public class AudioFade
    {
        private readonly AudioManager _audioManager;

        public AudioFade(AudioManager audioManager)
        {
            _audioManager = AudioManager.Instance;
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
            audioSource.DOFade(1, _audioManager.fadeInTime).OnComplete(() =>
            {
                onCompleted?.Invoke();
            });
        }

        /// <summary>
        ///     淡出
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onCompleted"></param>
        public void FadeOut(AudioSource audioSource,
            [CanBeNull] Action onCompleted = null)
        {
            audioSource.DOFade(0, _audioManager.fadeOutTime).OnComplete(() =>
            {
                audioSource.Stop();
                onCompleted?.Invoke();
            });
        }
    }
}
