using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentPlayerStates : MonoBehaviour
{

}


public class Idle : PlayerState
{
    public Idle(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        
    }

    public override void Enter()
    {
        base.Enter();


    }
    public override void Leave()
    {
        base.Leave();

    }
}
