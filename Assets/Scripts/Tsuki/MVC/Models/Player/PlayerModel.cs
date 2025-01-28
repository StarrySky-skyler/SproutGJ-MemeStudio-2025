// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Tsuki.MVC.Models.Player
{
    [CreateAssetMenu(fileName = "PlayerModel", menuName = "Tsuki/New PlayerModel", order = 0)]
    public class PlayerModel : ScriptableObject
    {
        [Header("移动范围")]
        public Vector2 moveRange;
        
        [Header("移动一格的时间（箱子也是）")]
        public float moveTime;
        
        [Header("单元格大小")]
        public float girdSize;
        
        [Header("血量")]
        public int maxHp;
        
        [Header("障碍物")]
        public LayerMask obstacleLayer;
        
        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value) return;
                _isMoving = value;
                OnMoveStateChanged?.Invoke(_isMoving);
            }
        }
        [CanBeNull] public event Action<bool> OnMoveStateChanged; 
        
        [HideInInspector]
        public Vector2Int moveDirection;
        
        private int _currentHp;
        private bool _isMoving;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _currentHp = maxHp;
            _isMoving = false;
        }
        
        /// <summary>
        /// 玩家受到伤害
        /// </summary>
        /// <param name="damage">伤害值</param>
        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            _currentHp = Mathf.Clamp(_currentHp - damage, 0, maxHp);
        }

        /// <summary>
        /// 玩家回复生命值
        /// </summary>
        /// <param name="heal">回复量</param>
        public void Heal(int heal)
        {
            if (heal <= 0) return;
            _currentHp = Mathf.Clamp(_currentHp + heal, 0, maxHp);
        }
    }
}
