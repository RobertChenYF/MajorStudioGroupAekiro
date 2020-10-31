using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Player : MonoBehaviour
{
    public Temp_Loc locA, locB, locC, locD;
    Temp_Loc currentLoc;
    Vector2 posA, posB, posC, posD;
    Vector2 targetPos;

    public bool isRight;
    public bool isUp;
    bool canDamage = true;
    public int Damage;

    void Start()
    {
        isRight = true;
        isUp = false;
        // START AT POS B (bottom right)
        Damage = 0;

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
            currentLoc = locD;
            locD.isOccupied = true;
        }
        else
            locD.isOccupied = false;

        if (isRight && !isUp)
        {
            targetPos = posB;
            currentLoc = locB;
            locB.isOccupied = true;
        }
        else
            locB.isOccupied = false;

        if (!isRight && isUp)
        {
            targetPos = posC;
            currentLoc = locC;
            locC.isOccupied = true;
        }
        else
            locC.isOccupied = false;

        if (!isRight && !isUp)
        {
            targetPos = posA;
            currentLoc = locA;
            locA.isOccupied = true;
        }
        else
            locA.isOccupied = false;


        if (currentLoc.isHit)
        {
            StartCoroutine(TakeDamage());
        }

    }

    IEnumerator TakeDamage()
    {
        if (canDamage)
            Damage++;
        canDamage = false;
        yield return new WaitForSeconds(0.75f);
        canDamage = true;
        

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
