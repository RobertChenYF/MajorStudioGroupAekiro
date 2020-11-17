using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_Idle : Tank_State
{
    private float t;
    private Tank_State[][] MoveList;

    public override void DoState()
    {
        Boss.transform.position = Boss.currentBossLocation.transform.position;
        if (t < Duration)
        {
            t += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.Prepare);

            /*if (SM.comboActive)
                SM.ChangeState(SM.Prepare);
            else
            {
                SM.PerformCombo(MoveList[Random.Range(0, MoveList.Length)]);
            }*/
        }
    }

    public override void Enter()
    {
        Boss.sp.color = Color.cyan;
        Boss.rb.velocity = Vector2.zero;
        t = 0;

        MoveList = SM.MoveList_1;

    }

    public override void Leave()
    {
        base.Leave();

    }

    public Tank_State_Idle(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
