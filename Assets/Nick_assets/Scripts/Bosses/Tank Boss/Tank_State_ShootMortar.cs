using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_ShootMortar : Tank_State
{
    private bool prepPhase, shootPhase, returnPhase;
    private float t1, t2, t3;
    private float StartingHitDuration;

    public override void DoState()
    {
        if (t1 < PrepDuration) // prep timer - aim
        {
            t1 += Time.deltaTime;
        }
        else
        {
            if (prepPhase)
            {
                prepPhase = false;
                shootPhase = true;
                Boss.sp.color = Color.red;
                // SHOOT AT THE TARGETS
                Boss.ShootMortar(Boss.targets);
            }
            else
            {
                if (t2 < HitDuration) // shoot duration
                {
                    t2 += Time.deltaTime;
                }
                else
                {
                    if (shootPhase)
                    {
                        Boss.sp.color = Color.green;
                        shootPhase = false;
                        returnPhase = true;
                    }
                    else
                    {
                        if (t3 < ReturnDuration) // Return phase
                        {
                            t3 += Time.deltaTime;
                        }
                        else
                        {
                            SM.ChangeState(SM.Idle);
                        }
                    }
                }
            }
        }
    }

    public override void Enter()
    {
        HitDuration = HitDuration * NumAttacks;

        t1 = 0; t2 = 0; t3 = 0;
        prepPhase = true; shootPhase = false; returnPhase = false;

        // DETERMINE TARGET LOCATIONS

        // PLACE TARGETS
    }

    public override void Leave()
    {
        base.Leave();
        HitDuration = StartingHitDuration;
    }

    public Tank_State_ShootMortar(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myPrepDur, float myHitDur, float myRetDur, int numAtk) : base(myManager, myBoss, myName, myPrepDur, myHitDur, myRetDur, numAtk)
    {
        StartingHitDuration = myHitDur;
    }
}
