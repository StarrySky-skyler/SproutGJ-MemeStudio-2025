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
    public enum SoundEffectType
    {
        move1 = 1,
        move2,
        victory,
        fail,
        strange
    }

    public class AudioManager : Singleton<AudioManager>
    {
        [Header("音频组件")] public AudioSource bgmAudioSource;
        public AudioSource soundEffectAudioSource;

        [Header("Bgm")] public List<AudioClip> bgmList;

        [Header("音效")] public List<AudioClip> soundEffectList;

        [Header("渐入渐出时间")] public float fadeInTime;
        public float fadeOutTime;

        private PlayerModel _playerModel;

        private void Start()
        {
            bgmAudioSource.loop = true;
            soundEffectAudioSource.loop = false;
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            // 注册事件
            _playerModel.OnMoveStateChanged += PlayMoveSoundEffect;
        }

        private void PlayMoveSoundEffect(bool moveState)
        {
            if (moveState)
                PlaySoundEffect(UnityEngine.Random.Range(1, 3) == 1 ? SoundEffectType.move1 : SoundEffectType.move2);
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
        /// <param name="soundEffectType"></param>
        public void PlaySoundEffect(SoundEffectType soundEffectType)
        {
            AudioClip clip = null;
            switch (soundEffectType)
            {
                case SoundEffectType.move1:
                    clip = soundEffectList.Find(clip => clip.name == "Move a cat");
                    break;
                case SoundEffectType.move2:
                    clip = soundEffectList.Find(clip => clip.name == "Move a cat2");
                    break;
                case SoundEffectType.victory:
                    clip = soundEffectList.Find(clip => clip.name == "Victory this pat");
                    break;
                case SoundEffectType.fail:
                    clip = soundEffectList.Find(clip => clip.name == "Fail this pat");
                    break;
                case SoundEffectType.strange:
                    clip = soundEffectList.Find(clip => clip.name == "Strange Open meme");
                    break;
            }

            soundEffectAudioSource.PlayOneShot(clip);
        }

        private void SetBgm(string bgmName)
        {
            AudioClip clip = bgmList.Find(clip => clip.name == bgmName);
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
