using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static event Action<CardAbility> CardPicked;

    [SerializeField] private AbilityCard[] _cards;

    [Header("Tweens")]
    [SerializeField] private float _hoverHeight = 100;
    [SerializeField] private float _hoverSpeed = 2;

    internal void ShowCards()
    {
        foreach (AbilityCard card in _cards)
        {
            // Start listening to the actions of the card
            card.CardAction += OnCardAction; // TODO: Wwait until the cards are inplace before listening to the actions
        }
    }

    private void OnCardAction(CardActions action, AbilityCard card)
    {

        Debug.Log("OnCardACtion " + action + " | " + card);
        switch (action)
        {
            case CardActions.HoverEnter:
                StartCardHover(card);
                break;
            case CardActions.HoverExit:
                StopCardHover(card);
                break;
            case CardActions.Click:
                PickCard(card);
                break;
            default:
                break;
        }
    }

    public void StartCardHover(AbilityCard card)
    {
        card.elements.DOAnchorPos(Vector2.up * _hoverHeight, _hoverSpeed).SetLoops(2, LoopType.Yoyo).SetSpeedBased().SetEase(Ease.InOutSine);
    }

    public void StopCardHover(AbilityCard card)
    {
        // Kill the ongoing hoover
        DOTween.Kill(card.elements);
        card.elements.DOAnchorPos(Vector2.zero, _hoverSpeed).SetSpeedBased().SetEase(Ease.InOutSine);
    }

    public void PickCard(AbilityCard card)
    {
        
    }

    public void HideCards()
    {
        foreach (AbilityCard card in _cards)
        {
            // Stop listening to the actions of the card
            card.CardAction -= OnCardAction;
        }
    }
}
