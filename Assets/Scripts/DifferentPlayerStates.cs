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
        if(Input.GetKeyDown(playerStateManager.hitKey))
        {
            playerStateManager.ChangeState(new BeforeHitCharge(playerStateManager));
        }
        else if(Input.GetKeyDown(playerStateManager.dodgeKey))
        {
            playerStateManager.ChangeState(new ReadyToRoll(playerStateManager));
        }
        else if(Input.GetKeyDown(playerStateManager.blockKey))
        {
            playerStateManager.ChangeState(new StartBlocking(playerStateManager));
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.Idle);

    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class BeforeHitCharge : PlayerState
{
    public float timeHoldingAttackKey = 0;
    public BeforeHitCharge(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        timeHoldingAttackKey += Time.deltaTime;

        if (Input.GetKeyUp(playerStateManager.hitKey))
        {
            playerStateManager.ChangeState(new LightHit(playerStateManager));

        }
        else if (timeHoldingAttackKey >= playerStateManager.secondsBeforeStartChargingAttack)
        {
            playerStateManager.ChangeState(new HitCharge(playerStateManager));
        }
    }

    public override void Enter()
    {
        base.Enter();
        timeHoldingAttackKey = 0;

    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class HitCharge : PlayerState
{
    public static float chargeTime;
    public HitCharge(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        chargeTime += Time.deltaTime;
        Debug.Log("charge heavy attack");
        if (Input.GetKeyUp(playerStateManager.hitKey))
        {
            playerStateManager.ChangeState(new HeavyHit(playerStateManager));

        }
        else if (chargeTime >= playerStateManager.maximumSecondsHoldingCharge)
        {
            playerStateManager.ChangeState(new HeavyHit(playerStateManager));
        }
    }

    public override void Enter()
    {
        base.Enter();
        chargeTime = 0;
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.ChargeHeavyHit);
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class LightHit : PlayerState
{

    public LightHit(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        //back to idle after animation end


    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Light Hit");
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.LightHit);
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class HeavyHit : PlayerState
{

    public HeavyHit(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        //back to idle after animation end


    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Heavy Hit");
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.HeavyHit);
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class StartBlocking : PlayerState
{
    public float timer;
    public StartBlocking(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        timer += Time.deltaTime;
        if (timer >= playerStateManager.DelayBeforeDeflect)
        {
            playerStateManager.ChangeState(new Deflect(playerStateManager));
        }


    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Start Blocking");
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.BlockPoint);
        
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class Deflect : PlayerState
{
    public float timer;
    public Deflect(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        timer += Time.deltaTime;
        if (!Input.GetKey(playerStateManager.blockKey))
        {
            playerStateManager.ChangeState(new BlockRecover(playerStateManager));
        }
        if (timer >= playerStateManager.PerfectDeflectWindow)
        {
            playerStateManager.ChangeState(new Blocking(playerStateManager));
        }


    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Deflect");
        timer = 0;
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class Blocking : PlayerState
{
    
    public Blocking(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        if (!Input.GetKey(playerStateManager.blockKey))
        {
            playerStateManager.ChangeState(new BlockRecover(playerStateManager));
        }
        Debug.Log("blocking");

    }

    public override void Enter()
    {
        base.Enter();
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.Block);

    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class BlockRecover : PlayerState
{
    public float timer;
    public BlockRecover(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        

        if (timer > playerStateManager.BlockRecover)
        {
            playerStateManager.ChangeState(new Idle(playerStateManager));
        }
        timer += Time.deltaTime;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("block recover");
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.BlockBackswing);
        timer = 0;
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class ReadyToRoll : PlayerState
{
    public float timer;
    public ReadyToRoll(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyUp(playerStateManager.dodgeKey))
        {
            playerStateManager.ChangeState(new Shuffle(playerStateManager));
        }
        else if (timer >= playerStateManager.SecondsBeforeStartRolling)
        {
            playerStateManager.ChangeState(new Roll(playerStateManager));
        }
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0;
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class Roll : PlayerState
{

    public Roll(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Roll");
        playerStateManager.PlayAnimation(PlayerStateManager.AnimationState.Roll);
    }
    public override void Leave()
    {
        base.Leave();

    }

}

public class Shuffle : PlayerState
{

    public Shuffle(PlayerStateManager theGameStateManager) : base(theGameStateManager)
    {

    }

    public override void StateBehavior()
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Shuffle");
        playerStateManager.ChangeState(new Idle(playerStateManager));
    }
    public override void Leave()
    {
        base.Leave();

    }

}