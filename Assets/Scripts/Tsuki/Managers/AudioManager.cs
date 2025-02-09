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
using Tsuki.Base;
using Tsuki.Entities.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class AudioManager : Singleton<AudioManager>
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
        private bool _allowAddCorrectBoxSfx; // 是否允许添加正确箱子音效，用于场切后防止立即播放
        private AudioEntity _audioEntity;

        //[CanBeNull] private AudioClip _lastMoveSoundEffect;
        private AudioFade _audioFade;
        private GameObject _audioGo;
        private AudioSource _bgmAudioSource;
        private AudioSource _sfxAudioSource;

        private void Start()
        {
            _audioGo = GameObject.FindWithTag("Audio");
            // 若未找到Audio对象，则实例化一个
            if (!_audioGo) _audioGo = Instantiate(audioPrefab);
            _bgmAudioSource = _audioGo.GetComponents<AudioSource>()[0];
            _sfxAudioSource = _audioGo.GetComponents<AudioSource>()[1];
            _audioEntity = _audioGo.GetComponent<AudioEntity>();
            _bgmAudioSource.loop = true;
            _sfxAudioSource.loop = false;
            //_lastMoveSoundEffect = null;
            _bgmAudioSource.volume = 0;
            // StartCoroutine(PlayBgm());
            PlayLevelBgm(false);
            // 注册事件
            GameManager.Instance.RegisterEvent(GameManagerEventType.OnGameUndo,
                () => { _audioEntity.PlaySfx(UNDO_SFX_NAME); });
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

        private void PlayLevelBgm(bool fadeOutLast = true)
        {
            if (!_audioEntity)
            {
                DebugYumihoshi.Warn<AudioManager>("音频", "音频实体为空");
                return;
            }
            _audioEntity.PlayBgm(GetCurrentLevelBgm(), fadeOutLast);
        }

        private AudioClip GetCurrentLevelBgm()
        {
            int level = LevelManager.Instance.GetCurrentLevel();
            DebugYumihoshi.Log<AudioManager>("音频", $"当前关卡为{level}");
            DebugYumihoshi.Log<AudioManager>("音频",
                $"当前关卡音频为{bgmList[level - 1]}");
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
                    _audioEntity.PlaySfx(MOVE_SFX_NAME);
                    break;
                case 4:
                case 5:
                    _audioEntity.PlaySfx(MOVE_ON_DRY_SFX_NAME);
                    break;
                case 6:
                case 7:
                    _audioEntity.PlaySfx(MOVE_ON_ICE_SFX_NAME);
                    break;
                default:
                    DebugYumihoshi.Error<AudioManager>("音频",
                        $"播放移动音效时，关卡数{LevelManager.Instance.GetCurrentLevel()}不合法");
                    break;
            }
        }

        /// <summary>
        ///     播放胜利或失败音效
        /// </summary>
        /// <param name="winStatus"></param>
        private void PlayWinSoundEffect(bool winStatus)
        {
            _audioEntity.PlaySfx(winStatus
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
            _audioEntity.PlaySfx(FAIL_SFX_NAME);
            while (_sfxAudioSource.isPlaying) yield return null;

            callback?.Invoke();
        }
    }
}
