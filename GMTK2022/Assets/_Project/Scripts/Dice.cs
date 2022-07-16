using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public DiceType type;
    internal Rigidbody myRigidbody;
    public DiceSide[] sides;

    private void Awake()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
    }

    internal void SetSide(int diceSideNumber, int index)
    {
        sides[index].diceSideNumber = diceSideNumber;
        sides[index].diceSideText.SetText(diceSideNumber.ToString());
    }
}
