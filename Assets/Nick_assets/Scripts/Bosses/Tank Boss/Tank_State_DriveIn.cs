using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_DriveIn : Tank_State
{
    public override void DoState()
    {
        if (!Boss.isDriveBy)
        {
            SM.ChangeState(SM.Idle);
        }
    }

    public override void Enter()
    {
        Debug.Log("Drive IN");

        //Boss.sp.color = Color.green;
        int r = Random.Range(1, 4); // 1 = left, 2 = middle, 3 = right
        int r2 = Random.Range(0, 2); // 0 = top, else, bot

        Location_Boss target;
        Location_Boss start;

        switch (r)
        {
            case 1:
                if (r2 == 0)
                    target = Boss.BossLocC;
                else
                    target = Boss.BossLocA;
                break;
            case 2:
                if (r2 == 0)
                    target = Boss.BossLocCD;
                else
                    target = Boss.BossLocAB;
                break;
            case 3:
                if (r2 == 0)
                    target = Boss.BossLocD;
                else
                    target = Boss.BossLocB;
                break;
            default:
                target = Boss.BossLocC; // default to C
                Debug.Log("ERROR");
                break;
        }

        if (target.isUp)
        {
            if (Boss.isOffLeft)
                start = Boss.Off_C;
            else
                start = Boss.Off_D;
        }
        else
        {
            if (Boss.isOffLeft)
                start = Boss.Off_A;
            else
                start = Boss.Off_B;
        }

        Boss.StartDriveBy(start, target);
    }

    public override void Leave()
    {
        base.Leave();
        Boss.isDriving = false;
    }

    public Tank_State_DriveIn(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
