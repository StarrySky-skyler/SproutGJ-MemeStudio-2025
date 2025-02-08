// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/06 19:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Entities.Audio
{
    public class AudioEntity : MonoBehaviour
    {
        private AudioSource[] _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponents<AudioSource>();
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                _audioSource[0].loop = true;
                _audioSource[0].Play();
            }
        }
    }
}
