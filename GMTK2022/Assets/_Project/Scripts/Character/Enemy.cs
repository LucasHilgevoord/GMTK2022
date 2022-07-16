using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public void LoadFromBaseData(EnemyBaseData baseData)
    {
        this.characterName = baseData.GetEnemyName();
        this.maxHealth = baseData.GetMaxHealth();
    }

    public override void DamageAnimation()
    {
    }

    public override void HealAnimation()
    {
    }

    public override void ShieldAnimation()
    {
    }

    public override void DieAnimation()
    {
    }
}
