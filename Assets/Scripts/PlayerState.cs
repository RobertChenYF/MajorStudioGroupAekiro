using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    // Start is called before the first frame update
    protected PlayerStateManager playerStateManager;
    


    public abstract void StateBehavior();
    public virtual void Enter()
    {

    } // Virtual so can be overriden in derived classes.
    public virtual void Leave()
    {


    }


    public PlayerState(PlayerStateManager thePlayerStateManager) // Constructor that takes an argument.
    {
       playerStateManager = thePlayerStateManager;  
    }
}
