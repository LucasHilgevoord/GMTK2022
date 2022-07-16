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

    public Sprite[] diceAbilitySprites;

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
        int lengthOfAbilities = 0;

        switch (type)
        {
            case DiceType.D4:
                lengthOfAbilities = 4;
                break;
            case DiceType.D6:
                lengthOfAbilities = 6;
                break;
            case DiceType.D8:
                lengthOfAbilities = 8;
                break;
            case DiceType.D10:
                lengthOfAbilities = 10;
                break;
            case DiceType.D12:
                lengthOfAbilities = 12;
                break;
            case DiceType.D20:
                lengthOfAbilities = 20;
                break;
        }

        if (abilities.Length > lengthOfAbilities || abilities.Length < lengthOfAbilities)
        {
            throw new Exception("Not enough abilities for the dice you want to create! | " + abilities.Length + "  |  " + lengthOfAbilities);
        }

        // Set the dice
        currentDice = SetDice(type);

        // Set all the sides
        SetSides(abilities);

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

    private void SetSides(DiceAbility[] abilities)
    {
        abilities.Shuffle();

        for (int i = 0; i < abilities.Length; i++)
        {
            currentDice.SetSide(diceAbilitySprites[(int)abilities[i]], abilities[i], i);
        }
    }
}
