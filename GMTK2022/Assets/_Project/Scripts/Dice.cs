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
        for (int i = 0; i < dices.Length; i++)
        {
            dices[i].SetActive(false);
        }

        dices[(int)type].SetActive(true);
    }

    public void SetRandomDice()
    {
        List<DiceType> diceTypes = Enum.GetValues(typeof(DiceType)).Cast<DiceType>().ToList();

        SetDice(diceTypes[UnityEngine.Random.Range(0, diceTypes.Count)]);
    }
}
