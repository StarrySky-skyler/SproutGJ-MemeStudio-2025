// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections.Generic;
using Tsuki.Base;
using Tsuki.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Tsuki.MVC.Models.Player
{
    [CreateAssetMenu(fileName = "PlayerModel",
        menuName = "Tsuki/New Player Config", order = 0)]
    public class PlayerModel : ScriptableObject
    {
        [Header("移动一格的时间（箱子也是）")] public float moveTime;

        [Header("关卡最大移动步数")] public List<int> maxMoveSteps;

        public UnityEvent<bool> onMoveStatusChanged = new();

        public UnityEvent<int, bool> onStepChanged = new();

        private int _currentLeftStep;

        private bool _isMoving;

        public int CurrentLeftStep
        {
            get => _currentLeftStep;
            private set
            {
                if (_currentLeftStep == value) return;
                bool tags = value > _currentLeftStep;
                _currentLeftStep = value;
                onStepChanged?.Invoke(_currentLeftStep, tags);
            }
        }

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value) return;
                _isMoving = value;
                onMoveStatusChanged?.Invoke(_isMoving);
            }
        }

        public Vector2Int LastDirection { get; set; } // 移动方向
        public Vector3 CurrentPos { get; set; }
        public Stack<Vector3> LastPosStack { get; private set; } // 上一次的位置

        /// <summary>
        ///     初始化
        /// </summary>
        public void Init()
        {
            CurrentLeftStep =
                SceneManager.GetActiveScene().name.Contains("Level")
                    ? GetCurrentLevelMaxStep()
                    : 0;
            _isMoving = false;
            LastPosStack = new Stack<Vector3>();
            if (SceneManager.GetActiveScene().name.Contains("Level"))
                CurrentPos = GameObject.FindWithTag("Player").transform.position;
        }

        public void AddStep(int step = 1)
        {
            CurrentLeftStep =
                Mathf.Clamp(CurrentLeftStep + step, 0,
                    GetCurrentLevelMaxStep());
        }

        public void ReduceStep(int step = 1)
        {
            CurrentLeftStep =
                Mathf.Clamp(CurrentLeftStep - step, 0,
                    GetCurrentLevelMaxStep());
        }

        public int GetCurrentLevelMaxStep()
        {
            if (LevelManager.Instance.GetCurrentLevel() <= maxMoveSteps.Count &&
                LevelManager.Instance.GetCurrentLevel() >= 1)
                return maxMoveSteps
                    [LevelManager.Instance.GetCurrentLevel() - 1];
            DebugYumihoshi.Error<PlayerModel>("玩家model",
                "初始化当前步数时出错，关卡数越界");
            return _currentLeftStep;
        }
    }
}
