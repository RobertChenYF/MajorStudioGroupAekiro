using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State_Reposition : Tank_State
{
    private float tPrep, t, r;
    private bool prepPhase;
    Location_Boss target;
    Location_Boss[] possibleLocs;
    Vector2 startPos, endPos;

    public override void DoState()
    {
        if (tPrep < PrepDuration)
        {
            tPrep += Time.deltaTime;
            // Rev up - prep
        }
        else
        {
            if (prepPhase)
            {
                prepPhase = false;
                //Boss.sp.color = Color.blue;
            }
            else
            {
                if (t < Duration)
                {
                    t += Time.deltaTime;
                    r += Time.deltaTime / Duration;
                    Boss.transform.position = Vector3.Lerp(startPos, endPos, r);
                }
                else
                {
                    // Back to Idle
                    // snap to new location
                    Boss.transform.position = endPos;
                    Boss.currentBossLocation = target;
                    SM.ChangeState(SM.Idle);
                }
            }
        }
    }

    public override void Enter()
    {
        Boss.SetSound(2);
        Boss.PlaySound();
        SM.PlayAnimation(Tank_StateManager.AnimationState.Tank_Driving);
        tPrep = 0; t = 0; r = 0;
        prepPhase = true;

        possibleLocs = GetPossibleLocs(Boss.currentBossLocation);
        target = possibleLocs[Random.Range(0, possibleLocs.Length)];

        startPos = Boss.currentBossLocation.transform.position;
        endPos = target.transform.position;
        Boss.isDriving = true;
    }

    private Location_Boss[] GetPossibleLocs(Location_Boss curr)
    {
        switch (curr.GetName())
        {
            case "A":
                return new Location_Boss[] { Boss.BossLocAB, Boss.BossLocB, Boss.BossLocC };
            case "B":
                return new Location_Boss[] { Boss.BossLocA, Boss.BossLocAB, Boss.BossLocD };
            case "C":
                return new Location_Boss[] { Boss.BossLocA, Boss.BossLocCD, Boss.BossLocD };
            case "D":
                return new Location_Boss[] { Boss.BossLocB, Boss.BossLocCD, Boss.BossLocC };
            case "AB":
                return new Location_Boss[] { Boss.BossLocA, Boss.BossLocB, Boss.BossLocCD };
            case "CD":
                return new Location_Boss[] { Boss.BossLocC, Boss.BossLocD, Boss.BossLocAB };
            default:
                return new Location_Boss[] { Boss.currentBossLocation };

        }
    }

    public override void Leave()
    {
        base.Leave();
        Boss.isDriving = false;
    }

    public Tank_State_Reposition(Tank_StateManager myManager, Tank_Boss myBoss, string myName, float myDur, float myPrepDur) : base(myManager, myBoss, myName, myDur, myPrepDur)
    { }
}
