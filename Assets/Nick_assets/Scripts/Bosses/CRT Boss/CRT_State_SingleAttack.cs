using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CRT_State_SingleAttack : CRT_State
{
    private float duration;
    private float t;

    public override void DoState()
    {
        if (t < duration)
        {
            Debug.Log("SINGLE_ATTACK");
            t += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.Idle);
        }
    }

    public override void Enter()
    {
        Boss.sp.color = Color.red;

        duration = Boss.SingleAttackDuration;
        t = 0;

        Boss.HitTargets();
    }
    public override void Leave()
    {
        base.Leave();

    }

    public CRT_State_SingleAttack(CRT_StateManager myStateManager, CRT_Boss myBoss) : base(myStateManager, myBoss)
    {

    }
}
