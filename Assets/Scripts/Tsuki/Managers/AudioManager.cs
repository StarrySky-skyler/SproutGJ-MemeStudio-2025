// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 20:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using System.Collections.Generic;
using AnRan;
using DG.Tweening;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("音频组件")] public AudioSource bgmAudioSource;
        public AudioSource soundEffectAudioSource;

        [Header("Bgm")] public List<AudioClip> bgmList;

        [Header("音效")] public List<AudioClip> soundEffectList;

        [Header("渐入渐出时间")] public float fadeInTime;
        public float fadeOutTime;

        private PlayerModel _playerModel;
        private bool _moveSoundSwitch;

        protected override void Awake()
        {
            base.Awake();
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
        }

        private void Start()
        {
            bgmAudioSource.loop = true;
            soundEffectAudioSource.loop = false;
            // 注册事件
            _playerModel.OnMoveStateChanged += PlayMoveSoundEffect;
            BoxManager.Instance.OnWinChanged += PlayWinSoundEffect;
        }

        private void PlayMoveSoundEffect(bool moveState)
        {
            if (!moveState) return;
            if (_moveSoundSwitch)
            {
                PlaySoundEffect("Move a cat");
            }
            else
            {
                PlaySoundEffect("Move a cat2");
            }
            _moveSoundSwitch = !_moveSoundSwitch;
        }

        private void PlayWinSoundEffect(bool winState)
        {
            if (winState)
                PlaySoundEffect("Victroy this pat");
            else
                PlaySoundEffect("Fail");
        }

        /// <summary>
        /// 播放Bgm
        /// </summary>
        /// <param name="bgmName"></param>
        public void PlayBgm(string bgmName)
        {
            // 正在播放，渐出后渐入
            if (bgmAudioSource.isPlaying)
            {
                FadeOut(bgmAudioSource, () =>
                {
                    SetBgm(bgmName);
                    FadeIn(bgmAudioSource);
                });
            }
            // 未播放，直接渐入
            else
            {
                SetBgm(bgmName);
                FadeIn(bgmAudioSource);
            }
        }

        /// <summary>
        /// 播放一次音效
        /// </summary>
        /// <param name="soundEffectName"></param>
        public void PlaySoundEffect(string soundEffectName)
        {
            AudioClip clip = Resources.Load<AudioClip>("Tsuki/AudioClips/SoundEffect/" + soundEffectName);

            soundEffectAudioSource.PlayOneShot(clip);
        }

        private void SetBgm(string bgmName)
        {
            AudioClip clip = Resources.Load<AudioClip>("Tsuki/AudioClips/Bgm/" + bgmName);
            bgmAudioSource.clip = clip;
        }

        private void FadeIn(AudioSource audioSource, [CanBeNull] Action onCompleted = null)
        {
            audioSource.DOFade(1, fadeInTime).OnComplete(() => { onCompleted?.Invoke(); });
        }

        private void FadeOut(AudioSource audioSource, [CanBeNull] Action onCompleted = null)
        {
            audioSource.DOFade(0, fadeOutTime).OnComplete(() => { onCompleted?.Invoke(); });
        }
    }
}
