// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/06 19:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using Tsuki.Base;
using Tsuki.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tsuki.Entities.Audio
{
    public class AudioEntity : MonoBehaviour
    {
        [HideInInspector] public AudioSource[] audioSource;
        private AudioFade _audioFade;

        private AudioClip _sfxClick;

        private void Start()
        {
            // 防止重复
            if (FindObjectsByType<AudioEntity>(FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
            // 初始化
            _audioFade = new AudioFade(ModelsManager.Instance.GameMod);
            _sfxClick = Resources.Load<AudioClip>("Music/Sfx/Got Hurt");
            if (!_sfxClick)
                DebugYumihoshi.Warn<AudioEntity>("全局音频", "鼠标点击音效为空，加载失败");

            audioSource = GetComponents<AudioSource>();
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                audioSource[0].loop = true;
                audioSource[0].Play();
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame) PlaySfxClick();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += (_, _) =>
            {
                if (SceneManager.GetActiveScene().name == "Select")
                    PlayBgm("Exotic Lingzhi Paradise");
            };
        }

        /// <summary>
        /// 播放鼠标点击音效
        /// </summary>
        private void PlaySfxClick()
        {
            if (!_sfxClick) return;
            audioSource[2].Stop();
            audioSource[2].PlayOneShot(_sfxClick);
        }

        /// <summary>
        ///     播放Bgm
        /// </summary>
        /// <param name="bgmName"></param>
        /// <param name="fadeOut">上一曲是否渐出，若未播放则此项无用</param>
        /// <exception cref="NotImplementedException"></exception>
        public void PlayBgm(string bgmName, bool fadeOut = true)
        {
            AudioClip targetAudio =
                Resources.Load<AudioClip>("Music/Bgm/" + bgmName);
            if (!targetAudio)
            {
                DebugYumihoshi.Error<AudioEntity>("全局音频", "Bgm为空，加载失败");
                return;
            }

            if (audioSource[0].clip == targetAudio)
            {
                DebugYumihoshi.Warn<AudioEntity>("全局音频", "Bgm重复，不播放");
                return;
            }

            DebugYumihoshi.Log<AudioEntity>("全局音频", $"播放Bgm{bgmName}");
            if (fadeOut && audioSource[0].isPlaying)
            {
                _audioFade.FadeOut(audioSource[0], () =>
                {
                    audioSource[0].clip = targetAudio;
                    _audioFade.FadeIn(audioSource[0]);
                });
            }
            else
            {
                audioSource[0].clip = targetAudio;
                _audioFade.FadeIn(audioSource[0]);
            }
        }

        /// <summary>
        ///     播放Bgm
        /// </summary>
        /// <param name="bgmClip"></param>
        /// <param name="fadeOut">上一曲是否渐出，若未播放则此项无用</param>
        public void PlayBgm(AudioClip bgmClip, bool fadeOut = true)
        {
            if (bgmClip == audioSource[0].clip)
            {
                DebugYumihoshi.Warn<AudioEntity>("全局音频", "Bgm重复，不播放");
                return;
            }

            DebugYumihoshi.Log<AudioEntity>("全局音频", $"播放Bgm{bgmClip.name}");
            if (fadeOut && audioSource[0].isPlaying)
            {
                _audioFade.FadeOut(audioSource[0], () =>
                {
                    audioSource[0].clip = bgmClip;
                    _audioFade.FadeIn(audioSource[0]);
                });
            }
            else
            {
                audioSource[0].clip = bgmClip;
                _audioFade.FadeIn(audioSource[0]);
            }
        }

        /// <summary>
        ///     播放音效
        /// </summary>
        /// <param name="sfxName">Resources下Audio的音效文件名</param>
        public void PlaySfx(string sfxName)
        {
            DebugYumihoshi.Log<AudioEntity>("全局音频", $"播放音效{sfxName}");
            AudioClip clip = Resources.Load<AudioClip>("Music/Sfx/" + sfxName);
            if (!clip)
            {
                DebugYumihoshi.Error<AudioEntity>("全局音频", "音效为空，加载失败");
                return;
            }

            audioSource[1].PlayOneShot(clip);
        }

        public void PlaySfx(AudioClip clip)
        {
            DebugYumihoshi.Log<AudioEntity>("全局音频", $"播放音效{clip.name}");
            if (!clip)
            {
                DebugYumihoshi.Error<AudioEntity>("全局音频", "音效为空，加载失败");
                return;
            }

            audioSource[1].PlayOneShot(clip);
        }
    }
}
