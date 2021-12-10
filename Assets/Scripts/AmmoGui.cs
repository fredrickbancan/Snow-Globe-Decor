using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AmmoGui : MonoBehaviour
{
    [SerializeField] private GameObject selectedOverlay = null;
    [SerializeField] private float scaleUpAmount = 1.1f;
    [SerializeField] private float scaleUpSpeed = 0.5f;
    private RectTransform rectTransform = null;
    private bool selected = false;
    
    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Selected()
    {
        rectTransform.DOScale(new Vector3(scaleUpAmount, scaleUpAmount, 1.0f), scaleUpSpeed).SetEase(Ease.OutCirc);
        selectedOverlay.GetComponent<Image>().DOFade(1.0f, scaleUpSpeed).SetEase(Ease.Linear);
    }

    public void Deselected()
    {
        rectTransform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), scaleUpSpeed).SetEase(Ease.OutCirc);
        selectedOverlay.GetComponent<Image>().DOFade(0.0f, scaleUpSpeed).SetEase(Ease.Linear);
    }
}
