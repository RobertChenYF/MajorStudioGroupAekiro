using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_Prepare : Tank_State
{
    Tank_State AttackToPerform;
    private Tank_State[] MoveList;

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

    /*private List<Location> DetermineTargets(Tank_State attack, Location playerLoc)
    {
        List<Location> targets = new List<Location>();

        switch (attack.GetName())
        {
            case "AttackJump":
                targets.Add(playerLoc);
                return targets;

            default:
                return targets;
        }
    }*/

    private void DetermineAction()
    {
        switch (AttackToPerform.GetName())
        {
            case "Reposition":
                SM.ChangeState(SM.Reposition);
                break;

            case "DriveBy":
                PrepareDriveBy();
                break;

            case "AttackJump_1":
                Boss.SetSound(4);
                TargetPlayerLocation();
                Boss.TargetLocations();
                SM.ChangeState(SM.AttackJumpSingle);
                break;

            case "AttackJump_3":
                Boss.SetSound(4);
                TargetPlayerLocation();
                Boss.TargetLocations();
                SM.ChangeState(SM.AttackJumpTriple);
                break;

            case "ShootGun_1":
                Boss.SetSound(5);
                TargetPlayerLocation();
                Boss.TargetLocations();
                SM.ChangeState(SM.ShootGunSingle);
                break;

            case "ShootGun_3":
                Boss.SetSound(5);
                TargetPlayerLocation();
                Boss.TargetLocations();
                SM.ChangeState(SM.ShootGunTriple);
                break;

            case "Mortar_1":
                Boss.SetSound(5);
                if (Boss.CheckCanMortar())
                {
                    TargetPlayerLocation();
                    SM.ChangeState(SM.ShootMortar_1);
                }
                else
                {
                    Debug.Log("CANNOT FIRE");
                    SM.ChangeState(SM.Reposition);
                }
                break;

            case "Mortar_2":
                Boss.SetSound(5);
                if (Boss.CheckCanMortar())
                {
                    TargetMultipleLocations(SM.ShootMortar_2.GetNumAttacks());
                    SM.ChangeState(SM.ShootMortar_2);
                }
                else
                {
                    Debug.Log("CANNOT FIRE");
                    SM.ChangeState(SM.Reposition);
                }
                break;

            case "Oil_1":
                Boss.SetSound(5);
                TargetPlayerLocation();
                SM.ChangeState(SM.ShootOil_1);
                break;
            default:
                SM.ChangeState(SM.Idle);
                break;
        }
    }

    private void TargetPlayerLocation()
    {
        Location playerLoc = FindPlayer();
        Boss.targets = new List<Location>() { playerLoc };
    }

    private void TargetMultipleLocations(int numAtk)
    {
        List<Location> TargetLocs = new List<Location>();

        List<int> temp = new List<int>();

        for (int i = 0; i < numAtk; i++)
        {
            int x;
            do { x = Random.Range(0, Boss.PlayerLocations.Length); }
            while (temp.Contains(x));
            temp.Add(x);
        }

        foreach (int i in temp)
            TargetLocs.Add(Boss.PlayerLocations[i]);
        Boss.targets = TargetLocs;
    }

    private void DetermineDriveBy()
    {
        //int r = Random.Range(1, 5); // random number, 1 - 4
        bool targetUp = Boss.GetPlayerIsUp();
        if (Boss.isOffLeft)
        {
            //int r = Random.Range(0, 2);
            if (!targetUp) // A -> B
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
            //int r = Random.Range(0, 2);
            if (!targetUp) // B -> A
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
                Boss.SetSound(1);
                DetermineDriveBy();
                Boss.DriveComboCounter++;
                SM.ChangeState(SM.DriveBy);
            }
            else
            {
                Boss.SetSound(3);
                SM.ChangeState(SM.DriveIn);
            }
        }
        else // Start the drive by combo
        {
            Boss.DriveComboCounter = 0;
            Boss.SetSound(2);
            canStartDrive = true;
        }
    }

    private Tank_State GetNextState()
    {
        return SM.nextAttackState;
    }

    private void StartDriveCombo()
    {
        SM.ChangeState(SM.DriveOff);
    }

    public override void Enter()
    {
        Boss.sp.color = Color.yellow;
        t = 0;

        AttackToPerform = GetNextState();
        DetermineAction();
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_Prepare(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
