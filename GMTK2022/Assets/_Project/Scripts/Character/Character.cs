using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public enum CharacterAnimType
{ 
    attack,
    heal,
    defend,
    die
}

public abstract class Character : MonoBehaviour
{
    public RectTransform _characterSprite;

    protected int maxHealth = 100;
    protected int maxShield => maxHealth;
    protected string characterName;

    private int currentHealth;
    private int shield;
    //private Dice[] dices;

    [Header("UI-Config")]
    public Image healthBar;
    public Image shieldBar;
    public CanvasGroup dialogueBox;
    public TextMeshProUGUI dialogueText;

    private void Start()
    {
        currentHealth = maxHealth;
        shield = maxHealth;
    }

    #region virtual-methods
    public virtual void Damage(int damage)
    {
        SayDialogue("OUCH! You think you're sh*t ?!");

        if (shield > 0)
        {
            if (shield >= damage)
            {
                shield -= damage;
                Debug.Log("shield " + shield);
            }
            else
            {
                int leftoverDamage = damage - shield;
                currentHealth -= leftoverDamage;
                Debug.Log("shield + health " + shield + " | " + currentHealth);
                shield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
            Debug.Log("health " + currentHealth);
        }

        // Clamp the values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        shield = Mathf.Clamp(shield, 0, maxShield);

        AnimateCharacter(CharacterAnimType.attack);
        UpdateCharacterUI();

        if (currentHealth <= 0)
            Die();
    }
    public virtual void Heal(int healAmount)
    {
        SayDialogue("Ahhh, feels nice.");

        shield = Mathf.Clamp(currentHealth + healAmount, 0, healAmount) ;
        AnimateCharacter(CharacterAnimType.heal);
        UpdateCharacterUI();
    }
    public virtual void AddShield(int shieldAmount)
    {
        SayDialogue("HAHA! SHIELD POWERED UP!");

        shield = Mathf.Clamp(shield + shieldAmount, 0, maxShield);
        AnimateCharacter(CharacterAnimType.defend);
        UpdateCharacterUI();
    }
    public virtual void Die()
    {
        AnimateCharacter(CharacterAnimType.die);
        SayDialogue("Damn I'm dead.");
    }

    public virtual void SayDialogue(string dialogue)
    {
        dialogueText.SetText(dialogue);

        Sequence dialoguePopup = DOTween.Sequence();
        dialoguePopup.Append(dialogueBox.DOFade(1.0f, 0.2f));
        dialoguePopup.AppendInterval(1.5f);
        dialoguePopup.Append(dialogueBox.DOFade(0.0f, 0.2f));
    }
    #endregion

    #region abstract-methods
    public abstract void DamageAnimation();
    public abstract void HealAnimation();
    public abstract void ShieldAnimation();
    public abstract void DieAnimation();
    #endregion

    private void AnimateCharacter(CharacterAnimType animType)
    {
        switch (animType)
        {
            case CharacterAnimType.attack:
                DamageAnimation();
                break;
            case CharacterAnimType.heal:
                HealAnimation();
                break;
            case CharacterAnimType.defend:
                ShieldAnimation();
                break;
            case CharacterAnimType.die:
                DieAnimation();
                break;
        }
    }
    private void UpdateCharacterUI()
    {
        if (currentHealth >= 0)
            healthBar.DOFillAmount((float)currentHealth / maxHealth, 0.2f);
        
        if (shield >= 0)
            shieldBar.DOFillAmount((float)shield / maxShield, 0.2f);
    }
}
