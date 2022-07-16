using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

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

    private void Start()
    {
        currentHealth = maxHealth;
        shield = maxHealth;
    }

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
        shield = Mathf.Clamp(currentHealth + healAmount, 0, healAmount) ;
        AnimateCharacter(CharacterAnimType.heal);
        UpdateCharacterUI();
    }
    public virtual void AddShield(int shieldAmount)
    {
        shield = Mathf.Clamp(shield + shieldAmount, 0, maxShield);
        AnimateCharacter(CharacterAnimType.defend);
        UpdateCharacterUI();
    }
    public virtual void Die()
    {
        AnimateCharacter(CharacterAnimType.die);
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
