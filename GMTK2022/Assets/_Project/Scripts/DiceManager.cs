using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public DiceType type;

    public Dice[] dices;

    private int _currentDiceIndex;

    private Dice currentDice;

    /// <summary>
    /// Sets the dice to what we need
    /// </summary>
    /// <param name="type"></param>
    public Dice SetDice(DiceType type)
    {
        // Disable previous dice
        HideDice();

        // Show new dice
        _currentDiceIndex = (int)type;
        dices[_currentDiceIndex].gameObject.SetActive(true);

        return dices[_currentDiceIndex];
    }

    public Dice SetRandomDice()
    {
        List<DiceType> diceTypes = Enum.GetValues(typeof(DiceType)).Cast<DiceType>().ToList();
        return SetDice(diceTypes[UnityEngine.Random.Range(0, diceTypes.Count)]);
    }

    public void HideDice()
    {
        dices[_currentDiceIndex].gameObject.SetActive(false);
    }

    public Dice SetDice(DiceType type, DiceAbility[] abilities)
    {
        int diceSideAmount = 0;

        switch (type)
        {
            case DiceType.D4:
                diceSideAmount = 4;
                break;
            case DiceType.D6:
                diceSideAmount = 6;
                break;
            case DiceType.D8:
                diceSideAmount = 8;
                break;
            case DiceType.D10:
                diceSideAmount = 10;
                break;
            case DiceType.D12:
                diceSideAmount = 12;
                break;
            case DiceType.D20:
                diceSideAmount = 20;
                break;
        }

        // Set the dice
        currentDice = SetDice(type);

        // Set all the sides
        SetSides(diceSideAmount);

        return currentDice;
    }

    public Dice SetDice(DiceType type, int attacks, int defends, int heals)
    {
        List<DiceAbility> diceSet = new List<DiceAbility>();

        for (int i = 0; i < attacks; i++)
        {
            diceSet.Add(DiceAbility.Attack);
        }

        for (int i = 0; i < defends; i++)
        {
            diceSet.Add(DiceAbility.Defend);
        }

        for (int i = 0; i < heals; i++)
        {
            diceSet.Add(DiceAbility.Heal);
        }

        return SetDice(type, diceSet.ToArray());
    }

    private void SetSides(int diceSideAmount)
    {
        for (int i = 0; i < diceSideAmount; i++)
        {
            currentDice.SetSide(i+1, i);
        }
    }
}
