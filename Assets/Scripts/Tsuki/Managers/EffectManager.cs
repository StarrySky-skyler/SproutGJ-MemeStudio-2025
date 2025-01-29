// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 22:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using AnRan;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Managers
{
    public class EffectManager : Singleton<EffectManager>
    {
        [Header("脚印特效")] 
        [CanBeNull] public GameObject footPrint;
        
        private PlayerModel _playerModel;

        protected override void Awake()
        {
            base.Awake();
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
        }
        
        private void Start()
        {
            // 注册事件
            if (footPrint) _playerModel.OnMoveStateChanged += SpawnFootPrint;
        }
        
        private void OnDestroy()
        {
            // 注销事件
            if (footPrint) _playerModel.OnMoveStateChanged -= SpawnFootPrint;
        }

        /// <summary>
        /// 生成脚本特效
        /// </summary>
        /// <param name="moveState"></param>
        private void SpawnFootPrint(bool moveState)
        {
            if (!moveState) return;
            Instantiate(footPrint, _playerModel.lastPos, Quaternion.identity);
        }
    }
}
