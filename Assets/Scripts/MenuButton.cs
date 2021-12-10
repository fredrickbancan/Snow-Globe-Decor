using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    [SerializeField] public GameSound hoverSound;
    [SerializeField] public GameSound pressSound;

    /// <summary>
    /// When user mouses over button, play sound
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound2D(hoverSound);
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetEase(Ease.OutCirc);
    }

    /// <summary>
    /// When user clicks button, play sound
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound2D(pressSound);
        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f).SetEase(Ease.OutCirc);
    }
}
