using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CRT_State_Attack : CRT_State
{
    //private float t;
    private float timer;

    public override void DoState()
    {
        if (timer < 2.5f)
        {
            timer += Time.deltaTime;
        }
        else
            SM.ChangeState(SM.Idle);
    }

    public override void Enter()
    {
        Boss.sp.color = Color.red;
        timer = 0;

        /*if (isConsecutive)
        {
            //t = 0;
            timer = PrepDuration;
        }
        else*/
    }

    IEnumerator ConsecAttacks()
    {
        yield return new WaitForSeconds(1);
    }

    public override void Leave()
    {
        base.Leave();

    }

    public CRT_State_Attack(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, float myDur, float myPrepDur, float myCd, bool myBlock) :base(myStateManager, myBoss, myName, myDur, myPrepDur, myCd, myBlock)
    {

    }
}
