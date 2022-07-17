using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public DiceType type;

    public Dice[] playerDices;
    public Dice[] enemyDices;

    private int _currentPlayerDiceIndex;
    private int _currentEnemyDiceIndex;

    private Dice[] currentDices;

    /// <summary>
    /// Sets the dice to what we need
    /// </summary>
    /// <param name="type"></param>
    public Dice[] SetDices(DiceType playerDiceType, DiceType enemyDiceType)
    {
        // Disable previous dice
        HideDices();

        // Show new dices
        _currentPlayerDiceIndex = (int)playerDiceType;
        playerDices[_currentPlayerDiceIndex].gameObject.SetActive(true);

        _currentEnemyDiceIndex = (int)enemyDiceType;
        enemyDices[_currentEnemyDiceIndex].gameObject.SetActive(true);

        return new Dice[] { playerDices[_currentPlayerDiceIndex], enemyDices[_currentEnemyDiceIndex] };
    }

    public void HideDices()
    {
        playerDices[_currentPlayerDiceIndex].gameObject.SetActive(false);
        enemyDices[_currentEnemyDiceIndex].gameObject.SetActive(false);
    }

    public Dice[] SetDice(DiceType playerDiceType, DiceType enemyDiceType)
    {
        int playerDiceSideAmount = 0;
        int enemyDiceSideAmount = 0;

        switch (playerDiceType)
        {
            case DiceType.D4:
                playerDiceSideAmount = 4;
                break;
            case DiceType.D6:
                playerDiceSideAmount = 6;
                break;
            case DiceType.D8:
                playerDiceSideAmount = 8;
                break;
            case DiceType.D10:
                playerDiceSideAmount = 10;
                break;
            case DiceType.D12:
                playerDiceSideAmount = 12;
                break;
            case DiceType.D20:
                playerDiceSideAmount = 20;
                break;
        }

        switch (enemyDiceType)
        {
            case DiceType.D4:
                enemyDiceSideAmount = 4;
                break;
            case DiceType.D6:
                enemyDiceSideAmount = 6;
                break;
            case DiceType.D8:
                enemyDiceSideAmount = 8;
                break;
            case DiceType.D10:
                enemyDiceSideAmount = 10;
                break;
            case DiceType.D12:
                enemyDiceSideAmount = 12;
                break;
            case DiceType.D20:
                enemyDiceSideAmount = 20;
                break;
        }

        // Set the dice
        currentDices = SetDices(playerDiceType, enemyDiceType);

        // Set all the sides
        SetSides(playerDiceSideAmount, enemyDiceSideAmount);

        return currentDices;
    }

    private void SetSides(int playerDiceSideAmount, int enemyDiceSideAmount)
    {
        for (int i = 0; i < playerDiceSideAmount; i++)
        {
            currentDices[0].SetSide(i+1, i);
        }

        for (int i = 0; i < enemyDiceSideAmount; i++)
        {
            currentDices[1].SetSide(i + 1, i);
        }
    }
}
