// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 00:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using AnRan;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("暂停UI预制体")]
        public GameObject pausePanel;
        
        private GameObject _pausePanel;

        protected override void Awake()
        {
            base.Awake();
        }
        
        private void Start()
        {
            ResetPauseUI();
            // 注册事件
            GameManager.Instance.OnGamePause += ShowPauseUI;
            GameManager.Instance.OnGameResume += HidePauseUI;
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                ResetPauseUI();
            };
        }

        /// <summary>
        /// 初始化暂停UI
        /// </summary>
        private void ResetPauseUI()
        {
            GameObject ui = GameObject.Find("UI");
            _pausePanel = Instantiate(pausePanel, ui.transform.position, Quaternion.identity, ui.transform);
            _pausePanel.SetActive(false);
        }

        /// <summary>
        /// 处理暂停UI
        /// </summary>
        private void ShowPauseUI()
        {
            _pausePanel.SetActive(true);
        }

        /// <summary>
        /// 处理暂停UI
        /// </summary>
        private void HidePauseUI()
        {
            _pausePanel.SetActive(false);
        }

        private void OnDestroy()
        {
            // 注销事件
            GameManager.Instance.OnGamePause -= ShowPauseUI;
            GameManager.Instance.OnGameResume -= HidePauseUI;
        }
    }
}
