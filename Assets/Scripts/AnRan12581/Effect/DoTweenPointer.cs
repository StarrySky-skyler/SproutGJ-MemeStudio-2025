using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DoTweenPointer : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("放大倍数")] public float mutiple = 1.2f;
    [Header("整体动画时间")] public float dotimer = 0.1f;
    [Header("进入风格")] public Ease Enter_style = Ease.InOutQuad;
    [Header("退出风格")] public Ease Exit_style = Ease.InOutQuad;

    [SerializeField] [ReadOnly] private Text _text;

    [SerializeField] [ReadOnly] private string _textColoredStr;

    [SerializeField] [ReadOnly] private string _originStr;

    private void Start()
    {
        if (!transform.Find("Text")) return;

        if (transform.Find("Text").TryGetComponent(out Text txt))
        {
            _text = txt;
            _textColoredStr = _text.text;
            _originStr = _text.text;
            _textColoredStr = $"<color=#FFFFF>{_originStr}</color>";
        }
        else
        {
            _text = null;
        }
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * mutiple, dotimer).SetEase(Enter_style);

        if (_text == null) return;

        _text.DOText(_textColoredStr, 0.1f).OnComplete(() =>
        {
            _text.DOText(_originStr, 0.1f);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, dotimer).SetEase(Exit_style);
    }
}
