// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/08 00:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using DG.Tweening;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.AutoWall
{
    public enum WallType
    {
        A,
        B,
        C,
        D,
    }

    public enum HandleType
    {
        None = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
    }

    public class AutoWall : MonoBehaviour
    {
        public WallType wallType;

        private Transform _spriteTrans;
        private Vector3 _startPos;
        private BoxCollider2D _boxCollider2D;
        private Tween _hideTween;

        private void Start()
        {
            _spriteTrans = transform.Find("Sprite");
            _startPos = _spriteTrans.position;
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            switch (wallType)
            {
                case WallType.A:
                case WallType.B:
                    ModelsManager.Instance.PlayerMod.onStepChanged.AddListener(
                        HandleDisplay);
                    break;
                case WallType.C:
                case WallType.D:
                    ModelsManager.Instance.PlayerMod.onStepChanged.AddListener(
                        HandleDisplayReverse);
                    break;
                default:
                    Debug.LogError("自动墙类型错误");
                    break;
            }
        }

        private void OnDisable()
        {
            switch (wallType)
            {
                case WallType.A:
                case WallType.B:
                    ModelsManager.Instance.PlayerMod.onStepChanged
                        .RemoveListener(
                            HandleDisplay);
                    break;
                case WallType.C:
                case WallType.D:
                    ModelsManager.Instance.PlayerMod.onStepChanged
                        .RemoveListener(
                            HandleDisplayReverse);
                    break;
                default:
                    Debug.LogError("自动墙类型错误");
                    break;
            }
        }

        private void HandleDisplay(int leftStep)
        {
            int costStep = ModelsManager.Instance.PlayerMod.maxMoveStep -
                           leftStep;
            HandleType wwallType = (HandleType)(costStep % 4);

            switch (wwallType)
            {
                case HandleType.A:
                case HandleType.B:
                    HandleSprite(true);
                    _boxCollider2D.enabled = true;
                    break;
                case HandleType.C:
                case HandleType.D:
                    HandleSprite(false);
                    _boxCollider2D.enabled = false;
                    break;
                case HandleType.None:
                    break;
                default:
                    Debug.LogError("处理沙漠自动墙类型错误");
                    break;
            }
        }

        private void HandleDisplayReverse(int leftStep)
        {
            int costStep = ModelsManager.Instance.PlayerMod.maxMoveStep -
                           leftStep;
            HandleType wwallType = (HandleType)(costStep % 4);

            switch (wwallType)
            {
                case HandleType.C:
                case HandleType.D:
                    HandleSprite(true);
                    _boxCollider2D.enabled = true;
                    break;
                case HandleType.A:
                case HandleType.B:
                    HandleSprite(false);
                    _boxCollider2D.enabled = false;
                    break;
                case HandleType.None:
                    break;
                default:
                    Debug.LogError("处理沙漠自动墙类型错误");
                    break;
            }
        }

        private void HandleSprite(bool show)
        {
            _hideTween?.Complete();
            if (show)
            {
                _hideTween = _spriteTrans.DOMove(_startPos,
                    ModelsManager.Instance.PlayerMod.moveTime);
            }
            else
            {
                Vector3 targetPos = _startPos;
                targetPos.y -= 0.5f;
                _hideTween = _spriteTrans.DOMove(targetPos,
                    ModelsManager.Instance.PlayerMod.moveTime);
            }
        }
    }
}
