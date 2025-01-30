// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 00:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [CanBeNull] public event Action OnGamePause;
        [CanBeNull] public event Action OnGameResume;
        [CanBeNull] public event Action OnGameUndo;
        
        private PlayerModel _playerModel;

        protected override void Awake()
        {
            base.Awake();
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            DontDestroyOnLoad(gameObject);
        }

        public void OnPause(InputValue context)
        {
            OnGamePause?.Invoke();
            Time.timeScale = 0;
        }

        public void OnResume(InputValue context)
        {
            Time.timeScale = 1;
            OnGameResume?.Invoke();
        }

        public void OnReload(InputValue context)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnUndo(InputValue context)
        {
            // 如果正在移动则不允许撤销
            if (_playerModel.IsMoving) return;
            OnGameUndo?.Invoke();
        }
    }
}
