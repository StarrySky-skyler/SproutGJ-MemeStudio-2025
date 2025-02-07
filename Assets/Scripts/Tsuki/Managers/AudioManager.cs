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
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [Header("Audio预制体")] public GameObject audioPrefab;

        [Header("渐入渐出时间")] public float fadeInTime;
        public float fadeOutTime;

        [Header("移动音效")] public AudioClip moveSoundEffect;

        [Header("胜利音效")] public AudioClip winSoundEffect;

        [Header("失败音效")] public AudioClip failSoundEffect;

        [Header("BGM")] public List<AudioClip> bgmList;

        [Header("生日BGM")] public AudioClip birthdayBgm;

        [HideInInspector] public bool allowRandomBgm = true;

        //[CanBeNull] private AudioClip _lastMoveSoundEffect;
        private AudioFade _audioFade;
        private AudioSource _bgmAudioSource;
        private AudioSource _sfxAudioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioFade = new AudioFade(this);
        }

        private void Start()
        {
            GameObject audioGo = GameObject.FindWithTag("Audio");
            // 若未找到Audio对象，则实例化一个
            if (!audioGo) audioGo = Instantiate(audioPrefab);
            _bgmAudioSource = audioGo.GetComponents<AudioSource>()[0];
            _sfxAudioSource = audioGo.GetComponents<AudioSource>()[1];
            _bgmAudioSource.loop = true;
            _sfxAudioSource.loop = false;
            //_lastMoveSoundEffect = null;
            _bgmAudioSource.volume = 0;
            // StartCoroutine(PlayBgm());
            PlayLevelBgm(false);
            SceneManager.sceneLoaded += (scene, mode) => { PlayLevelBgm(); };
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

        private void PlayLevelBgm(bool fadeOutLast = true)
        {
            MannulPlayBgm(GetCurrentLevelBgm(), fadeOutLast);
        }

        /// <summary>
        /// 手动切换播放Bgm
        /// </summary>
        /// <param name="fadeOutLast">是否先渐出</param>
        /// <param name="targetAudio">目标Bgm</param>
        public void MannulPlayBgm(AudioClip targetAudio, bool fadeOutLast = true)
        {
            if (fadeOutLast)
            {
                _audioFade.FadeOut(_bgmAudioSource, () =>
                {
                    _bgmAudioSource.clip = targetAudio;
                    _audioFade.FadeIn(_bgmAudioSource);
                });
            }
            else
            {
                _bgmAudioSource.clip = targetAudio;
                _audioFade.FadeIn(_bgmAudioSource);
            }
        }

        private AudioClip GetCurrentLevelBgm()
        {
            int level = LevelManager.Instance.GetCurrentLevel();
            Debug.Log("音频：当前关卡为" + level);
            Debug.Log("音频：当前关卡音频为" + bgmList[level - 1]);
            return ModelsManager.Instance.GameMod.bgmList[level - 1];
        }

        private IEnumerator PlayBgm()
        {
            while (allowRandomBgm)
            {
                RandomPlayBgm();
                yield return new WaitForSeconds(_bgmAudioSource.clip.length -
                                                fadeOutTime);
                _audioFade.FadeOut(_bgmAudioSource);
                yield return new WaitForSeconds(fadeOutTime);
            }
        }

        private void PlayMoveSoundEffect()
        {
            _sfxAudioSource.PlayOneShot(moveSoundEffect);
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
            _audioFade.FadeIn(_bgmAudioSource);
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
                    PlayMoveSoundEffect();
                    break;
                case SoundEffectType.Win:
                    _sfxAudioSource.PlayOneShot(winSoundEffect);
                    break;
                case SoundEffectType.Fail:
                    _sfxAudioSource.PlayOneShot(failSoundEffect);
                    break;
                default:
                    Debug.LogError($"{soundEffectType.ToString()}音效类型不存在");
                    break;
            }
        }

        private void SetRandomBgm()
        {
            AudioClip lastClip = _bgmAudioSource.clip;
            AudioClip clip = lastClip;
            while (clip == lastClip)
            {
                clip = bgmList[Random.Range(0, bgmList.Count - 1)];
            }

            _bgmAudioSource.clip = clip;
        }

        /// <summary>
        /// 播放生日Bgm
        /// </summary>
        public void PlayBirthdayBgm()
        {
            allowRandomBgm = false;
            StopCoroutine(PlayBgm());
            _audioFade.FadeOut(_bgmAudioSource, () =>
            {
                _bgmAudioSource.clip = birthdayBgm;
                _audioFade.FadeIn(_bgmAudioSource);
            });
        }
    }
}
