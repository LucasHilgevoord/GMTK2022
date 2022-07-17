using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CardActions
{
    HoverEnter,
    HoverExit,
    Click
}

public class AbilityCard : MonoBehaviour
{
    public event Action<CardActions, AbilityCard> CardAction;

    public RectTransform rect;
    public RectTransform elements;
    public Quaternion startRot;

    public CardAbility Ability => _ability;
    [SerializeField] private CardAbility _ability;

    private void Start()
    {
        startRot = rect.rotation;
    }

    public void OnHoverEnter()
    {
        CardAction?.Invoke(CardActions.HoverEnter, this);
    }

    public void OnHoverExit()
    {
        CardAction?.Invoke(CardActions.HoverExit, this);
    }

    public void OnClick()
    {
        CardAction?.Invoke(CardActions.Click, this);
    }
}
