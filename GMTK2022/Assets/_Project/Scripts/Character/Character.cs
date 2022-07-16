using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public enum CharacterAnimType
{ 
    attack,
    healSelf,
    addShield,
    takeDamage,
    die
}

public class Character : MonoBehaviour
{
    public RectTransform _characterSprite;
    public CanvasGroup statusGroup;

    protected int maxHealth;
    protected int maxShield => maxHealth;
    protected string characterName;

    private int currentHealth;
    private int currentShield;
    public Dice[] dices; // ASSIGN THROUGH CHARACTER DATA/INVENTORY
    public bool IsAlive { get { return currentHealth > 0; } }

    [Header("UI-Config")]
    public Image healthBar;
    public Image shieldBar;
    public CanvasGroup dialogueBox;
    public TextMeshProUGUI dialogueText;

    [Header("Animations")]
    public Image shadow;
    public GameObject _spineObj;
    protected SpineHandler spineHandler;
    public string spineSuffix;

    private void Start()
    {
        Initialize(null);
    }

    private void Initialize(CharacterData baseData)
    {
        //characterName = baseData.GetEnemyName();
        maxHealth = 100;//baseData.GetMaxHealth();
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
                Debug.Log("shield " + currentShield);
            }
            else
            {
                int leftoverDamage = damage - currentShield;
                currentHealth -= leftoverDamage;
                Debug.Log("shield + health " + currentShield + " | " + currentHealth);
                currentShield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
            Debug.Log("health " + currentHealth);
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

        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth) ;
        //AnimateCharacter(CharacterAnimType.heal);
        UpdateCharacterUI();
    }
    public void AddShield(int shieldAmount)
    {
        SayDialogue("HAHA! SHIELD POWERED UP!");

        currentShield = Mathf.Clamp(currentShield + shieldAmount, 0, maxShield);
        //AnimateCharacter(CharacterAnimType.defend);
        UpdateCharacterUI();
    }
    public void Die()
    {
        //AnimateCharacter(CharacterAnimType.die);
        SayDialogue("Damn I'm dead.");
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
