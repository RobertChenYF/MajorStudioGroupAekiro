using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Player : MonoBehaviour
{
    public Temp_Loc locA, locB, locC, locD;
    Vector2 posA, posB, posC, posD;
    Vector2 targetPos;

    public bool isRight;
    public bool isUp;

    void Start()
    {
        isRight = true;
        isUp = false;
        // START AT POS B (bottom right)

        posA = locA.transform.position;
        posB = locB.transform.position;
        posC = locC.transform.position;
        posD = locD.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();

        if (isRight && isUp)
        {
            targetPos = posD;
            locD.isOccupied = true;
        }
        else
            locD.isOccupied = false;

        if (isRight && !isUp)
        {
            targetPos = posB;
            locB.isOccupied = true;
        }
        else
            locB.isOccupied = false;

        if (!isRight && isUp)
        {
            targetPos = posC;
            locC.isOccupied = true;
        }
        else
            locC.isOccupied = false;

        if (!isRight && !isUp)
        {
            targetPos = posA;
            locA.isOccupied = true;
        }
        else
            locA.isOccupied = false;

    }

    void UpdateMovement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            isRight = true;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            isRight = false;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            isUp = true;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            isUp = false;

        this.transform.position = Vector2.Lerp(this.transform.position, targetPos, Time.deltaTime * 10);
    }
}
