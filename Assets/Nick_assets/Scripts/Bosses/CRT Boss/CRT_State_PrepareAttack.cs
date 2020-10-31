using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_State_PrepareAttack : CRT_State
{
    //private float t;

    CRT_State AttackToPerform;

    public override void DoState()
    {

    }

    private Temp_Loc FindPlayer()
    {
        Temp_Loc loc = null;
        for(int i = 0; i < Boss.locations.Length; i++)
        {
            if (Boss.locations[i].isOccupied)
            {
                loc = Boss.locations[i];
                //locs.Add(Boss.locations[i]);
            }
        }
        return loc;
    } // returns player's location

    private CRT_State GetNextComboStep() // get the next step in the active combo
    {
        return SM.comboNextState;
    }

    private List<Temp_Loc> DetermineTargets(CRT_State attack, Temp_Loc player) // aquire targets according to attack
    {
        List<Temp_Loc> targets = new List<Temp_Loc>();

        switch (attack.GetName())
        {
            case "Single":
                targets.Add(player);
                return targets;

            case "DoubleHoz":
                if (player.isUp) // if up
                {
                    for (int i = 0; i < Boss.locations.Length; i++)
                        if (Boss.locations[i].isUp)
                            targets.Add(Boss.locations[i]);
                }
                else // if down
                {
                    for (int i = 0; i < Boss.locations.Length; i++)
                        if (!Boss.locations[i].isUp)
                            targets.Add(Boss.locations[i]);
                }
                return targets;

            case "DoubleVer":
                if (player.isRight) // if right
                {
                    for (int i = 0; i < Boss.locations.Length; i++)
                        if (Boss.locations[i].isRight)
                            targets.Add(Boss.locations[i]);
                }
                else // if left
                {
                    for (int i = 0; i < Boss.locations.Length; i++)
                        if (!Boss.locations[i].isRight)
                            targets.Add(Boss.locations[i]);
                }
                return targets;

            case "AllSimul":
                foreach (Temp_Loc t in Boss.locations)
                    targets.Add(t);
                return targets;

            case "ASDF":
                return targets;

            case "asdf":
                return targets;

            default:
                return targets;
        }
    }

    /*private float DetermineDuration(CRT_State attack) // Set respective duration for the attack
    {
        switch (attack.GetName())
        {
            case "Single":
                if (SM.comboActive)
                    return Boss.PrepDur_Combo_fast;
                else
                    return Boss.PrepDuration_Single;
            case "DoubleHoz":
                if (SM.comboActive)
                    return Boss.PrepDur_Combo_medium;
                else
                    return Boss.PrepDuration_Double;
            case "DoubleVer":
                if (SM.comboActive)
                    return Boss.PrepDur_Combo_medium;
                else
                    return Boss.PrepDuration_Double;
            default:
                return 0;
        }
    }*/

    public override void Enter()
    {
        Boss.sp.color = Color.yellow;

        Temp_Loc playerLoc = FindPlayer(); // Find the location the player is occupying
        AttackToPerform = GetNextComboStep(); // Determine which attack to perform (random, currently)
        //Duration = DetermineDuration(AttackToPerform); // Determine duration of the current attack
        Boss.targets = DetermineTargets(AttackToPerform, playerLoc); // Determine the target locations based on the current attack
        Boss.TargetLocations(); // Target (visually indicate - yellow) those locations
    }

    public override void Leave()
    {
        base.Leave();
    }

    public CRT_State_PrepareAttack(CRT_StateManager myStateManager, CRT_Boss myBoss, string myName, float myDur) : base(myStateManager, myBoss, myName, myDur)
    { 
    }
}
