using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_PrepareAttack : CRT_State
{
    private float duration;
    private float t;
    //private int c; attackNum

    public override void DoState()
    {
        if (t < duration) // slight pause
        {
            Debug.Log("PREPARING ATTACK");
            t += Time.deltaTime;
        }
        else
        {
            // Determine which attack
            SM.ChangeState(SM.SingleAttack);
        }
    }

    private List<Temp_Loc> FindPlayer()
    {
        List<Temp_Loc> locs = new List<Temp_Loc>();
        for(int i = 0; i < Boss.locations.Length; i++)
        {
            if (Boss.locations[i].isOccupied)
            {
                locs.Add(Boss.locations[i]);
            }
        }
        return locs;
    }

    public override void Enter()
    {
        Boss.sp.color = Color.yellow;

        // DETERMINE WHICH ATTACK
        Boss.targets = FindPlayer();
        Boss.TargetLocations();

        //c = Random.Range(0, Boss.NumAttacks);


        duration = Boss.PrepDuration_Single;
        t = 0;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public CRT_State_PrepareAttack(CRT_StateManager myStateManager, CRT_Boss myBoss) : base(myStateManager, myBoss)
    {

    }
}
