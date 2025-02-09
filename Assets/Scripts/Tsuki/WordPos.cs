// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/09 22:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using UnityEngine;

namespace Tsuki
{
    public class WordPos : MonoBehaviour
    {
        public GameObject word;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            word.SetActive(true);
        }
    }
}
