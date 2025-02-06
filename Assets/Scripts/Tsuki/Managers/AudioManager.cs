// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 20:01
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Tsuki.Managers
{
    /// <summary>
    /// 音效类型
    /// </summary>
    public enum SoundEffectType
    {
        Move = 1,
        Win,
        Fail
    }

    public class AudioManager : Singleton<AudioManager>
    {
        [Header("音频组件")] public AudioSource bgmAudioSource;
        public AudioSource soundEffectAudioSource;

        [Header("渐入渐出时间")] public float fadeInTime;
        public float fadeOutTime;

        [Header("移动音效")] public AudioClip moveSoundEffect;

        [Header("胜利音效")] public AudioClip winSoundEffect;

        [Header("失败音效")] public AudioClip failSoundEffect;

        [Header("BGM")] public List<AudioClip> bgmList;
        
        [Header("生日BGM")] public AudioClip birthdayBgm;

        [HideInInspector]
        public bool allowRandomBgm = true;

        //[CanBeNull] private AudioClip _lastMoveSoundEffect;
        private AudioFade _audioFade;

        protected override void Awake()
        {
            base.Awake();
            _audioFade = new AudioFade(this);
        }

        private void Start()
        {
            bgmAudioSource.loop = false;
            soundEffectAudioSource.loop = false;
            //_lastMoveSoundEffect = null;
            bgmAudioSource.volume = 0;
            StartCoroutine(PlayBgm());
        }

        private void OnEnable()
        {
            // 注册事件
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged.AddListener(
                RandomPlayMoveSoundEffect);
            BoxManager.Instance.onWinChanged.AddListener(PlayWinSoundEffect);
        }

        private void OnDisable()
        {
            // 注销事件
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged
                .RemoveListener(RandomPlayMoveSoundEffect);
            BoxManager.Instance.onWinChanged.RemoveListener(PlayWinSoundEffect);
        }

        private IEnumerator PlayBgm()
        {
            while (allowRandomBgm)
            {
                RandomPlayBgm();
                yield return new WaitForSeconds(bgmAudioSource.clip.length -
                                                fadeOutTime);
                _audioFade.FadeOut(bgmAudioSource);
                yield return new WaitForSeconds(fadeOutTime);
            }
        }

        private void PlayMoveSoundEffect()
        {
            soundEffectAudioSource.PlayOneShot(moveSoundEffect);
        }

        /// <summary>
        /// 播放移动音效
        /// </summary>
        /// <param name="moveStatus"></param>
        private void RandomPlayMoveSoundEffect(bool moveStatus)
        {
            if (!moveStatus) return;
            PlaySoundEffect(SoundEffectType.Move);
        }

        /// <summary>
        /// 播放胜利或失败音效
        /// </summary>
        /// <param name="winStatus"></param>
        private void PlayWinSoundEffect(bool winStatus)
        {
            PlaySoundEffect(winStatus
                ? SoundEffectType.Win
                : SoundEffectType.Fail);
        }

        /// <summary>
        /// 随机循环播放Bgm
        /// </summary>
        private void RandomPlayBgm()
        {
            SetRandomBgm();
            _audioFade.FadeIn(bgmAudioSource);
        }

        /// <summary>
        /// 播放一次音效
        /// </summary>
        /// <param name="soundEffectType"></param>
        public void PlaySoundEffect(SoundEffectType soundEffectType)
        {
            switch (soundEffectType)
            {
                case SoundEffectType.Move:
                    // 第一次播放移动音效
                    // AudioClip clip = null;
                    // if (!_lastMoveSoundEffect)
                    // {
                    //     clip = moveSoundEffect[
                    //         Random.Range(0, moveSoundEffect.Count - 1)];
                    // }
                    // // 移动音效与上次不同，播放新的移动音效
                    // else
                    // {
                    //     clip = _lastMoveSoundEffect;
                    //     while (clip == _lastMoveSoundEffect)
                    //     {
                    //         clip = moveSoundEffect[
                    //             Random.Range(0, moveSoundEffect.Count - 1)];
                    //     }
                    // }
                    //
                    // soundEffectAudioSource.PlayOneShot(clip);
                    // _lastMoveSoundEffect = clip;
                    PlayMoveSoundEffect();
                    break;
                case SoundEffectType.Win:
                    soundEffectAudioSource.PlayOneShot(winSoundEffect);
                    break;
                case SoundEffectType.Fail:
                    soundEffectAudioSource.PlayOneShot(failSoundEffect);
                    break;
                default:
                    Debug.LogError($"{soundEffectType.ToString()}音效类型不存在");
                    break;
            }
        }

        private void SetRandomBgm()
        {
            AudioClip lastClip = bgmAudioSource.clip;
            AudioClip clip = lastClip;
            while (clip == lastClip)
            {
                clip = bgmList[Random.Range(0, bgmList.Count - 1)];
            }

            bgmAudioSource.clip = clip;
        }

        /// <summary>
        /// 播放生日Bgm
        /// </summary>
        public void PlayBirthdayBgm()
        {
            allowRandomBgm = false;
            StopCoroutine(PlayBgm());
            _audioFade.FadeOut(bgmAudioSource, () =>
            {
                bgmAudioSource.clip = birthdayBgm;
                _audioFade.FadeIn(bgmAudioSource);
            });
        }
    }
}
