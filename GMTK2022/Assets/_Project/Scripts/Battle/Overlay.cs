using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    private Image overlay;

    public void Awake()
    {
        overlay = GetComponent<Image>();
    }

    public void ShowOverlay(bool show, Action OnComplete = null)
    {
        // Start with the correct color
        Color c = overlay.color;
        c.a = show ? 0 : 1;
        overlay.color = c;

        if (show)
            overlay.DOFade(1.0f, 0.5f).OnComplete(() => OnComplete?.Invoke());
        else
            overlay.DOFade(0.0f, 0.5f).OnComplete(() => OnComplete?.Invoke());
    }
}
