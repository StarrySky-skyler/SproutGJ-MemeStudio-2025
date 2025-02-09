// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 00:01
// @version: 1.0
// @description: 游戏管理器单例
// *****************************************************************************

using Tsuki.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public enum GameManagerEventType
    {
        OnGamePause = 1,
        OnGameResume,
        OnGameUndo,
        BeforeGameReload
    }

    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent onGamePause = new();
        public UnityEvent onGameResume = new();
        public UnityEvent onGameUndo = new();
        public UnityEvent beforeGameReload = new();
        public UnityEvent<bool> onAllowLoadGame = new();

        private bool _allowLoadGame;

        public bool AllowLoadGame
        {
            get => _allowLoadGame;
            set
            {
                if (_allowLoadGame == value) return;
                _allowLoadGame = value;
                onAllowLoadGame?.Invoke(value);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform.parent);
        }

        private void Start()
        {
            if (!GameObject.Find("chatBack")) AllowLoadGame = true;
            SceneManager.sceneLoaded += (_, _) =>
            {
                AllowLoadGame = false;
                if (!GameObject.Find("chatBack")) AllowLoadGame = true;
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onGamePause.RemoveAllListeners();
            onGameResume.RemoveAllListeners();
            onGameUndo.RemoveAllListeners();
        }

        public void OnPause(InputValue context)
        {
            onGamePause?.Invoke();
            Time.timeScale = 0;
        }

        public void OnResume(InputValue context)
        {
            Time.timeScale = 1;
            onGameResume?.Invoke();
        }

        public void OnReload(InputValue context)
        {
            beforeGameReload?.Invoke();
            AudioManager.Instance.WaitPlayFailSfx(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        public void OnUndo(InputValue context)
        {
            // 如果正在移动则不允许撤销
            if (ModelsManager.Instance.PlayerMod.IsMoving) return;
            // 如果回到起点，不允许撤销
            if (ModelsManager.Instance.PlayerMod.CurrentLeftStep ==
                ModelsManager.Instance.PlayerMod.GetCurrentLevelMaxStep())
                return;
            onGameUndo?.Invoke();
        }

        public void OnNextLevel(InputValue context)
        {
#if UNITY_EDITOR
            LevelManager.Instance.LoadNextLevel(true);
#endif
        }

        /// <summary>
        ///     注册事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void RegisterEvent(GameManagerEventType type, UnityAction action)
        {
            switch (type)
            {
                case GameManagerEventType.OnGamePause:
                    onGamePause.AddListener(action);
                    break;
                case GameManagerEventType.OnGameResume:
                    onGameResume.AddListener(action);
                    break;
                case GameManagerEventType.OnGameUndo:
                    onGameUndo.AddListener(action);
                    break;
                case GameManagerEventType.BeforeGameReload:
                    beforeGameReload.AddListener(action);
                    break;
                default:
                    DebugYumihoshi.Error<GameManager>("游戏管理器单例", "注册事件失败");
                    break;
            }
        }

        /// <summary>
        ///     注销事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void UnregisterEvent(GameManagerEventType type,
            UnityAction action)
        {
            switch (type)
            {
                case GameManagerEventType.OnGamePause:
                    onGamePause.RemoveListener(action);
                    break;
                case GameManagerEventType.OnGameResume:
                    onGameResume.RemoveListener(action);
                    break;
                case GameManagerEventType.OnGameUndo:
                    onGameUndo.RemoveListener(action);
                    break;
                case GameManagerEventType.BeforeGameReload:
                    beforeGameReload.RemoveListener(action);
                    break;
                default:
                    DebugYumihoshi.Error<GameManager>("游戏管理器单例", "注销事件失败");
                    break;
            }
        }
    }
}
