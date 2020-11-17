using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_DriveCombo : Tank_State
{
    private float t;
    public override void DoState()
    {

        if (t < Duration)
        {
            t += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.Prepare);
        }
    }

    public override void Enter()
    {
        base.Enter();
        t = 0;
        Boss.rb.velocity = Vector2.zero;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_DriveCombo(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
