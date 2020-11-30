using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_Dead : Tank_State
{ 


    public override void DoState()
    {
        throw new System.NotImplementedException();
    }

    public override void Enter()
    {
        base.Enter();
        SM.PlayAnimation(Tank_StateManager.AnimationState.Tank_Die);
        Boss.SetSound(2);
        Boss.PlaySound();

    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_Dead(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur) : base(myManager, myBoss, myName, myDur)
    { }
}
