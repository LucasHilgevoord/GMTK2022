using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceManager _diceManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Character _player;
    private BattlePhase _currentPhase;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentPhase = BattlePhase.PlayerTurn;
        NextPhase(false);
    }

    private void NextPhase(bool change = true)
    {
        if (change)
            _currentPhase = _currentPhase.Next();

        Debug.Log("Next Phase " + _currentPhase);
            
        switch (_currentPhase)
        {
            default:
            case BattlePhase.PlayerTurn:
                OnPlayerTurn();
                break;
            case BattlePhase.EnemyTurn:
                OnEnemyTurn();
                break;
        }
    }
    
    private void OnPlayerTurn()
    {
        // Wait for dice to be rolled
        DiceManager.DiceRolled += OnDiceRolled;
    }

    private void OnDiceRolled(object value)
    {
        DiceManager.DiceRolled -= OnDiceRolled;
        Debug.Log("Dice rolled");

        ApplyAbility(null);
    }

    private void ApplyAbility(object ability)
    {
        Debug.Log("Apply ability");
        Character target = _currentPhase == BattlePhase.PlayerTurn ? /*_enemyManager.FocussedEnemy*/ null : _player;
        // Apply ability

        NextPhase();
    }

    private void OnEnemyTurn()
    {
        DiceManager.DiceRolled += OnDiceRolled;
        _diceManager.RollDice();
    }
}
