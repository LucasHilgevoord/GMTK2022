using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public DiceType type;

    public GameObject[] dices;
    internal Rigidbody myRigidbody;
    private int _currentDiceIndex;

    private void Awake()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Sets the dice to what we need
    /// </summary>
    /// <param name="type"></param>
    public void SetDice(DiceType type)
    {
        // Disable previous dice
        HideDice();

        // Show new dice
        _currentDiceIndex = (int)type;
        dices[_currentDiceIndex].SetActive(true);
    }

    public void SetRandomDice()
    {
        List<DiceType> diceTypes = Enum.GetValues(typeof(DiceType)).Cast<DiceType>().ToList();

        SetDice(diceTypes[UnityEngine.Random.Range(0, diceTypes.Count)]);
    }

    public void HideDice()
    {
        dices[_currentDiceIndex].SetActive(false);
    }
}
