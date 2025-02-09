// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 18:02
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
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

        [Header("存档槽位总数")] public int archiveCount;
        [Header("最大关卡数")] public int maxLevel;

        [Header("音频")] [Header("渐入时间")] public float fadeInTime = 10f;

        [Header("渐出时间")] public float fadeOutTime = 10f;
        [Header("渐入曲线")] public Ease fadeInEase = Ease.InOutQuad;
        [Header("渐出曲线")] public Ease fadeOutEase = Ease.InOutQuad;
    }
}
