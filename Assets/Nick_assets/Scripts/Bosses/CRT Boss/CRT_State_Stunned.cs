using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_Stunned : CRT_State
{
    private float timer;

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
        Boss.sp.color = Color.green;
        Boss.ClearTargets();
        timer = 0;
    }

    public override void Leave()
    {
        
    }

    public CRT_State_Stunned(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, float myDur) : base(myStateManager, myBoss, myName, myDur)
    {

    }
}
