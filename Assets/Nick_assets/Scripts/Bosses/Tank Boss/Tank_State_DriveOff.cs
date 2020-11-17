using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_DriveOff : Tank_State
{
    public override void DoState()
    {
        if (!Boss.isDriveBy)
        {
            SM.ChangeState(SM.DriveCombo);
        }
    }

    public override void Enter()
    {
        Boss.sp.color = Color.green;

        Location_Boss target;
        int r = Random.Range(0, 2);
        if (r == 0) // Go Left
        {
            Boss.isOffLeft = true;
            if (Boss.isUp)
            {
                target = Boss.Off_C;
            }
            else
            {
               target = Boss.Off_A;
            }
        }
        else // Go right
        {
            Boss.isOffLeft = false;
            if (Boss.isUp)
            {
                target = Boss.Off_D;
            }
            else
            {
                target = Boss.Off_B;
            }
        }
        Boss.StartDriveBy(Boss.currentBossLocation, target);
        Boss.isDriving = true;
        //Boss.DriveComboDelayStart(target);
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_DriveOff(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
