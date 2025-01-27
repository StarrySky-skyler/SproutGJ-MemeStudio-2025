// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tsuki.MVC.Models
{
    [CreateAssetMenu(fileName = "PlayerModel", menuName = "Tsuki/New PlayerModel", order = 0)]
    public class PlayerModel : ScriptableObject
    {
        [Header("移动范围")]
        public Vector2Int moveRange;
        
        [FormerlySerializedAs("moveStep")] [Header("单元格大小")]
        public float girdSize;
        
        [Header("血量")]
        public int maxHp;
        
        private int _currentHp;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _currentHp = maxHp;
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
