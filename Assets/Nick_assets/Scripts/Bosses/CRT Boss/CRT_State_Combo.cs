using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_Combo : CRT_State
{
    public override void DoState()
    {

    }

    public override void Enter()
    {
        Boss.sp.color = Color.yellow;
        Boss.ClearTargets();
    }

    public override void Leave()
    {
        base.Leave();
    }

    public CRT_State_Combo(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, CRT_State myPrev): base(myStateManager, myBoss, myName, myPrev)
    {
    }
}
