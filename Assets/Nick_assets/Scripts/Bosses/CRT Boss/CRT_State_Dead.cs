using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_Dead : CRT_State
{
    public override void DoState()
    {
    }

    public override void Enter()
    {
        Boss.sp.color = Color.gray;
    }

    public override void Leave()
    {
    }

    public CRT_State_Dead(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, float myDur) : base(myStateManager, myBoss, myName, myDur)
    {
    }
}
