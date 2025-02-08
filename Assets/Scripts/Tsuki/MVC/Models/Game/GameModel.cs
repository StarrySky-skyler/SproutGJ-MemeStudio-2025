// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 18:02
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace Tsuki.MVC.Models.Game
{
    [CreateAssetMenu(fileName = "GameModel", menuName = "Tsuki/New Game Config",
        order = 1)]
    public class GameModel : ScriptableObject
    {
        [Header("单元格大小")] public float girdSize;
        [Header("地面")] public LayerMask groundLayer;
        [Header("障碍物")] public LayerMask obstacleLayer;
        [Header("草层")] public LayerMask grassLayer;
        [Header("冰块层")] public LayerMask groundIceLayer;
        public LayerMask groundIceLineLayer;

        [Header("关卡BGM配置")]
        public List<AudioClip> bgmList;
        
        [Header("存档槽位总数")] public int archiveCount;
        [Header("最大关卡数")] public int maxLevel;
    }
}
