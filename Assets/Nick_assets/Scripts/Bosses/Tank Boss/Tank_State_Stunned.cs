using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_Stunned : Tank_State
{
    float timer;


    public override void DoState()
    {
        if (timer < Duration)
        {
            timer += Time.deltaTime;
            // 
        }
        else
        {
            SM.ChangeState(SM.Idle);
        }
    }

    public override void Enter()
    {
        base.Enter();
        Boss.ClearTargets();
        timer = 0;

    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_Stunned(Tank_StateManager myStateManager, Tank_Boss myBoss, string myName, float myDur) : base(myStateManager, myBoss, myName, myDur)
    {

    }
}
