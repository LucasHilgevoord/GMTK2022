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
    private CardAbility _playerAbility, _enemyAbility;

    [Header("Attack")]
    private float _chargeBack = 100;
    private float _chargeFront = 200;
    private float _chargeTargetHit = 100;
    private Vector3 _chargeRotate = new Vector3(0, 0, 6);

    private void Start()
    {
        SoundManager.Instance.Play(Sounds.battleMusic, true);
        Initialize();
    }

    private void Initialize()
    {
        _currentPhase = BattlePhase.PickAbility;
        NextPhase(false);
    }

    private void NextPhase(bool change = true)
    {
        if (change)
            _currentPhase = _currentPhase.Next();

        switch (_currentPhase)
        {
            case BattlePhase.PickAbility:
                CardsManager.CardPicked += OnAbiliyPicked;
                _cardsManager.ShowCards();
                break;
            case BattlePhase.ThrowDice:
                NextPhase(); // TEMP
                break;
            case BattlePhase.Ability:
                CheckAbility(UnityEngine.Random.Range(1, 20), UnityEngine.Random.Range(1, 20));
                break;
            default:
                break;
        }
    }

    private void OnAbiliyPicked(CardAbility ability)
    {
        CardsManager.CardPicked -= OnAbiliyPicked;
        _playerAbility = ability;
        _enemyAbility = (CardAbility)UnityEngine.Random.Range(0, Enum.GetNames(typeof(CardAbility)).Length - 1);
        DisplayAbilityPick();
    }

    private void DisplayAbilityPick()
    {
        _player.iconEffect.ShowIcon(_playerAbility);
        _enemyManager.FocussedEnemy.iconEffect.ShowIcon(_enemyAbility);
        NextPhase();
    }

    private void CheckAbility(int playerValue, int attackerValue)
    {
        if (_playerAbility == CardAbility.Attack && _enemyAbility == CardAbility.Attack)
        {
            AttackTarget(_player, _enemyManager.FocussedEnemy, playerValue, false);
            AttackTarget(_enemyManager.FocussedEnemy, _player, attackerValue, false);
        } 
        else if ((_playerAbility == CardAbility.Attack && _enemyAbility == CardAbility.Parry) ||
            (_playerAbility == CardAbility.Parry && _enemyAbility == CardAbility.Attack))
        {
            OnParry(playerValue, attackerValue);
        } else
        {
            if (_playerAbility == CardAbility.Attack)
            {
                AttackTarget(_player, _enemyManager.FocussedEnemy, playerValue, true);
            }
            else if (_enemyAbility == CardAbility.Attack)
            {
                AttackTarget(_enemyManager.FocussedEnemy, _player, attackerValue, true);
            }
            
            if (_playerAbility == CardAbility.Heal)
            {
                HealTarget(_player, playerValue);
            } else if (_enemyAbility == CardAbility.Heal)
            {
                HealTarget(_enemyManager.FocussedEnemy, attackerValue);
            }
        }

        // TESTING
        IEnumerator waitForNextPhase()
        {
            yield return new WaitForSeconds(2);
            NextPhase();
        }
        StartCoroutine(waitForNextPhase());
    }

    private void AttackTarget(Character caster, Character target, int damage, bool useTargetKnockback = true)
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
        }).OnComplete(() =>
        {
            // Damage the character here if both enemies attack eachother
            if (!useTargetKnockback)
                target.Damage(damage);
        }));
        sequence.Join(c.DORotate(Vector3.zero, 0.2f));

        // Target gets hit
        if (useTargetKnockback)
        {
            sequence.Append(t.DOAnchorPosX(t.anchoredPosition.x + _chargeTargetHit * dir, 0.2f).OnStart(() =>
            {
                target.Damage(damage);
                target.PlayAnimation("damage_1");
            }));
            sequence.Join(t.DORotate(-_chargeRotate * dir, 0.1f));
        }

        // Move entities back to the original pos
        sequence.Append(c.DOAnchorPosX(0, 0.2f).OnStart(() =>
        {
            caster.PlayAnimation("attack_2");
        }));

        if (useTargetKnockback)
        {
            sequence.Join(t.DOAnchorPosX(0, 0.2f).OnStart(() =>
            {
                caster.PlayAnimation("damage_2");
            }));
            sequence.Join(t.DORotate(Vector3.zero, 0.2f));
        }
        sequence.OnComplete(() => 
        {
            caster.PlayAnimation("idle", true);
            if (target.IsAlive)
            {
                target.PlayAnimation("idle", true);
                //onComplete?.Invoke();
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
        CardAbility winnerAbility = playerWins ? _playerAbility : _playerAbility;
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

        // Potential: Add shield if the parry wins
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

        dieSequence.PrependInterval(1f);
        dieSequence.Append(target.statusGroup.DOFade(0, 0.5f));
        dieSequence.Join(target.shadow.DOFade(0, 0.5f));
        dieSequence.Play();

        if (target == _player)
        {
            Debug.Log("You lost");
        }
        else
        {
            Debug.Log("You win!");
        }
    }
}
