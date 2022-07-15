using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public void RollDice()
    {
        int diceValue = UnityEngine.Random.Range(1, 7);
        DiceAbility rndAbility = (DiceAbility)Enum.GetNames(typeof(DiceAbility)).Length;
        DiceResult result = new DiceResult(rndAbility, diceValue);
    }
}
