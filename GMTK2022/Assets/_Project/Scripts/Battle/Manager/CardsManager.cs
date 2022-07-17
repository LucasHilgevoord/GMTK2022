using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static event Action<CardAbility> CardPicked;
    [SerializeField] private AbilityCard[] _cards;
    [SerializeField] private RectTransform _cardParent;

    private AbilityCard _focussedCard;

    [Header("Tweens")]
    [SerializeField] private float _showCardsDuration = 0.5f;
    [SerializeField] private float _showCardsDelay = 0.5f;
    [SerializeField] private float _hoverHeight = 200;
    [SerializeField] private float _focusDuration = 0.2f;
    [SerializeField] private float _focusReturnDuration = 50;

    private bool _enableActions;

    private void Start()
    {
        // So we can hide it in the inspector while in edit mode
        _cardParent.gameObject.SetActive(true);
        
        HideCards(true);
    }

    internal void ShowCards()
    {
        int index = 0;
        _enableActions = false;
        foreach (AbilityCard card in _cards)
        {
            int i = index;
            
            // Start listening to the actions of the card
            card.CardAction += OnCardAction; // TODO: Wwait until the cards are inplace before listening to the actions
            card.elements.DORotateQuaternion(card.startRot, _showCardsDuration).SetEase(Ease.InOutSine);
            card.elements.DOAnchorPos(Vector2.zero, _showCardsDuration)
                .SetEase(Ease.InOutSine)
                .SetDelay(_showCardsDelay * i)
                .OnComplete(() =>
                {
                    if (i == _cards.Length - 1)
                        _enableActions = true;
                });
            
            index++;
        }
    }
    
    private void OnCardAction(CardActions action, AbilityCard card)
    {
        // Not allowing actions
        if (!_enableActions) { return; }
        _focussedCard = card;

        switch (action)
        {
            case CardActions.HoverEnter:
                StartCardHover();
                break;
            case CardActions.HoverExit:
                StopCardHover();
                break;
            case CardActions.Click:
                PickCard();
                break;
            default:
                break;
        }
    }

    public void StartCardHover()
    {
        _focussedCard.elements.DOAnchorPos(Vector2.up * _hoverHeight, _focusDuration).SetEase(Ease.InOutSine);
    }

    public void StopCardHover()
    {
        // Kill the ongoing hoover
        DOTween.Kill(_focussedCard.elements);
        _focussedCard.elements.DOAnchorPos(Vector2.zero, _focusReturnDuration).SetEase(Ease.InOutSine);
    }

    public void PickCard()
    {
        CardPicked?.Invoke(_focussedCard.Ability);
        HideCards();
    }

    public void HideCards(bool snap = false)
    {
        _enableActions = false;
        
        if (_focussedCard != null)
            DOTween.Kill(_focussedCard.elements);

        foreach (AbilityCard card in _cards)
        {
            // Stop listening to the actions of the card
            card.CardAction -= OnCardAction;

            // Go back to the starting pos
            // TODO: Set to to the same position, anchor positions are annoying
            Vector2 startPos = new Vector2(-card.rect.anchoredPosition.x, -_cardParent.rect.height);

            if (snap)
            {
                card.elements.anchoredPosition = startPos;
                card.elements.rotation = card.startRot;
            }
            else
            {
                card.elements.DOAnchorPos(startPos, _showCardsDuration).SetEase(Ease.InOutSine);
                card.elements.DORotateQuaternion(Quaternion.identity, _showCardsDuration).SetEase(Ease.InOutSine);
            }
        }
    }
}
