using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DiceBox : MonoBehaviour
{
    public static event Action<DiceResult> DiceRolled;

    public Camera cameraToPlaceWalls;

    [Header("Box edges")]
    public Transform topWall;
    public Transform bottomWall;
    public Transform leftWall;
    public Transform rightWall;
    public Transform floor;
    public Transform roof;
    public float wallsize;
    private float boxHeight;
    private float boxWidth;

    [Header("Dice Throws")]
    public DiceManager diceManager;
    private Dice currentPlayerDice;
    private Dice currentEnemyDice;
    public float forceStrength;
    public float rotateStrength;
    private bool watchDices;
    private float hideDiceCooldown = 1.0f;

    [Header("Dice Results")]
    public LayerMask diceSideLayer;

    private void Start()
    {
        SetBoxDimensions();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowRandomDice();
        }

        if (watchDices)
        {
            if (currentPlayerDice.myRigidbody.velocity.x == 0
                && currentPlayerDice.myRigidbody.velocity.y == 0
                && currentPlayerDice.myRigidbody.velocity.z == 0
                && currentEnemyDice.myRigidbody.velocity.x == 0
                && currentEnemyDice.myRigidbody.velocity.y == 0
                && currentEnemyDice.myRigidbody.velocity.z == 0)
            {
                watchDices = false;

                currentPlayerDice.myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                currentPlayerDice.transform.DOMove(new Vector3(-2f, 1, 0), 1f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => {
                        StartCoroutine(HideDice());
                        Invoke(nameof(RollCompleted), 0.5f);
                    });

                currentEnemyDice.myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                currentEnemyDice.transform.DOMove(new Vector3(2f, 1, 0), 1f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => {
                        StartCoroutine(HideDice());
                        Invoke(nameof(RollCompleted), 0.5f);
                    });
            }
        }
    }

    internal void RollCompleted()
    {
        int[] diceResult = DecideResult();
        DiceResult result = new DiceResult(diceResult);
        DiceRolled?.Invoke(result);
    }

    //internal void RollAIDice(Dice[] dices)
    //{
    //    SoundManager.Instance.Play(Sounds.diceRoll);

    //    int rndDice = UnityEngine.Random.Range(0, dices.Length);
    //    Dice dice = dices[rndDice];

    //    //DiceAbility ability = dice.sides[UnityEngine.Random.Range(0, dice.sides.Length)].type;
    //    int[] diceValue = dice.sides.Length;
    //    DiceResult result = new DiceResult(diceValue);
    //    DiceRolled?.Invoke(result);
    //}

    private IEnumerator HideDice()
    {
        yield return new WaitForSeconds(hideDiceCooldown);
        diceManager.HideDices();
    }
    private int[] DecideResult()
    {
        Vector3 playerDiceWorldPos = currentPlayerDice.transform.position;
        Vector3 enemyDiceWorldPos = currentEnemyDice.transform.position;

        Vector2 playerDiceWorldToViewportPoint = cameraToPlaceWalls.WorldToViewportPoint(playerDiceWorldPos);

        Vector2 enemyDiceWorldToViewportPoint = cameraToPlaceWalls.WorldToViewportPoint(enemyDiceWorldPos);

        Ray r = cameraToPlaceWalls.ViewportPointToRay(playerDiceWorldToViewportPoint);
        RaycastHit info;
        Physics.Raycast(r, out info, 100f, diceSideLayer);

        Ray r2 = cameraToPlaceWalls.ViewportPointToRay(enemyDiceWorldToViewportPoint);
        RaycastHit info2;
        Physics.Raycast(r2, out info2, 100f, diceSideLayer);

        if (info.collider != null && info2.collider != null)
            return new int[] {info.collider.GetComponent<DiceSide>().diceSideNumber, info2.collider.GetComponent<DiceSide>().diceSideNumber }; 

        return null;
    }

    private void FixedUpdate()
    {
        if (watchDices)
        {
            currentPlayerDice.myRigidbody.drag += 0.01f;
            currentPlayerDice.myRigidbody.angularDrag += 0.01f;

            currentEnemyDice.myRigidbody.drag += 0.01f;
            currentEnemyDice.myRigidbody.angularDrag += 0.01f;
        }
    }

    /// <summary>
    /// Sets the dimensions of the DiceBox
    /// </summary>
    void SetBoxDimensions()
    {
        // Calculates the frustum to size the box
        boxHeight = 2.0f * Vector3.Distance(this.transform.position, cameraToPlaceWalls.transform.position)
            * Mathf.Tan(cameraToPlaceWalls.fieldOfView * 0.5f * Mathf.Deg2Rad);
        boxWidth = boxHeight * cameraToPlaceWalls.aspect;

        // Set all the floors and walls at the right positions
        floor.localScale = new Vector3(boxWidth, boxHeight);
        roof.localScale = new Vector3(boxWidth, boxHeight);

        roof.transform.localPosition = new Vector3(0, wallsize);

        topWall.localScale = new Vector3(boxWidth, wallsize, wallsize);
        bottomWall.localScale = new Vector3(boxWidth, wallsize, wallsize);

        rightWall.localScale = new Vector3(wallsize, wallsize, boxHeight);
        leftWall.localScale = new Vector3(wallsize, wallsize, boxHeight);

        topWall.transform.localPosition = new Vector3(0, wallsize / 2f, (boxHeight / 2) + wallsize / 2f - 0.5f);
        bottomWall.transform.localPosition = new Vector3(0, wallsize / 2f, (-boxHeight / 2) - wallsize / 2f + 0.5f);

        leftWall.transform.localPosition = new Vector3((-boxWidth / 2) - wallsize / 2f + .5f, wallsize / 2f, 0);
        rightWall.transform.localPosition = new Vector3((boxWidth / 2) + wallsize / 2f - .5f, wallsize / 2f, 0);
    }


    /// <summary>
    /// Throws a random dice inside the box
    /// </summary>
    public void ThrowRandomDice()
    {
        SoundManager.Instance.Play(Sounds.diceRoll);

        Dice[] dices = diceManager.SetDice(DiceType.D20, DiceType.D20);

        // Player Dice
        currentPlayerDice = dices[0];

        currentPlayerDice.myRigidbody.drag = 1f;
        currentPlayerDice.myRigidbody.angularDrag = 1f;

        currentPlayerDice.myRigidbody.constraints = RigidbodyConstraints.None;

        currentPlayerDice.transform.localPosition = new Vector3(-boxWidth / 2f + 2f, wallsize / 2f, -boxHeight / 2f + 2f);
        currentPlayerDice.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        currentPlayerDice.myRigidbody.velocity = new Vector3(UnityEngine.Random.Range(0, forceStrength), 0, UnityEngine.Random.Range(0, forceStrength));

        // Enemy Dice
        currentEnemyDice = dices[1];

        currentEnemyDice.myRigidbody.drag = 1f;
        currentEnemyDice.myRigidbody.angularDrag = 1f;

        currentEnemyDice.myRigidbody.constraints = RigidbodyConstraints.None;

        currentEnemyDice.transform.localPosition = new Vector3(-boxWidth / 2f + 2f, wallsize / 2f, -boxHeight / 2f + 2f);
        currentEnemyDice.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        currentEnemyDice.myRigidbody.velocity = new Vector3(UnityEngine.Random.Range(0, forceStrength), 0, UnityEngine.Random.Range(0, forceStrength));


        watchDices = true;
    }


}
