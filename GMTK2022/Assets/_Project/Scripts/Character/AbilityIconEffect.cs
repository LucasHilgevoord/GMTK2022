using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AbilityIconEffect : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private float _showDuration = 1;
    [SerializeField] private float _tweenHeight = 70;
    [SerializeField] private Sprite[] _iconSprites;

    internal void ShowIcon(CardAbility ability)
    {
        _icon.gameObject.SetActive(true);
        Color c = _icon.color;
        c.a = 0;

        _icon.color = c;
        _icon.rectTransform.anchoredPosition = new Vector2(0, -_tweenHeight);
        _icon.sprite = _iconSprites[(int)ability];

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_icon.rectTransform.DOAnchorPosY(_tweenHeight, _showDuration).SetEase(Ease.InOutSine));
        sequence.Join(_icon.DOFade(1, _showDuration / 2).SetLoops(2, LoopType.Yoyo));
        sequence.Play().OnComplete(() => { _icon.gameObject.SetActive(false); });
    }
}
