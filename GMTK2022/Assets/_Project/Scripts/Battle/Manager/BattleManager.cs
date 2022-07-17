using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceBox _diceBox;
    
    [Header("Managers")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private CardsManager _cardsManager;
    private BattlePhase _currentPhase;

    [Header("Characters")]
    [SerializeField] private Character _player;
    private TurnData _playerTurn, _enemyTurn;

    [Header("Attack")]
    private float _chargeBack = 100;
    private float _chargeFront = 200;
    private float _chargeTargetHit = 100;
    private Vector3 _chargeRotate = new Vector3(0, 0, 6);

    public ActiveAbilitiesPanel activeAbilitiesPanel;

    private void Start()
    {
        SoundManager.Instance.Play(Sounds.battleMusic, true, 0.8f);
        StartBattle(true);
    }

    private void StartBattle(bool initialize = false)
    {
        _currentPhase = (BattlePhase)0;
        EnemyManager.NextEnemyAppeared += OnNextEnemyAppeared;
        
        if (!initialize)
            _player.ResetStats();
            
        _enemyManager.NextEnemy();
    }

    private void OnNextEnemyAppeared()
    {
        EnemyManager.NextEnemyAppeared -= OnNextEnemyAppeared;
        NextPhase(false);
    }

    private void NextPhase(bool change = true)
    {
        if (change)
            _currentPhase = _currentPhase.Next();

        switch (_currentPhase)
        {
            case BattlePhase.Initialize:
                InitializeBattle();
                break;
            case BattlePhase.PickAbility:
                CardsManager.CardPicked += OnAbilityPicked;
                activeAbilitiesPanel.ShowActiveAbility(false, false);
                activeAbilitiesPanel.ShowActiveAbility(true, false);
                _cardsManager.ShowCards();
                break;
            case BattlePhase.ThrowDice:
                DiceBox.DiceRolled += OnDiceRolled;
                _diceBox.ThrowRandomDice();
                break;
            case BattlePhase.Ability:
                CheckAbility(_playerTurn.diceValue, _enemyTurn.diceValue);
                break;
            default:
                break;
        }
    }

    private void InitializeBattle()
    {
        ShowPlayer(true);

        _player.SayDialogue("I need to\nget the f*** away\nfrom this place!");
        _enemyManager.FocussedEnemy.SayDialogue("You ain't runnin' away!");

        _playerTurn = new TurnData(_player);
        _enemyTurn = new TurnData(_enemyManager.FocussedEnemy);
        NextPhase();
    }

    private void ShowPlayer(bool show)
    {
        if (show)
            _player.GetComponent<RectTransform>().DOAnchorPosX(170f, 0.5f).SetEase(Ease.InOutSine);
        else
            _player.GetComponent<RectTransform>().DOAnchorPosX(-500f, 0.5f).SetEase(Ease.InOutSine);
    }

    private void OnAbilityPicked(CardAbility ability)
    {
        CardsManager.CardPicked -= OnAbilityPicked;
        _playerTurn.SetAbility(ability);
        _enemyTurn.SetAbility((CardAbility)UnityEngine.Random.Range(0, Enum.GetNames(typeof(CardAbility)).Length));

        activeAbilitiesPanel.SetActiveAbilityIcon(true, _playerTurn.ability);
        activeAbilitiesPanel.SetActiveAbilityIcon(false, _enemyTurn.ability);
        activeAbilitiesPanel.ShowActiveAbility(true, true);
        activeAbilitiesPanel.ShowActiveAbility(false, true);

        NextPhase();
    }

    private void OnDiceRolled(int[] result)
    {
        DiceBox.DiceRolled -= OnDiceRolled;

        // QUICKFIX
        if (result == null || result.Length == 0)
        {
            result = new int[]{ 0, 0 };
        }
        else if (result.Length == 1)
        {
            result = new int[] { result[0], result[0] };
        }

        _playerTurn.SetDiceValue(result[0]);
        _enemyTurn.SetDiceValue(result[1]);
        NextPhase();
    }

    private void CheckAbility(int playerValue, int attackerValue)
    {
        float nextPhaseDelay = 2;
        CardAbility playerAbility = _playerTurn.ability;
        CardAbility enemyAbility = _enemyTurn.ability;

        if (playerValue > attackerValue) activeAbilitiesPanel.ShowActiveAbility(false, false);
        else if (attackerValue > playerValue) activeAbilitiesPanel.ShowActiveAbility(true, false);
        else
        {
            activeAbilitiesPanel.ShowActiveAbility(false, false);
            activeAbilitiesPanel.ShowActiveAbility(true, false);
        }

        if (playerAbility == CardAbility.Attack && enemyAbility == CardAbility.Attack)
        {
            // CASE: Both characters attack
            nextPhaseDelay = 3;
            AttackTarget(_player, _enemyManager.FocussedEnemy, playerValue, () =>
            {
                AttackTarget(_enemyManager.FocussedEnemy, _player, attackerValue);
            });
        } 
        else if ((playerAbility == CardAbility.Attack && enemyAbility == CardAbility.Parry) ||
            (playerAbility == CardAbility.Parry && enemyAbility == CardAbility.Attack))
        {
            // CASE: One character parries, see which one is the winner
            OnParry(playerValue, attackerValue);
        } else
        {
            // Healing characters
            if (playerAbility == CardAbility.Heal)
            {
                HealTarget(_player, playerValue);
            }
            if (enemyAbility == CardAbility.Heal)
            {
                HealTarget(_enemyManager.FocussedEnemy, attackerValue);
            }

            // Only one attacks, and the other one does something else
            if (playerAbility == CardAbility.Attack)
            {
                AttackTarget(_player, _enemyManager.FocussedEnemy, playerValue);
            }
            if (enemyAbility == CardAbility.Attack)
            {
                AttackTarget(_enemyManager.FocussedEnemy, _player, attackerValue);
            }
        }

        // TESTING
        IEnumerator waitForNextPhase()
        {
            yield return new WaitForSeconds(nextPhaseDelay);
            NextPhase();
        }
        StartCoroutine(waitForNextPhase());
    }

    private void AttackTarget(Character caster, Character target, int damage, Action onComplete = null)
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
        sequence.Append(c.DOAnchorPosX(c.anchoredPosition.x + _chargeFront * dir, 0.2f).OnStart(() =>
        {
            caster.PlayAnimation("attack_1");
        }));
        sequence.Join(c.DORotate(Vector3.zero, 0.2f));

        // Target gets hit
        sequence.Append(t.DOAnchorPosX(t.anchoredPosition.x + _chargeTargetHit * dir, 0.2f).OnStart(() =>
        {
            target.Damage(damage);
            target.PlayAnimation("damage_1");
        }));
        sequence.Join(t.DORotate(-_chargeRotate * dir, 0.1f));

        // Move entities back to the original pos
        sequence.Append(c.DOAnchorPosX(0, 0.2f).OnStart(() =>
        {
            caster.PlayAnimation("attack_2");
        }));

        sequence.Join(t.DOAnchorPosX(0, 0.2f).OnStart(() =>
        {
            caster.PlayAnimation("damage_2");
        }));
        sequence.Join(t.DORotate(Vector3.zero, 0.2f));
        sequence.OnComplete(() => 
        {
            caster.PlayAnimation("idle", true);
            if (target.IsAlive)
            {
                target.PlayAnimation("idle", true);
                onComplete?.Invoke();
            }
            else
            {
                OnTargetKilled(target);
            }
        });
        sequence.Play();

        // sfx
        if (caster == _player)
            SoundManager.Instance.Play(Sounds.playerAttack);
        else
            SoundManager.Instance.Play(Sounds.enemyAttack);
    }

    private void OnParry(int playerValue, int attackerValue)
    {
        bool playerWins = playerValue >= attackerValue;
        CardAbility winnerAbility = playerWins ? _playerTurn.ability : _enemyTurn.ability;
        int winnerValue = playerWins ? playerValue : attackerValue;
        int loserValue = playerWins ? attackerValue : playerValue;
        Character winner = playerWins ? _player : _enemyManager.FocussedEnemy;
        Character loser = playerWins ? _enemyManager.FocussedEnemy : _player;


        if (winnerAbility == CardAbility.Parry)
        {
            AttackTarget(winner, loser, loserValue);
            AddShield(winner, winnerValue - loserValue);
        }
        else
            AttackTarget(winner, loser, winnerValue);
    }

    private void AddShield(Character caster, int shield)
    {
        caster.AddShield(shield);
        SoundManager.Instance.Play(Sounds.addShield);  // sfx
    }

    private void HealTarget(Character caster, int health)
    {
        caster.Heal(health);
        SoundManager.Instance.Play(Sounds.heal);  // sfx
    }

    private void OnTargetKilled(Character target)
    {
        target.PlayAnimation("die", false, true);
        Sequence dieSequence = DOTween.Sequence();
        dieSequence.Append(target.statusGroup.DOFade(0, 0.5f));
        dieSequence.Join(target.shadow.DOFade(0, 0.5f));
        dieSequence.AppendInterval(2f);
        dieSequence.Play().OnComplete(() => 
        {
            if (target == _player)
            {
                Debug.Log("You lost");
            }
            else
            {
                StartBattle();
            }
        });
    }
}
