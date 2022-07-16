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

    [Header("Attack")]
    private float _chargeBack = 100;
    private float _chargeFront = 200;
    private float _chargeTargetHit = 100;
    private Vector3 _chargeRotate = new Vector3(0, 0, 6);

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
            OnAttack(_player, _enemyManager.FocussedEnemy, 0);
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
                OnAttack(caster, target, result.diceValue);
                break;
            case DiceAbility.Defend:
                OnDefend();
                break;
            case DiceAbility.Heal:
                break;
        }
    }

    private void OnAttack(Character caster, Character target, int damage)
    {
        // Apply attack
        RectTransform c = caster._characterSprite;
        RectTransform t = target._characterSprite;

        int dir = c.position.x < t.position.x ? 1 : -1;
        Sequence sequence = DOTween.Sequence();
        
        // Caster wind up the charge
        sequence.Append(c.DOAnchorPosX(c.anchoredPosition.x - _chargeBack * dir, 0.2f)); 
        sequence.Append(c.DORotate(_chargeRotate * dir, 0.1f));

        // Caster charges
        sequence.Append(c.DOAnchorPosX(c.anchoredPosition.x + _chargeFront * dir, 0.2f)).OnStart(() =>
        {
            caster.PlayAnimation("attack_1");
        });
        sequence.Join(c.DORotate(Vector3.zero, 0.2f));

        // Target gets hit
        sequence.Append(t.DOAnchorPosX(t.anchoredPosition.x + _chargeTargetHit * dir, 0.2f).OnStart(() =>
        { 
            target.Damage(damage);
            target.PlayAnimation("damage_1");
        }));
        sequence.Join(t.DORotate(-_chargeRotate * dir, 0.1f));
        
        // Move entities back to the original pos
        sequence.Append(c.DOAnchorPosX(0, 0.3f)).OnStart(() =>
        {
            caster.PlayAnimation("attack_2");
        });
        sequence.Join(t.DOAnchorPosX(0, 0.2f)).OnStart(() =>
        {
            target.PlayAnimation("damage_2");
        });
        sequence.Join(t.DORotate(Vector3.zero, 0.2f));
        sequence.OnComplete(() => 
        {
            caster.PlayAnimation("idle", true);
            target.PlayAnimation("idle", true);
            OnAbilityFinished();
        });
        sequence.Play();
    }

    private void OnDefend()
    {
        
    }

    private void OnHeal()
    {
        
    }

    private void OnAbilityFinished()
    {
        NextPhase();
    }

    private void OnEnemyTurn()
    {
        DiceBox.DiceRolled += OnDiceRolled;

        IEnumerator wait()
        {
            yield return new WaitForSeconds(1);
            _diceBox.RollCompleted();
        };

        StartCoroutine(wait());
    }
}
