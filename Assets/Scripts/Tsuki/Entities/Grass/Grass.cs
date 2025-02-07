// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 18:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using UnityEngine;

namespace Tsuki.Entities.Grass
{
    public class Grass : MonoBehaviour
    {
        private static readonly int Break = Animator.StringToHash("Break");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Weeders"))
            {
                _animator.SetTrigger(Break);
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
