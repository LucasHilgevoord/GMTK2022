using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static event System.Action<object> DiceRolled;

    public void RollDice()
    {
        int diceValue = Random.Range(1, 7);
        DiceRolled.Invoke(diceValue);
    }
}
