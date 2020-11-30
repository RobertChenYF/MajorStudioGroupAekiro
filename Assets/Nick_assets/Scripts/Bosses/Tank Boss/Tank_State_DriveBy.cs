using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_DriveBy : Tank_State
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
        //Boss.sp.color = Color.green;

        Boss.StartDriveBy(Boss.startDriveLoc, Boss.endDriveLoc);
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_DriveBy(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
   
