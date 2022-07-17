using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using DG.Tweening;

public class UI_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;

    public bool animated = true;
    public bool sounds = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(animated)
            rectTransform.DOScale(0.8f, 0.2f);
        if (sounds)
            SoundManager.Instance.Play(Sounds.uiClick);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (animated)
            rectTransform.DOScale(1.0f, 0.2f);
        if (sounds)
            SoundManager.Instance.Play(Sounds.uiClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animated)
            rectTransform.DOScale(0.9f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animated)
            rectTransform.DOScale(1f, 0.2f);
    }
}
