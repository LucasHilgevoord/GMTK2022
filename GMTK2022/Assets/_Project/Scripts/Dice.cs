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

    internal void SetSide(Sprite sprite, CardAbility type, int index)
    {
        sides[index].type = type;
        sides[index].spriteRenderer.sprite = sprite;
    }
}
