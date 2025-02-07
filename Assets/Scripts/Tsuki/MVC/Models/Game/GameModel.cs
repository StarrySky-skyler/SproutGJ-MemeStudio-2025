// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 18:02
// @version: 1.0
// @description:
// *****************************************************************************

using UnityEngine;

namespace Tsuki.MVC.Models.Game
{
    [CreateAssetMenu(fileName = "GameModel", menuName = "Tsuki/New Game Config", order = 1)]
    public class GameModel : ScriptableObject
    {
        [Header("单元格大小")] public float girdSize;
        [Header("地面")] public LayerMask groundLayer;
        [Header("障碍物")] public LayerMask obstacleLayer;
        [Header("草层")] public LayerMask grassLayer;
        [Header("冰块层")]  public LayerMask groundIceLayer;
         public LayerMask groundIceLineLayer;
    }
}
