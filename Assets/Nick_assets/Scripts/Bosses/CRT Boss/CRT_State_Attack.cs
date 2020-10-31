using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CRT_State_Attack : CRT_State
{
    private float t;
    private float timer;

    public override void DoState()
    {
        /*if (isConsecutive)
        {
            if (t < timer) // slight pause
            {
                t += Time.deltaTime;
            }
            else
            {

            }
        }*/
    }

    public override void Enter()
    {
        Boss.sp.color = Color.red;

        if (isConsecutive)
        {
            t = 0;
            timer = PrepDuration;
        }
        else
            Boss.HitTargets(); // If not a consecutuve attack, 
    }

    IEnumerator ConcecAttacks()
    {
        yield return new WaitForSeconds(1);
    }

    public override void Leave()
    {
        base.Leave();

    }

    public CRT_State_Attack(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, float myDur, float myPrepDur, float myCd, bool myCons) :base(myStateManager, myBoss, myName, myDur, myPrepDur, myCd, myCons)
    {

    }
}
