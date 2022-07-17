using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ProgressionPhase
{
    Fight,
    GameOver,
    Win
}

public class ProgressionText : MonoBehaviour
{
    [SerializeField] private Image _progressionImage;
    [SerializeField] private Sprite[] _progressionSprites;
    [SerializeField] private float _hideDelay = 0.5f;
    [SerializeField] private float _popupSpeed = 0.5f;

    internal void ShowNewPhase(ProgressionPhase phase, Action OnComplete)
    {
        _progressionImage.gameObject.SetActive(true);
        _progressionImage.transform.localScale = Vector3.zero;
        _progressionImage.sprite = _progressionSprites[(int)phase];

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_progressionImage.transform.DOScale(Vector3.one, _popupSpeed).SetEase(Ease.OutBack));
        sequence.AppendInterval(_hideDelay);
        sequence.Append(_progressionImage.transform.DOScale(Vector3.zero, _popupSpeed).SetEase(Ease.InBack).OnComplete(() =>
        {
            _progressionImage.gameObject.SetActive(false);
            OnComplete?.Invoke();
        }));
    }
}
