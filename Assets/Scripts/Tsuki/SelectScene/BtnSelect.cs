// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/09 12:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Entities.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Tsuki.SelectScene
{
    public class BtnSelect : MonoBehaviour
    {
        private AudioEntity _audioEntity;
        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void Start()
        {
            _audioEntity = GameObject.FindWithTag("Audio")
                .GetComponent<AudioEntity>();
        }

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _audioEntity.PlaySfx("Talk_U1");
        }
    }
}
