using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public enum CharacterAnimType
{ 
    attack,
    heal,
    defend
}

public abstract class Character : MonoBehaviour
{
    public RectTransform _characterSprite;

    protected int maxHealth = 100;
    protected string characterName;

    private float currentHealth;
    private int shield = 100;
    //private Dice[] dices;

    [Header("UI-Config")]
    public Image healthBar;
    public Image shieldBar;

    #region virtual-methods
    public virtual void Damage(int damage)
    {
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

        AnimateCharacter(CharacterAnimType.attack);
        UpdateCharacterUI();
    }
    public virtual void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        AnimateCharacter(CharacterAnimType.heal);
        UpdateCharacterUI();
    }
    public virtual void AddShield(int shieldAmount)
    {
        shield += shieldAmount;

        if (shieldAmount > maxHealth)
            shield = maxHealth;

        AnimateCharacter(CharacterAnimType.defend);
        UpdateCharacterUI();
    }
    #endregion

    #region abstract-methods
    public abstract void DamageAnimation();
    public abstract void HealAnimation();
    public abstract void ShieldAnimation();
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
        }
    }
    private void UpdateCharacterUI()
    {
        if (currentHealth > 0)
            healthBar.DOFillAmount(currentHealth / maxHealth, 0.2f);
        
        if (shield > 0)
            shieldBar.DOFillAmount(shield / maxHealth, 0.2f);
    }
}
