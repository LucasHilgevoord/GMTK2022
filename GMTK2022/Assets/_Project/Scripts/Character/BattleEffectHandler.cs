using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class BattleEffectHandler : MonoBehaviour
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    [Header("Config")]
    public TextMeshProUGUI battleEffectText;
    public float animationDistance;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowBattleEffect(CharacterInteractionType interactionType, int effectAmount, int leftoverHealthDamage = 0)
    {
        switch (interactionType)
        {
            case CharacterInteractionType.healSelf:
                battleEffectText.color = Color.green;
                battleEffectText.SetText("+" + effectAmount + " HP");
                BattleEffectTextAnim(true);
                break;
            case CharacterInteractionType.addShield:
                battleEffectText.color = Color.blue;
                battleEffectText.SetText("+" + effectAmount + " DEF");
                BattleEffectTextAnim(true);
                break;
            case CharacterInteractionType.takeHealthDamage:
                battleEffectText.color = Color.red;
                battleEffectText.SetText("-" + effectAmount + " HP");
                BattleEffectTextAnim(false);
                break;
            case CharacterInteractionType.takeShieldDamage:
                battleEffectText.color = Color.red;
                battleEffectText.SetText("-" + effectAmount + " DEF");
                BattleEffectTextAnim(false);
                break;
            case CharacterInteractionType.takeMixedDamage:
                battleEffectText.color = Color.magenta;
                battleEffectText.SetText("-" + (effectAmount - leftoverHealthDamage) + " DEF\n" + "-" + leftoverHealthDamage + " HP");
                BattleEffectTextAnim(false);
                break;
        }
    }

    /// <summary>
    /// Animate the box either up or down based on the interaction's positivity on the character
    /// </summary>
    /// <param name="positiveEffect"></param>
    private void BattleEffectTextAnim(bool positiveEffect)
    {
        float originalPosY = rect.localPosition.y;

        Sequence battleEffectBoxSeq = DOTween.Sequence();

        battleEffectBoxSeq.Append(canvasGroup.DOFade(1.0f, 0.2f));

        if (positiveEffect)
            battleEffectBoxSeq.Append(rect.DOAnchorPosY(rect.localPosition.y + animationDistance, 1f));
        else
            battleEffectBoxSeq.Append(rect.DOAnchorPosY(rect.localPosition.y - animationDistance, 1f));

        battleEffectBoxSeq.Append(canvasGroup.DOFade(0.0f, 0.2f)).OnComplete(() =>
            rect.DOAnchorPosY(originalPosY, 0f)
        );
    }
}
