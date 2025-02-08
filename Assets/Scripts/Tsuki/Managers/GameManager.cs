// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 00:01
// @version: 1.0
// @description: 游戏管理器单例
// *****************************************************************************

using System;
using System.Collections;
using JetBrains.Annotations;
using Tsuki.Base;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

        private bool _allowLoadGame;

        private void Start()
        {
            AllowLoadGame = false;
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
            AudioManager.Instance.WaitPlayFailSFX(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        public void OnUndo(InputValue context)
        {
            // 如果正在移动则不允许撤销
            if (ModelsManager.Instance.PlayerMod.IsMoving) return;
            onGameUndo?.Invoke();
        }

        /// <summary>
        /// 注册事件
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
                    Debug.LogError("注册事件失败");
                    break;
            }
        }

        /// <summary>
        /// 注销事件
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
                    Debug.LogError("注销事件失败");
                    break;
            }
        }
    }
}
