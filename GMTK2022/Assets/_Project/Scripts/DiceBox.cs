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
    private Dice currentDice;
    public float forceStrength;
    public float rotateStrength;
    private bool watchDice;
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

        if (watchDice)
        {
            if (currentDice.myRigidbody.velocity.x == 0
                && currentDice.myRigidbody.velocity.y == 0
                && currentDice.myRigidbody.velocity.z == 0)
            {
                watchDice = false;

                currentDice.myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                currentDice.transform.DOMove(new Vector3(0, 1, 0), 1f)
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
        DiceAbility ability = DecideResult();
        int diceValue = currentDice.sides.Length;
        DiceResult result = new DiceResult(ability, diceValue);
        DiceRolled?.Invoke(result);
    }

    internal void RollAIDice(Dice[] dices)
    {
        int rndDice = UnityEngine.Random.Range(0, dices.Length);
        Dice dice = dices[rndDice];

        DiceAbility ability = dice.sides[UnityEngine.Random.Range(0, dice.sides.Length)].type;
        int diceValue = dice.sides.Length;
        DiceResult result = new DiceResult(ability, diceValue);
        DiceRolled?.Invoke(result);
    }

    private IEnumerator HideDice()
    {
        yield return new WaitForSeconds(hideDiceCooldown);
        diceManager.HideDice();
    }
    private DiceAbility DecideResult()
    {
        Ray r = cameraToPlaceWalls.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit info;
        Physics.Raycast(r, out info, 100f, diceSideLayer);

        if (info.collider != null)
            return info.collider.GetComponent<DiceSide>().type;

        return DiceAbility.Attack;
    }

    private void FixedUpdate()
    {
        if (watchDice)
        {
            currentDice.myRigidbody.drag += 0.01f;
            currentDice.myRigidbody.angularDrag += 0.01f;
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
        currentDice = diceManager.SetDice(DiceType.D20, 10, 5, 5);

        currentDice.myRigidbody.drag = 1f;
        currentDice.myRigidbody.angularDrag = 1f;

        currentDice.myRigidbody.constraints = RigidbodyConstraints.None;


        currentDice.transform.localPosition = new Vector3(-boxWidth / 2f + 2f, wallsize / 2f, -boxHeight / 2f + 2f);
        currentDice.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        //mainDice.myRigidbody.angularVelocity = new Vector3(UnityEngine.Random.Range(0, rotateStrength), UnityEngine.Random.Range(0, rotateStrength), UnityEngine.Random.Range(0, rotateStrength));
        currentDice.myRigidbody.velocity = new Vector3(UnityEngine.Random.Range(0, forceStrength), 0, UnityEngine.Random.Range(0, forceStrength));

        watchDice = true;
    }


}
