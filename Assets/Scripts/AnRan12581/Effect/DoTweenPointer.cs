using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class DoTweenPointer : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("�Ŵ���")] public float mutiple = 1.2f;
    [Header("����ʱ��")] public float dotimer = 0.2f;
    [Header("������")] public Ease Enter_style = Ease.InOutQuad;
    [Header("�˳����")] public Ease Exit_style = Ease.InOutQuad;

    public float textDuration;

    private Text _text;
    private string _textColoredStr;
    private string _originStr;

    private void Start()
    {
        _text = transform.Find("Text").GetComponent<Text>();
        _textColoredStr = _text.text;
        _originStr = _text.text;
        string temp = _textColoredStr.Aggregate("",
            (current, c) => current + ("<color=#FFFFF>" + c + "</color>"));
        _textColoredStr = temp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * mutiple, dotimer).SetEase(Enter_style);
        _text.DOText(_textColoredStr, textDuration, true).OnComplete(() =>
        {
            _text.DOText(_originStr, textDuration);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, dotimer).SetEase(Exit_style);
    }
}
