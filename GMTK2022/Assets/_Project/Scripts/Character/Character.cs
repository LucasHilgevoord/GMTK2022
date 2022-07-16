using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public enum CharacterAnimType
{ 
    damage,
    heal,
    shield
}

public abstract class Character : MonoBehaviour
{
    protected float maxHealth;
    protected string characterName;

    private float currentHealth;
    private float shield;
    //private Dice[] dices;

    [Header("UI-Config")]
    public Image healthBar;
    public Image shieldBar;

    #region virtual-methods
    public virtual void Damage(float damage)
    {
        if (shield > 0)
        {
            if (shield >= damage)
                shield -= damage;
            else
            {
                float leftoverDamage = damage - shield;
                currentHealth -= leftoverDamage;
                shield = 0;
            }
        }
        else
            currentHealth -= damage;

        AnimateCharacter(CharacterAnimType.damage);
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
    public virtual void AddShield(float shieldAmount)
    {
        shield += shieldAmount;

        if (shieldAmount > maxHealth)
            shield = maxHealth;

        AnimateCharacter(CharacterAnimType.shield);
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
            case CharacterAnimType.damage:
                DamageAnimation();
                break;
            case CharacterAnimType.heal:
                HealAnimation();
                break;
            case CharacterAnimType.shield:
                ShieldAnimation();
                break;
        }
    }
    private void UpdateCharacterUI()
    {
        healthBar.DOFillAmount(currentHealth / maxHealth, 0.2f);
        shieldBar.DOFillAmount(shield / maxHealth, 0.2f);
    }
}
