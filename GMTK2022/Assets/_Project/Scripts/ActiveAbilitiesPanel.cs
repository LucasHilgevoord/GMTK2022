using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class ActiveAbilitiesPanel : MonoBehaviour
{
    [Header("UI-Config")]
    public RectTransform playerActiveAbilityPanel;
    public RectTransform enemyActiveAbilityPanel;
    public Image playerActiveAbilityIcon;
    public Image enemyActiveAbilityIcon;

    [Header("Ability-Sprites")]
    public Sprite attack;
    public Sprite parry;
    public Sprite heal;

    public Ease animationEase;

    public void SetActiveAbilityIcon(bool player, CardAbility ability)
    {
        switch (ability)
        {
            case CardAbility.Attack:
                if (player) playerActiveAbilityIcon.sprite = attack;
                else enemyActiveAbilityIcon.sprite = attack;
                break;
            case CardAbility.Parry:
                if (player) playerActiveAbilityIcon.sprite = parry;
                else enemyActiveAbilityIcon.sprite = parry;
                break;
            case CardAbility.Heal:
                if (player) playerActiveAbilityIcon.sprite = heal;
                else enemyActiveAbilityIcon.sprite = heal;
                break;
        }
    }

    public void ShowActiveAbility(bool player, bool show)
    {
        if (show)
        { 
            if (player) playerActiveAbilityPanel.DOAnchorPosY(-250, 0.5f).SetEase(animationEase);
            else enemyActiveAbilityPanel.DOAnchorPosY(-250, 0.5f).SetEase(animationEase);
        }
        else
        { 
            if (player) playerActiveAbilityPanel.DOAnchorPosY(0, 0.5f).SetEase(animationEase);
            else enemyActiveAbilityPanel.DOAnchorPosY(0, 0.5f).SetEase(animationEase);
        }
    }


}
