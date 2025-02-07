// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/29 23:01
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.Types;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.Box
{
    public class CorrectPos : MonoBehaviour
    {
        public BoxType boxType;

        private SpriteRenderer _spriteRenderer;
        private Vector3 _originalPos;

        private void Start()
        {
            _originalPos = transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // 死锁位置
            if (boxType == BoxType.None) return;
            if (transform.position != _originalPos)
                transform.position = _originalPos;
        }

        private void LateUpdate()
        {
            // 死锁位置
            if (boxType == BoxType.None) return;
            if (transform.position != _originalPos)
                transform.position = _originalPos;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Box")) return;
            if (other.gameObject.GetComponent<BaseObj>().boxType == boxType)
            {
                _spriteRenderer.enabled = false;
                BoxManager.Instance.AddCorrectBox();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Box")) return;
            if (other.gameObject.GetComponent<BaseObj>().boxType == boxType)
            {
                _spriteRenderer.enabled = true;
                BoxManager.Instance.RemoveCorrectBox();
            }
        }
    }
}
