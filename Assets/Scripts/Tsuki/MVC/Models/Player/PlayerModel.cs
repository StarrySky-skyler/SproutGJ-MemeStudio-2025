// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tsuki.MVC.Models.Player
{
    [CreateAssetMenu(fileName = "PlayerModel",
        menuName = "Tsuki/New Player Config", order = 0)]
    public class PlayerModel : ScriptableObject
    {
        [Header("移动一格的时间（箱子也是）")] public float moveTime;

        [Header("最大移动步数")] public int maxMoveStep;

        public int CurrentLeftStep
        {
            get => _currentLeftStep;
            private set
            {
                if (_currentLeftStep == value) return;
                int originalStep = _currentLeftStep;
                _currentLeftStep = value;
                onStepChanged?.Invoke(_currentLeftStep,
                    _currentLeftStep > originalStep);
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

        public UnityEvent<bool> onMoveStatusChanged;

        public UnityEvent<int, bool> onStepChanged;

        public Vector2Int LastDirection { get; set; } // 移动方向
        public Vector3 CurrentPos { get; set; }
        public Stack<Vector3> LastPosStack { get; private set; } // 上一次的位置

        private bool _isMoving;

        private int _currentLeftStep;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            CurrentLeftStep = maxMoveStep;
            _isMoving = false;
            LastPosStack = new Stack<Vector3>();
            CurrentPos = GameObject.FindWithTag("Player").transform.position;
        }

        public void AddStep(int step = 1)
        {
            CurrentLeftStep =
                Mathf.Clamp(CurrentLeftStep + step, 0, maxMoveStep);
        }

        public void ReduceStep(int step = 1)
        {
            CurrentLeftStep =
                Mathf.Clamp(CurrentLeftStep - step, 0, maxMoveStep);
        }
    }
}
