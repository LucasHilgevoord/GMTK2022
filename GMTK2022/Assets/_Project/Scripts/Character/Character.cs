using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public virtual void Heal(float healAmount)
    {
        if ((currentHealth + healAmount) <= maxHealth)
            currentHealth += healAmount;
        else
            currentHealth = maxHealth;
    } 
}
