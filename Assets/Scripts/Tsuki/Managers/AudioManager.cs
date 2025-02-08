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
using Tsuki.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class AudioManager : Singleton<AudioManager>, IAudio
    {
        private const string UNDO_SFX_NAME = "Move a cat3";
        private const string MOVE_SFX_NAME = "Move a cat4";
        private const string MOVE_ON_DRY_SFX_NAME = "Move a cat_On_Dry";
        private const string MOVE_ON_ICE_SFX_NAME = "Move a cat_On_Ice";
        private const string WIN_SFX_NAME = "Victroy this pat";
        private const string FAIL_SFX_NAME = "Fail this pat";
        [Header("Audio预制体")] public GameObject audioPrefab;

        [Header("渐入渐出时间")] public float fadeInTime;
        public float fadeOutTime;

        [Header("关卡BGM配置")] public List<AudioClip> bgmList;

        //[CanBeNull] private AudioClip _lastMoveSoundEffect;
        private AudioFade _audioFade;
        private AudioSource _bgmAudioSource;
        private AudioSource _sfxAudioSource;
        private bool _allowAddCorrectBoxSfx; // 是否允许添加正确箱子音效，用于场切后防止立即播放

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
            // 注册事件
            GameManager.Instance.RegisterEvent(GameManagerEventType.OnGameUndo,
                () => { PlaySfx(UNDO_SFX_NAME); });
        }

        private void OnEnable()
        {
            // 注册事件
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged.AddListener(
                RandomPlayMoveSoundEffect);

            BoxManager.Instance.onWinChanged.AddListener(PlayWinSoundEffect);
            BoxManager.Instance.onBoxCorrectAdded.AddListener(() =>
            {
                if (_allowAddCorrectBoxSfx) PlayWinSoundEffect(true);
            });

            SceneManager.sceneLoaded += (_, _) =>
            {
                GameObject audio = GameObject.FindWithTag("Audio");
                if (!audio) audio = Instantiate(audioPrefab);
                _bgmAudioSource = audio.GetComponents<AudioSource>()[0];
                _sfxAudioSource = audio.GetComponents<AudioSource>()[1];
                _bgmAudioSource.loop = true;
                _sfxAudioSource.loop = false;
            };
            SceneManager.sceneLoaded += (_, _) => { PlayLevelBgm(false); };
            SceneManager.sceneLoaded += (_, _) =>
            {
                StartCoroutine(WaitCorrectBoxSfx());
            };
        }

        private void OnDisable()
        {
            // 注销事件
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged
                .RemoveListener(RandomPlayMoveSoundEffect);
            BoxManager.Instance.onWinChanged.RemoveListener(PlayWinSoundEffect);
        }

        private IEnumerator WaitCorrectBoxSfx()
        {
            _allowAddCorrectBoxSfx = false;
            yield return new WaitForSeconds(1.5f);
            _allowAddCorrectBoxSfx = true;
        }

        /// <summary>
        ///     播放音效
        /// </summary>
        /// <param name="sfxName">Resources下Audio的音效文件名</param>
        public void PlaySfx(string sfxName)
        {
            AudioClip clip = Resources.Load<AudioClip>("Music/Sfx/" + sfxName);
            if (!clip)
            {
                Debug.LogError("音频：未找到音效" + sfxName);
                return;
            }

            _sfxAudioSource.PlayOneShot(clip);
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
                Debug.LogError("音频：未找到bgm" + bgmName);
                return;
            }

            if (fadeOut && _bgmAudioSource.isPlaying)
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

        private void PlayLevelBgm(bool fadeOutLast = true)
        {
            PlayBgm(GetCurrentLevelBgm(), fadeOutLast);
        }

        private AudioClip GetCurrentLevelBgm()
        {
            int level = LevelManager.Instance.GetCurrentLevel();
            Debug.Log("音频：当前关卡为" + level);
            Debug.Log("音频：当前关卡音频为" +
                      bgmList[level - 1]);
            return bgmList[level - 1];
        }

        /// <summary>
        ///     播放移动音效
        /// </summary>
        /// <param name="moveStatus"></param>
        private void RandomPlayMoveSoundEffect(bool moveStatus)
        {
            if (!moveStatus) return;
            switch (LevelManager.Instance.GetCurrentLevel())
            {
                case 1:
                case 2:
                case 3:
                case 8:
                    PlaySfx(MOVE_SFX_NAME);
                    break;
                case 4:
                case 5:
                    PlaySfx(MOVE_ON_DRY_SFX_NAME);
                    break;
                case 6:
                case 7:
                    PlaySfx(MOVE_ON_ICE_SFX_NAME);
                    break;
                default:
                    Debug.LogError(
                        $"[ERROR] AudioManager >>> 播放移动音效时，关卡数{LevelManager.Instance.GetCurrentLevel()}不合法");
                    break;
            }
        }

        /// <summary>
        ///     播放胜利或失败音效
        /// </summary>
        /// <param name="winStatus"></param>
        private void PlayWinSoundEffect(bool winStatus)
        {
            PlaySfx(winStatus
                ? WIN_SFX_NAME
                : FAIL_SFX_NAME);
        }

        /// <summary>
        ///     等待播放失败音效
        /// </summary>
        /// <param name="callback"></param>
        public void WaitPlayFailSfx(Action callback = null)
        {
            StartCoroutine(WaitForPlayFailSfx(callback));
        }

        private IEnumerator WaitForPlayFailSfx(Action callback = null)
        {
            PlaySfx(FAIL_SFX_NAME);
            while (_sfxAudioSource.isPlaying) yield return null;

            callback?.Invoke();
        }

        /// <summary>
        ///     播放Bgm
        /// </summary>
        /// <param name="bgmClip"></param>
        /// <param name="fadeOut">上一曲是否渐出，若未播放则此项无用</param>
        public void PlayBgm(AudioClip bgmClip, bool fadeOut = true)
        {
            if (fadeOut && _bgmAudioSource.isPlaying)
            {
                _audioFade.FadeOut(_bgmAudioSource, () =>
                {
                    _bgmAudioSource.clip = bgmClip;
                    _audioFade.FadeIn(_bgmAudioSource);
                });
            }
            else
            {
                _bgmAudioSource.clip = bgmClip;
                _audioFade.FadeIn(_bgmAudioSource);
            }
        }
    }
}
