using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState currentPlayerState;

    [SerializeField]private GameObject PlayerTransform;
    [SerializeField]private ParticleSystem Spark;
    [SerializeField]private Animator ImpactRing;
    [SerializeField]private CRT_Boss Boss;

    [SerializeField]private Image playerHealthBar;
    public Location locA, locB, locC, locD;
    [HideInInspector]public Location currentLoc;
    [HideInInspector]public Location previousLoc;
    public enum AnimationState {Idle, LightHit, Block, Roll, BlockPoint, BlockBackswing, ChargeHeavyHit, HeavyHit};
    public enum HitResult {Land, Miss, Block, Deflect};

    private AnimationState currentAnimationState;
    public Animator mainCharacterAnimation;
    private Animator BossAnimation;

    private float HitPause;
    private float playerCurrentHealth;

    [Header("Player Stats")]
    public float playerFullHealth;
    public float playerLightHitDamage;


    [Header("Input Key")]
    public KeyCode hitKey;
    public KeyCode dodgeKey;
    public KeyCode blockKey;



    [Header("Hit Attack Parameter")]
    public float secondsBeforeStartChargingAttack;
    public float maximumSecondsHoldingCharge;
    public float hitPauseDuration;

    [Header("Block Parameter")]
    public float DelayBeforeDeflect;
    public float PerfectDeflectWindow;
    public float BlockRecover;

    [Header("Dodge Parameter")]
    public float SecondsBeforeStartRolling;

    [Header("Roll Parameter")]
    public float RollTime;

    // Start is called before the first frame update
    void Start()
    {
        currentLoc = locA;
        playerCurrentHealth = playerFullHealth;
        BossAnimation = Boss.gameObject.GetComponent<Animator>();
        ChangeState(new Idle(this));
        currentAnimationState = AnimationState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerState.StateBehavior();
        playerHealthBar.fillAmount = playerCurrentHealth / playerFullHealth;
        if (HitPause > 0)
        {
            HitPause -= Time.deltaTime;
            mainCharacterAnimation.speed = 0;
            BossAnimation.speed = 0;
        }
        else
        {
            mainCharacterAnimation.speed = 1;
            BossAnimation.speed = 1;
        }
        //Debug.Log(currentPlayerState.ToString());
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

    public PlayerState getCurrentPlayerState()
    {
        return currentPlayerState;
    }

    public HitResult MeleeAttack(float damage)
    {
        Boss.health -= (int)playerLightHitDamage;
        HitPause = hitPauseDuration;
        //particle       
        Spark.Emit(100);
        //impact ring
        ImpactRing.SetTrigger("Hit");
        return HitResult.Land;
    }

    public HitResult getMeleeAttacked(Location hitLocation, float damage)
    {
        if (hitLocation != currentLoc)
        {
            return HitResult.Miss;
        }
        else
        {
            if (currentPlayerState.ToString() == "Deflect")
            {
                //play deflect effect here
                HitPause = hitPauseDuration;
                return HitResult.Deflect;
            }
            else if (currentPlayerState.ToString() == "Blocking")
            {
                //play Block effect, health--

                HitPause = hitPauseDuration;
                return HitResult.Block;
            }
            else
            {
                //play get hit effect, health--, interprut current state
                playerCurrentHealth -= 10;
                HitPause = hitPauseDuration;
                return HitResult.Land;
            }
        }
        
    }

    public void MoveTransform(float t, Location previous, Location newLoc)
    {
        t = Mathf.Min(1,t);
        Debug.Log(t);
       PlayerTransform.transform.position = Vector3.Lerp(previous.gameObject.transform.position,newLoc.gameObject.transform.position,t);
    }

    public void flip()
    {
        PlayerTransform.transform.localScale = new Vector3(-PlayerTransform.transform.localScale.x, PlayerTransform.transform.localScale.y,PlayerTransform.transform.localScale.z);
    }
}
