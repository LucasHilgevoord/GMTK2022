using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnData
{
    public Character character;
    public CardAbility ability;
    public int diceValue;

    public TurnData(Character character) { this.character = character; }
    public void SetAbility(CardAbility ability) { this.ability = ability; }
    public void SetDiceValue(int diceValue) { this.diceValue = diceValue; }
}
