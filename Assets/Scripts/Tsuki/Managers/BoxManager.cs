// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/29 23:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class BoxManager : Singleton<BoxManager>
    {
        public bool Win
        {
            get => _win;
            private set
            {
                if (_win == value) return;
                _win = value;
                Debug.Log(_win ? $"所有箱子已归位" : $"所有箱子未归位");
                OnWinChanged?.Invoke(_win);
            }
        }

        [CanBeNull] public event Action<bool> OnWinChanged;

        private bool _win;
        private int _boxCount;
        private int _boxCorrectCount;

        private void Start()
        {
            ResetBoxCount();
        }

        private void OnEnable()
        {
            // 注册事件
            SceneManager.sceneLoaded += ResetBoxCount;
        }
        
        private void OnDisable()
        {
            // 注销事件
            SceneManager.sceneLoaded -= ResetBoxCount;
        }

        private void ResetBoxCount(Scene scene, LoadSceneMode mode)
        {
            ResetBoxCount();
        }

        private void ResetBoxCount()
        {
            _boxCorrectCount = 0;
            _win = false;
            _boxCount = GameObject.FindGameObjectsWithTag("Box").Length;
        }

        /// <summary>
        /// 增加正确的箱子
        /// </summary>
        public void AddCorrectBox()
        {
            Debug.Log($"增加正确的箱子");
            _boxCorrectCount = Mathf.Min(_boxCorrectCount + 1, _boxCount);
            CheckWin();
        }
        
        /// <summary>
        /// 减少正确的箱子
        /// </summary>
        public void RemoveCorrectBox()
        {
            Debug.Log($"减少正确的箱子");
            _boxCorrectCount = Mathf.Max(_boxCorrectCount - 1, 0);
        }

        private void CheckWin()
        {
            Win = _boxCorrectCount == _boxCount;
        }
    }
}
