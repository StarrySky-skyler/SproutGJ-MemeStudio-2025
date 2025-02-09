// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/09 19:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using Tsuki.Base;
using UnityEngine;

namespace Tsuki
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 _offset;
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _offset = transform.position - _player.transform.position;
        }

        private void LateUpdate()
        {
            transform.position =
                Commons.GetModifiedPos(_player.transform.position + _offset);
        }
    }
}
