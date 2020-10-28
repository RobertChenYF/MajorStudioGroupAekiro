using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_Idle : CRT_State
{

    public override void DoState()
    {
        Debug.Log("IDLE");

        //TEMP
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SM.ChangeState(SM.SingleAttack);
        }
        // SEARCH FOR PLAYER
    }

    public override void Enter()
    {
        Boss.sp.color = Color.white;
    }
    public override void Leave()
    {
        base.Leave();

    }

    public CRT_State_Idle(CRT_StateManager myStateManager, CRT_Boss myBoss) : base(myStateManager, myBoss)
    {

    }
}
