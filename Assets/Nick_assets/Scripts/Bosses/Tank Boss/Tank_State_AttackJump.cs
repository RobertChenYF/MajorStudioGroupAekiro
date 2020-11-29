using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_AttackJump : Tank_State
{
    float t0, t1, t2, t3;
    float r1, r2;
    bool prepPhase, jumpPhase, hitPhase, retPhase;
    Location target;
    Location_Boss endLocation;

    Vector2 startPos;
    Vector2 targetPos;
    Vector2 endPos;

    public override void DoState()
    {
        if (t0 < PrepDuration)
        {
            t0 += Time.deltaTime;
            // Prepping
            // Still yellow
        }
        else
        {
            if (prepPhase)
            {
                prepPhase = false;
                jumpPhase = true;
                Boss.sp.color = Color.blue;
            }
            else
            {
                if (t1 < Duration)
                {
                    t1 += Time.deltaTime;
                    // Jumping
                    float lerpRatio = t1 / Duration;
                    Vector2 positionOffset = Boss.JumpCurve.Evaluate(lerpRatio) * Boss.Jump_LerpOffset;
                    Boss.transform.position = Vector2.Lerp(startPos, targetPos, lerpRatio) + positionOffset;
                }
                else
                {
                    if (jumpPhase)
                    {
                        Boss.transform.position = targetPos;
                        Boss.HitTargets();
                        jumpPhase = false;
                        hitPhase = true;
                        Boss.sp.color = Color.red;
                    }
                    else
                    {
                        if (t2 < HitDuration)
                        {
                            t2 += Time.deltaTime;
                            // Hitting
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
                                // Jump 3 times
                                if (Boss.JumpAttacksCount < NumAttacks)
                                {
                                    Boss.JumpAttacksCount++;
                                    SM.ChangeState(SM.Prepare);
                                }
                                else
                                    EndAttack();

                            }
                        }
                    }
                }
            }
        }
    }

    private void EndAttack()
    {
        if (t3 < ReturnDuration)
        {
            t3 += Time.deltaTime;
            // Returning
            r2 += Time.deltaTime / ReturnDuration;
            Boss.transform.position = Vector3.Lerp(targetPos, endPos, r2);
        }
        else
        {
            // Back to Idle
            Boss.transform.position = endPos;
            Boss.currentBossLocation = endLocation;
            Boss.JumpAttacksCount = 1;
            SM.ChangeState(SM.Idle);
        }
    }

    private Location_Boss FindEndLocation(Location target)
    {
        if (target == Boss.PlayerLocations[0])
        {
            return Boss.BossLocA;
        }
        else if (target == Boss.PlayerLocations[1])
        {
            return Boss.BossLocB;
        }
        else if (target == Boss.PlayerLocations[2])
        {
            return Boss.BossLocC;
        }
        else if (target == Boss.PlayerLocations[3])
        {
            return Boss.BossLocD;
        }
        else
        {
            return Boss.BossLocCD;
        }
    }

    public override void Enter()
    {
        prepPhase = true;
        jumpPhase = false;
        hitPhase = false;
        retPhase = false;
        t0 = 0; t1 = 0; t2 = 0; t3 = 0;
        r1 = 0; r2 = 0;

        target = Boss.targets[0];
        endLocation = FindEndLocation(target);

        startPos = Boss.transform.position;
        targetPos = target.transform.position;
        endPos = endLocation.transform.position;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public Tank_State_AttackJump(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myPrepDur, float myDur, float myHitDur, float myRetDur, int numAtk) : base(myManager, myBoss, myName, myPrepDur, myDur, myHitDur, myRetDur, numAtk)
    { }
}
