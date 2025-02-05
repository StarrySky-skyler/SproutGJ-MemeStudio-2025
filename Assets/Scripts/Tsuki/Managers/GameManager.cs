// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 00:01
// @version: 1.0
// @description: 游戏管理器单例
// *****************************************************************************

using System;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Tsuki.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent onGamePause;
        public UnityEvent onGameResume;
        public UnityEvent onGameUndo;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnUndo(InputValue context)
        {
            // 如果正在移动则不允许撤销
            if (ModelsManager.Instance.playerModel.IsMoving) return;
            onGameUndo?.Invoke();
        }
    }
}
