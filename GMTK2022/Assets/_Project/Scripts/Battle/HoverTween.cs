using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTween : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverSpeed;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        rect.DOAnchorPos(Vector2.up * _hoverHeight, _hoverSpeed).SetLoops(-1, LoopType.Yoyo).SetSpeedBased().SetEase(Ease.InOutSine);
    }
}
