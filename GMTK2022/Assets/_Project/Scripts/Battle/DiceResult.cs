using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResult : MonoBehaviour
{
    public DiceAbility diceAbility;
    public int diceValue;

    public DiceResult(DiceAbility diceAbility, int diceValue)
    {
        this.diceAbility = diceAbility;
        this.diceValue = diceValue;
    }
}
