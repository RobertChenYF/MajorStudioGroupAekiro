using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState currentPlayerState;
    
    public enum AnimationState {Idle, LightHit, Block, Roll, BlockPoint, BlockBackswing, ChargeHeavyHit, HeavyHit};
    private AnimationState currentAnimationState;
    public Animator mainCharacterAnimation;

    [Header("Input Key")]
    public KeyCode hitKey;
    public KeyCode dodgeKey;
    public KeyCode blockKey;



    [Header("Hit Attack Parameter")]
    public float secondsBeforeStartChargingAttack;
    public float maximumSecondsHoldingCharge;

    [Header("Block Parameter")]
    public float DelayBeforeDeflect;
    public float PerfectDeflectWindow;
    public float BlockRecover;

    [Header("Dodge Parameter")]
    public float SecondsBeforeStartRolling;
    

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(new Idle(this));
        currentAnimationState = AnimationState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerState.StateBehavior();
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
        }
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        if (currentPlayerState != null) currentPlayerState.Leave();
        currentPlayerState = newPlayerState;
        currentPlayerState.Enter();
    }

    public void PlayAnimation(AnimationState animationState)
    {
         if (currentAnimationState != animationState)
         {
            mainCharacterAnimation.Play(animationState.ToString());
            currentAnimationState = animationState;
         }
        
    }

    public void BackToIdle()
    {
        
        ChangeState(new Idle(this));
    }
}
