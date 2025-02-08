// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/04 11:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Tsuki.Base;
using Tsuki.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Tsuki.Entities.CameraController
{
    public class CameraController : MonoBehaviour
    {
        [Header("聚焦物体")] [Header("繁茂城邦")] [CanBeNull]
        public Transform lushCityTrans;
        [Header("干旱城邦")] [CanBeNull] public Transform dryCityTrans;

        [Header("严寒城邦")] [CanBeNull] public Transform coldCityTrans;

        [Header("废土")] [CanBeNull] public Transform wasteLandTrans;

        [Header("相机移动时间")] public float moveTime;
        [Header("聚焦后视野大小")] public float targetFieldOfView;
        [Header("聚焦所需时间")] public float zoomTime;

        private Vector3 _originPos;
        private float _originFieldOfView;
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _originPos = Commons.GetModifiedPos(transform.position);
            _originFieldOfView = _camera.orthographicSize;
            // FocusOnTarget(FocusTargetType.LushCity);
        }

        /// <summary>
        /// 聚焦到目标位置
        /// </summary>
        public void FocusOnTarget(FocusTargetType targetType)
        {
            // target为空，直接返回
            Transform targetTrans = targetType switch
            {
                FocusTargetType.LushCity => lushCityTrans,
                FocusTargetType.DryCity => dryCityTrans,
                FocusTargetType.ColdCity => coldCityTrans,
                FocusTargetType.WasteLand => wasteLandTrans,
                _ => throw new ArgumentOutOfRangeException(nameof(targetType),
                    targetType, "聚焦枚举类型错误")
            };
            if (!targetTrans)
            {
                Debug.LogError("聚焦目标为空");
                return;
            }

            Focus(targetTrans);
        }

        /// <summary>
        /// 聚焦到给定的目标位置，z轴会自动修正
        /// </summary>
        /// <param name="trans"></param>
        public void FocusOnTarget(Transform trans)
        {
            Focus(trans);
        }

        private void Focus(Transform targetPos)
        {
            Vector3 newPos = Commons.GetModifiedPos(targetPos.position);
            // 移动
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(newPos, moveTime).SetEase(Ease.InOutQuad));

            sequence.Append(DOTween.To(() => _camera.orthographicSize,
                x => _camera.orthographicSize = x,
                targetFieldOfView, zoomTime).SetEase(Ease.InOutQuad));

            if (Vector3.Distance(transform.position, newPos) < 0.1f && (_camera.orthographicSize - targetFieldOfView) < 0.1f)
            {
                StopAllCoroutines();
                StartCoroutine(SelectScene());
            }
            // 聚焦

        }
        public void Reset()
        {
            transform.DOMove(_originPos, moveTime).SetEase(Ease.InOutQuad);
            DOTween.To(() => _camera.orthographicSize,
                x => _camera.orthographicSize = x,
                _originFieldOfView, zoomTime).SetEase(Ease.InOutQuad);
        }

        IEnumerator SelectScene()
        {
            yield return null;
            SceneManager.LoadScene(AnRan.GameManager.Instance.selectSaveData.level + 2);
        }
    }

    public enum FocusTargetType
    {
        LushCity = 1,
        DryCity,
        ColdCity,
        WasteLand
    }
}
