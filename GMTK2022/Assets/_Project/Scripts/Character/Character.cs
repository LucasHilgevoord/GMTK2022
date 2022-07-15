using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAnimType
{ 
    damage,
    heal,
    shield
}

public abstract class Character : MonoBehaviour
{
    [Header("Character Config")]
    public float maxHealth;

    private float currentHealth;
    private float shield;
    //private Dice[] dices;

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
    }

    public virtual void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        AnimateCharacter(CharacterAnimType.heal);
    }

    public virtual void AddShield(float shieldAmount)
    {
        shield += shieldAmount;

        if (shieldAmount > maxHealth)
            shield = maxHealth;

        AnimateCharacter(CharacterAnimType.shield);
    }

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
    public abstract void DamageAnimation();
    public abstract void HealAnimation();
    public abstract void ShieldAnimation();
}
