// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/08 23:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tsuki.Entities.ScreenMask
{
    public class ScreenMask : MonoBehaviour
    {
        [Header("渐进时间")] public float fadeInTime;
        [Header("渐退时间")] public float fadeOutTime;

        private Image _img;

        private void Awake()
        {
            _img = GetComponent<Image>();
            SceneManager.sceneLoaded += (_, _) =>
            {
                FadeOut();
            };
            DontDestroyOnLoad(transform.parent.gameObject);
        }

        private void Start()
        {
            FadeOut();
        }

        public void FadeIn(Action onCompleted = null)
        {
            _img.DOColor(Color.black, fadeInTime).OnComplete(() =>
            {
                onCompleted?.Invoke();
            });
        }

        public void FadeOut(Action onCompleted = null)
        {
            _img.DOColor(Color.clear, fadeOutTime).OnComplete((() =>
            {
                onCompleted?.Invoke();
            }));
        }
    }
}
