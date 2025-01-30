// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 22:01
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using JetBrains.Annotations;
using Tsuki.MVC.Models.Player;
using Tsuki.Effects;
using UnityEngine;
using UnityEngine.Pool;
namespace Tsuki.Managers
{
    public class EffectManager : Singleton<EffectManager>
    {
        [Header("脚印特效")] 
        [CanBeNull] public GameObject footPrint;
        public ObjectPool<GameObject> footPool;
        
        private PlayerModel _playerModel;

        protected override void Awake()
        {
            base.Awake();
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            footPool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 30, 60);
        }

        private void OnEnable()
        {
            // 注册事件
            if (footPrint) _playerModel.OnMoveStateChanged += SpawnFootPrintInPool;
        }

        private void OnDisable()
        {
            // 注销事件
            if (footPrint) _playerModel.OnMoveStateChanged -= SpawnFootPrintInPool;
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

        private void SpawnFootPrintInPool(bool moveState)
        {
            if (!moveState) return;
            GameObject obj = footPool.Get();
            obj.transform.position = _playerModel.lastPos;
        }


        private GameObject CreateFunc()
        {
            GameObject obj = Instantiate(footPrint, _playerModel.lastPos, Quaternion.identity);

            obj.GetComponent<Footprint>().footPool = footPool;

            return obj;
        }

        private void ActionOnDestroy(GameObject obj)
        {
            Destroy(obj);
        }

        private void ActionOnRelease(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void ActionOnGet(GameObject obj)
        {
            obj.SetActive(true);
        }

    }
}
