using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_ShootGun : Tank_State
{
    private float t1, t2, t3;
    private bool prepPhase, hitPhase, retPhase;

    public override void DoState()
    {
        if (t1 < PrepDuration) // Prep Phase
        {
            t1 += Time.deltaTime;
        }
        else
        {
            if (prepPhase) // Prep Phase ends 
            {
                Boss.HitTargets();
                prepPhase = false;
                hitPhase = true;
                Boss.sp.color = Color.red;
            }
            else
            {
                if (t2 < HitDuration) // Hit Phase
                {
                    t2 += Time.deltaTime;
                }
                else
                {
                    if (hitPhase)
                    {
                        Boss.ClearTargets();
                        hitPhase = false;
                        retPhase = true;
                        Boss.sp.color = Color.green;
                    }
                    else
                    {
                        if (t3 < ReturnDuration)
                        {
                            t3 += Time.deltaTime;
                        }
                        else
                        {
                            if (Boss.GunAttackCount < NumAttacks)
                            {
                                Boss.GunAttackCount++;
                                SM.ChangeState(SM.Prepare);
                            }
                            else
                            {
                                Boss.GunAttackCount = 1;
                                SM.ChangeState(SM.Idle);
                            }
                        }
                    }
                }
            }
        }
    }

    public override void Enter()
    {
        prepPhase = true;
        hitPhase = false;
        retPhase = false;
        t1 = 0; t2 = 0; t3 = 0;

        // Aim the gun?
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_ShootGun(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myPrepDur, float myHitDur, float myRetDur, int numAtk) : base(myManager, myBoss, myName, myPrepDur, myHitDur, myRetDur, numAtk)
    { }
}
