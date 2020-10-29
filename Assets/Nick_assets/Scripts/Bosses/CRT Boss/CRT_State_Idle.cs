using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_Idle : CRT_State
{
    private float duration;
    private float t;

    public override void DoState()
    {
        if (t < duration) // slight pause
        {
            Debug.Log("IDLE");
            t += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.PrepareAttack);
        }
    }

    public override void Enter()
    {
        Boss.sp.color = Color.white;

        duration = Boss.TimeBetweenAttacks;
        t = 0;

        Boss.ClearTargets();
    }
    public override void Leave()
    {
        base.Leave();

    }

    public CRT_State_Idle(CRT_StateManager myStateManager, CRT_Boss myBoss) : base(myStateManager, myBoss)
    {
    }
}
