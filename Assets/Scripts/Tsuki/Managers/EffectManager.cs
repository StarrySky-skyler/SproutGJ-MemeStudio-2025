// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 22:01
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using JetBrains.Annotations;
using Tsuki.Base;
using Tsuki.MVC.Models.Player;
using Tsuki.Effects;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class EffectManager : Singleton<EffectManager>
    {
        [Header("脚印特效")] [CanBeNull] public GameObject footPrint;
        private ObjectPool<GameObject> _footPool;

        protected override void Awake()
        {
            base.Awake();
            _footPool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet,
                ActionOnRelease, ActionOnDestroy, true, 30,
                60);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += (_, _) =>
            {
                _footPool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet,
                    ActionOnRelease, ActionOnDestroy, true, 30,
                    60);
            };
        }

        private void OnEnable()
        {
            // 注册事件
            if (footPrint)
                ModelsManager.Instance.PlayerMod.onMoveStatusChanged
                    .AddListener(SpawnFootPrintInPool);
        }

        private void OnDisable()
        {
            // 注销事件
            if (footPrint)
                ModelsManager.Instance.PlayerMod.onMoveStatusChanged
                    .RemoveListener(SpawnFootPrintInPool);
        }

        /// <summary>
        /// 生成脚本特效
        /// </summary>
        /// <param name="moveStatus"></param>
        private void SpawnFootPrint(bool moveStatus)
        {
            if (!moveStatus ||
                !ModelsManager.Instance.PlayerMod.LastPosStack.TryPeek(
                    out Vector3 result)) return;
            Instantiate(footPrint, result, Quaternion.identity);
        }

        private void SpawnFootPrintInPool(bool moveStatus)
        {
            if (!moveStatus ||
                !ModelsManager.Instance.PlayerMod.LastPosStack.TryPeek(
                    out Vector3 result)) return;
            GameObject obj = _footPool.Get();
            obj.transform.position = result;
            obj.transform.rotation = Quaternion.identity;
        }


        private GameObject CreateFunc()
        {
            GameObject obj = Instantiate(footPrint);

            obj.GetComponent<Footprint>().footPool = _footPool;

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
