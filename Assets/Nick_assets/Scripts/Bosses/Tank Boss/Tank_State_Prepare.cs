using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_Prepare : Tank_State
{
    Tank_State AttackToPerform;

    float t;
    bool canStartDrive;

    public override void DoState()
    {
        if (t < Duration)
        {
            t += Time.deltaTime;
        }
        else
        {
            if (canStartDrive)
            {
                StartDriveCombo();
            }
        }
    }

    private Location FindPlayer()
    {
        Location loc = null;
        for (int i = 0; i < Boss.PlayerLocations.Length; i++)
        {
            if (Boss.PlayerLocations[i].isOccupied)
            {
                loc = Boss.PlayerLocations[i];
            }
        }
        return loc;
    }

    private List<Location> DetermineTargets(/*Tank_State attack,*/ Location playerLoc)
    {
        List<Location> targets = new List<Location>();

        /*switch (attack.GetName())
        {

        }*/

        targets.Add(playerLoc);
        return targets;
    }

    private void DetermineDriveBy()
    {
        //int r = Random.Range(1, 5); // random number, 1 - 4

        if (Boss.isOffLeft)
        {
            int r = Random.Range(0, 2);
            if (r == 0) // A -> B
            {
                Boss.startDriveLoc = Boss.Off_A;
                Boss.endDriveLoc = Boss.Off_B;
                Boss.isOffLeft = false;
            }
            else // C -> D
            {
                Boss.startDriveLoc = Boss.Off_C;
                Boss.endDriveLoc = Boss.Off_D;
                Boss.isOffLeft = false;
            }
        }
        else
        {
            int r = Random.Range(0, 2);
            if (r == 0) // B -> A
            {
                Boss.startDriveLoc = Boss.Off_B;
                Boss.endDriveLoc = Boss.Off_A;
                Boss.isOffLeft = true;
            }
            else // D -> C
            {
                Boss.startDriveLoc = Boss.Off_D;
                Boss.endDriveLoc = Boss.Off_C;
                Boss.isOffLeft = true;
            }
        }
    }

    private void PrepareDriveBy()
    {
        if (Boss.isDriving) // is already in the drive by combo
        {
            canStartDrive = false;
            if (Boss.DriveComboCounter < Boss.DriveComboNumber)
            {
                DetermineDriveBy();
                Boss.DriveComboCounter++;
                SM.ChangeState(SM.DriveBy);
            }
            else
            {
                SM.ChangeState(SM.DriveIn);
            }
        }
        else // Start the drive by combo
        {
            Boss.DriveComboCounter = 0;
            canStartDrive = true;
        }
    }


    public override void Enter()
    {
        Boss.sp.color = Color.yellow;
        t = 0;

        // IF DRIVEBY
        //PrepareDriveBy();

        // IF JUMP ATTACK
        Location playerLoc = FindPlayer();
        Boss.targets = DetermineTargets(playerLoc);
        Boss.TargetLocations();
        SM.ChangeState(SM.AttackJump);
    }

    private void StartDriveCombo()
    {
        SM.ChangeState(SM.DriveOff);
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_Prepare(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
