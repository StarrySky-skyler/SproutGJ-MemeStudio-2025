using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Button))]
public class DoTweenPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header("放大倍数")]
    public float mutiple = 1.2f;
    [Header("动画时间")]
    public float dotimer = 0.2f;
    [Header("进入风格")]
    public Ease Enter_style=Ease.InOutQuad;
    [Header("退出风格")]
    public Ease Exit_style = Ease.InOutQuad;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * mutiple, dotimer).SetEase(Enter_style);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, dotimer).SetEase(Exit_style);
    }
}
