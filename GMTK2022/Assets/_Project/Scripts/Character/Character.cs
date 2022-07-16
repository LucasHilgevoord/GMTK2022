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
    private int shield;
    //private Dice[] dices;
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
        shield = maxHealth;

        spineHandler = new SpineHandler(_spineObj, spineSuffix);
    }

    #region virtual-methods
    public void Damage(int damage)
    {
        SayDialogue("OUCH! You think you're sh*t ?!");

        if (shield > 0)
        {
            if (shield >= damage)
            {
                shield -= damage;
                Debug.Log("shield " + shield);
            }
            else
            {
                int leftoverDamage = damage - shield;
                currentHealth -= leftoverDamage;
                Debug.Log("shield + health " + shield + " | " + currentHealth);
                shield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
            Debug.Log("health " + currentHealth);
        }

        // Clamp the values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        shield = Mathf.Clamp(shield, 0, maxShield);
        UpdateCharacterUI();

        if (currentHealth <= 0)
            Die();
    }
    public void Heal(int healAmount)
    {
        SayDialogue("Ahhh, feels nice.");

        shield = Mathf.Clamp(currentHealth + healAmount, 0, healAmount) ;
        //AnimateCharacter(CharacterAnimType.heal);
        UpdateCharacterUI();
    }
    public void AddShield(int shieldAmount)
    {
        SayDialogue("HAHA! SHIELD POWERED UP!");

        shield = Mathf.Clamp(shield + shieldAmount, 0, maxShield);
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

    //#region abstract-methods
    //public abstract void OnAttack();
    //public abstract void OnHeal();
    //public abstract void OnShieldIncrease();
    //public abstract void OnDie();
    //public abstract void OnDamageTaken();
    //public abstract void OnIdle();
    //#endregion

    //private void AnimateCharacter(CharacterAnimType animType)
    //{
    //    Debug.Log("animate character " + spineSuffix);
    //    switch (animType)
    //    {
    //        case CharacterAnimType.attack:
    //            OnAttack();
    //            break;
    //        case CharacterAnimType.healSelf:
    //            OnHeal();
    //            break;
    //        case CharacterAnimType.addShield:
    //            OnShieldIncrease();
    //            break;
    //        case CharacterAnimType.die:
    //            OnDie();
    //            break;
    //        case CharacterAnimType.takeDamage:
    //            OnDie();
    //            break;
    //    }
    //}

    internal void PlayAnimation(string animationName, bool loop = false, bool useSuffix = false)
    {
        spineHandler.PlayAnimation(animationName, loop, useSuffix);
    }

    private void UpdateCharacterUI()
    {
        if (currentHealth >= 0)
            healthBar.DOFillAmount((float)currentHealth / maxHealth, 0.2f);
        
        if (shield >= 0)
            shieldBar.DOFillAmount((float)shield / maxShield, 0.2f);
    }
}
