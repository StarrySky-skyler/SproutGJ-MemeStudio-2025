// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/09 22:02
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Tsuki
{
    public class WordPos : MonoBehaviour
    {
        public GameObject word;
        private string _text;
        private TextMeshProUGUI _textMeshProUGUI;

        private void Start()
        {
            _textMeshProUGUI = word.GetComponent<TextMeshProUGUI>();
            _text = _textMeshProUGUI.text;
            _textMeshProUGUI.text = "";
            word.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            word.SetActive(true);
            DOTween.To(() => _textMeshProUGUI.text,
                    x => _textMeshProUGUI.text = x, _text, _text.Length * 0.1f)
                .SetEase(Ease.Linear);
        }
    }
}
