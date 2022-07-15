using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceBox _diceBox;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnAttack(_player, _enemyManager.FocussedEnemy);
        }
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
        DiceBox.DiceRolled += OnDiceRolled;
    }

    private void OnDiceRolled(DiceResult result)
    {
        DiceBox.DiceRolled -= OnDiceRolled;
        Debug.Log("Dice rolled");

        ApplyAbility(result);
    }

    private void ApplyAbility(DiceResult result)
    {
        Debug.Log("Apply ability");
        Character caster = _currentPhase == BattlePhase.PlayerTurn ? _player : _enemyManager.FocussedEnemy;
        Character target = _currentPhase == BattlePhase.PlayerTurn ? _enemyManager.FocussedEnemy : _player;
        // Apply ability

        Debug.Log(result.diceAbility);

        switch (result.diceAbility)
        {
            default:
            case DiceAbility.Attack:
                OnAttack(caster, target);
                break;
            case DiceAbility.Defend:
                OnDefend();
                break;
            case DiceAbility.Heal:
                break;
        }
    }

    private void OnAttack(Character caster, Character target)
    {
        Debug.Log("Attack");
        // Apply attack
        RectTransform c = caster._characterSprite;
        RectTransform t = target._characterSprite;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(c.DOAnchorPosX(c.anchoredPosition.x - 100, 0.2f)); // Caster moves backwards
        sequence.Append(c.DORotate(Vector3.forward * 6, 0.1f));
        sequence.Append(c.DOAnchorPosX(c.anchoredPosition.x + 200, 0.1f)); // Caster charges
        sequence.Join(c.DORotate(Vector3.zero, 0.2f));
        sequence.Append(t.DOAnchorPosX(t.anchoredPosition.x + 100, 0.2f)); // Target gets hit
        sequence.Join(t.DORotate(Vector3.forward * -6, 0.1f));
        sequence.Append(c.DOAnchorPosX(0, 0.3f)); // Caster goes back
        sequence.Join(t.DOAnchorPosX(0, 0.2f)); // Target goes back
        sequence.Join(t.DORotate(Vector3.zero, 0.2f));
        sequence.OnComplete(OnAbilityFinished);
        sequence.Play();
        // Move caster backwards
        // Rotate caster a bit away from the target
        // Charge the caster into the target
        // Slowly Rotate caster back to oritinal rotation while charging
        // Move target backwards
        // Rotate target a bit away from the caster
        // Move the caster and target back to original positions
        
        
    }

    private void OnDefend()
    {
        
    }

    private void OnHeal()
    {
        
    }

    private void OnAbilityFinished()
    {
        //NextPhase();
    }

    private void OnEnemyTurn()
    {
        DiceBox.DiceRolled += OnDiceRolled;
        _diceBox.RollCompleted();
    }
}
