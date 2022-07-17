using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public enum CharacterInteractionType
{ 
    attack,
    healSelf,
    addShield,
    takeHealthDamage,
    takeShieldDamage,
    takeMixedDamage,
    die
}

public class Character : MonoBehaviour
{
    public RectTransform _characterSprite;
    public CanvasGroup statusGroup;

    public int maxHealth = 100;
    protected int maxShield => maxHealth;
    protected string characterName;

    private int currentHealth;
    private int currentShield;
    public bool IsAlive { get { return currentHealth > 0; } }

    [Header("UI-Config")]
    public Image healthBar;
    public Image shieldBar;
    public CanvasGroup dialogueBox;
    public TextMeshProUGUI dialogueText;
    public BattleEffectHandler battleEffectHandler;
    public AbilityIconEffect iconEffect;

    [Header("Animations")]
    public Image shadow;
    public GameObject _spineObj;
    public SpineHandler spineHandler;
    public string spineSuffix;
    public ParticleSystem healParticle;

    public bool IsDead { get { return currentHealth <= 0; } }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        //maxHealth = 100;
        currentHealth = maxHealth;
        currentShield = 0;

        spineHandler = new SpineHandler(_spineObj, spineSuffix);
        UpdateCharacterUI(true);
    }

    #region virtual-methods 
    public void Damage(int damage)
    {
        SayDialogue("OUCH! You think you're sh*t ?!");

        if (currentShield > 0)
        {
            if (currentShield >= damage)
            {
                currentShield -= damage;
                battleEffectHandler.ShowBattleEffect(CharacterInteractionType.takeShieldDamage, damage);

                Debug.Log("shield " + currentShield);
            }
            else
            {
                int leftoverDamage = damage - currentShield;
                currentHealth -= leftoverDamage;
                currentShield = 0;
                battleEffectHandler.ShowBattleEffect(CharacterInteractionType.takeMixedDamage, damage, leftoverDamage);

                Debug.Log("shield + health " + currentShield + " | " + currentHealth);

            }
        }
        else
        {
            currentHealth -= damage;
            battleEffectHandler.ShowBattleEffect(CharacterInteractionType.takeHealthDamage, damage);
        }

        // Clamp the values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);
        UpdateCharacterUI();

        if (currentHealth <= 0)
            Die();
    }
    public void Heal(int healAmount)
    {
        SayDialogue("Ahhh, feels nice.");
        battleEffectHandler.ShowBattleEffect(CharacterInteractionType.healSelf, healAmount);
        healParticle.Play();

        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth) ;
        UpdateCharacterUI();
    }
    public void AddShield(int shieldAmount)
    {
        SayDialogue("HAHA! SHIELD POWERED UP!");

        battleEffectHandler.ShowBattleEffect(CharacterInteractionType.addShield, shieldAmount);

        currentShield = Mathf.Clamp(currentShield + shieldAmount, 0, maxShield);
        UpdateCharacterUI();
    }
    public void Die()
    {
        SayDialogue("I don't feel so good..");
    }

    public void SayDialogue(string dialogue)
    {
        dialogueText.SetText(dialogue);

        Sequence dialoguePopup = DOTween.Sequence();
        dialoguePopup.Append(dialogueBox.DOFade(1.0f, 0.2f));
        dialoguePopup.AppendInterval(1.5f);
        dialoguePopup.Append(dialogueBox.DOFade(0.0f, 0.2f));
    }
    #endregion

    internal void PlayAnimation(string animationName, bool loop = false, bool useSuffix = false)
    {
        spineHandler.PlayAnimation(animationName, loop, useSuffix);
    }

    internal void ResetStats()
    {
        healParticle.Play();
        currentHealth = maxHealth;
        currentShield = 0;
        UpdateCharacterUI();
    }

    private void UpdateCharacterUI(bool snap = false)
    {
        if (snap)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
            shieldBar.fillAmount = currentShield / maxShield;
        } else
        {
            if (currentHealth >= 0)
                healthBar.DOFillAmount((float)currentHealth / maxHealth, 0.2f);

            if (currentShield >= 0)
                shieldBar.DOFillAmount((float)currentShield / maxShield, 0.2f);
        }
    }
}
