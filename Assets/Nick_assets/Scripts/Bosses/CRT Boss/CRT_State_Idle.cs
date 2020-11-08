using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_Idle : CRT_State
{
    private float t;
    private CRT_State[][] MoveList;

    public override void DoState()
    {
        if (t < Duration) // slight pause
        {
            t += Time.deltaTime;
        }
        else
        {
            if (SM.comboActive)
                SM.ChangeState(SM.PrepareAttack);
            else
            {
                //SM.ChangeState(SM.ComboState);
                SM.PerformCombo(MoveList[Random.Range(0, MoveList.Length)]);
            }
        }
    }

    public override void Enter()
    {
        Boss.sp.color = Color.cyan;

        t = 0;

        Boss.ClearTargets();

        switch (Boss.CurrentPhase)
        {
            case 1:
                MoveList = SM.MovesList_1;
                Duration = Boss.TimeBetweenAttacks_P1;
                break;
            case 2:
                MoveList = SM.MovesList_2;
                Duration = Boss.TimeBetweenAttacks_P2;
                break;
            case 3:
                MoveList = SM.MovesList_3;
                Duration = Boss.TimeBetweenAttacks_P3;
                break;
            default:
                break;
        }
    }
    public override void Leave()
    {
        base.Leave();

    }

    public CRT_State_Idle(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, float myDur) : base(myStateManager, myBoss, myName, myDur)
    {
    }
}
