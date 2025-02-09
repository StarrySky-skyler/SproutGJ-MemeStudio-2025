// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/06 19:02
// @version: 1.0
// @description:
// *****************************************************************************

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tsuki.Entities.Audio
{
    public class AudioEntity : MonoBehaviour
    {
        private AudioSource[] _audioSource;
        private AudioClip _sfxClick;

        private void Start()
        {
            _sfxClick = Resources.Load<AudioClip>("Music/Sfx/Got Hurt");
            if (!_sfxClick) Debug.LogWarning("AudioEntity >>> 鼠标点击音效为空，加载失败");

            _audioSource = GetComponents<AudioSource>();
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                _audioSource[0].loop = true;
                _audioSource[0].Play();
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame) PlaySfxClick();
        }

        /// <summary>
        /// 播放鼠标点击音效
        /// </summary>
        private void PlaySfxClick()
        {
            if (!_sfxClick) return;
            _audioSource[2].Stop();
            _audioSource[2].PlayOneShot(_sfxClick);
        }
    }
}
