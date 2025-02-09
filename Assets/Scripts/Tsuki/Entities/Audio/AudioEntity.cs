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
        [HideInInspector] public AudioSource[] audioSource;

        private AudioClip _sfxClick;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _sfxClick = Resources.Load<AudioClip>("Music/Sfx/Got Hurt");
            if (!_sfxClick) Debug.LogWarning("AudioEntity >>> 鼠标点击音效为空，加载失败");

            audioSource = GetComponents<AudioSource>();
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                audioSource[0].loop = true;
                audioSource[0].Play();
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
            audioSource[2].Stop();
            audioSource[2].PlayOneShot(_sfxClick);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name"></param>
        public void PlaySfx(string name)
        {
            if (name == "Load a Save") audioSource[0].Stop();
            AudioClip clip = Resources.Load<AudioClip>($"Music/Sfx/{name}");
            if (!clip)
            {
                Debug.LogWarning($"AudioEntity >>> 音效 {name} 为空，加载失败");
                return;
            }

            audioSource[1].Stop();
            audioSource[1].PlayOneShot(clip);
        }
    }
}
