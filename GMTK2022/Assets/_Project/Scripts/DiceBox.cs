using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBox : MonoBehaviour
{
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
    public Dice mainDice;
    public float forceStrength;
    private bool watchDice;

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

        topWall.transform.localPosition = new Vector3(0, wallsize / 2f, (boxHeight / 2) + wallsize / 2f);
        bottomWall.transform.localPosition = new Vector3(0, wallsize / 2f, (-boxHeight / 2) - wallsize / 2f);

        leftWall.transform.localPosition = new Vector3((-boxWidth / 2) - wallsize / 2f, wallsize / 2f, 0);
        rightWall.transform.localPosition = new Vector3((boxWidth / 2) + wallsize / 2f, wallsize / 2f, 0);

    }


    /// <summary>
    /// Throws a random dice inside the box
    /// </summary>
    void ThrowRandomDice()
    {
        mainDice.SetRandomDice();

        mainDice.transform.localPosition = new Vector3(-boxWidth / 2f + 2f, wallsize / 2f, -boxHeight / 2f + 2f);

        mainDice.myRigidbody.velocity = new Vector3(UnityEngine.Random.Range(0, forceStrength), 0, UnityEngine.Random.Range(0, forceStrength));

        watchDice = true;
    }


}
