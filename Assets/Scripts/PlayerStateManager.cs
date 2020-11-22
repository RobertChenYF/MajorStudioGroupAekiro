using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState currentPlayerState;
    private Material BossMat;
    private ScreenShakeControl screenShakeControl;

    private float bossFlashAmount = 0;
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
    public KeyCode ControllerHitKey;
    public KeyCode ControllerDodgeKey;
    public KeyCode ControllerBlockKey;


    [Header("Hit Attack Parameter")]
    public float secondsBeforeStartChargingAttack;
    public float maximumSecondsHoldingCharge;
    public float hitPauseDuration;
    public float lightAttackScreenShakeDuration;
    public float lightAttackScreenShakeMagnitude;

    [Header("Block Parameter")]
    public float DelayBeforeDeflect;
    public float PerfectDeflectWindow;
    public float BlockRecover;
    public float BlockDamagePercentage;

    [Header("Dodge Parameter")]
    public float SecondsBeforeStartRolling;

    [Header("Roll Parameter")]
    public float RollTime;

    [Header("Get Hit Stun")]
    public float GetHitStunDuration;

    // Start is called before the first frame update
    void Start()
    {
        screenShakeControl = GameObject.Find("ScreenShakerManager").GetComponent<ScreenShakeControl>();
        currentLoc = locA;
        playerCurrentHealth = playerFullHealth;
        BossAnimation = Boss.gameObject.GetComponent<Animator>();
        BossMat = Boss.gameObject.GetComponent<SpriteRenderer>().material;
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

        BossMat.SetFloat("_flashAmount", bossFlashAmount);
        bossFlashAmount -= Time.deltaTime*2.0f;
        bossFlashAmount = Mathf.Max(0,bossFlashAmount);

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
        bossFlashAmount += 0.9f;
        ScreenShake(lightAttackScreenShakeDuration,lightAttackScreenShakeMagnitude,lightAttackScreenShakeMagnitude);
        ControllerRumble(0.10f,0.4f);
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
                //stun boss;
                HitPause = hitPauseDuration;
                return HitResult.Deflect;
            }
            else if (currentPlayerState.ToString() == "Blocking")
            {
                //play Block effect, health--
                playerCurrentHealth -= damage * BlockDamagePercentage;
                HitPause = hitPauseDuration;
                return HitResult.Block;
            }
            else
            {
                //play get hit effect, health--, interprut current state
                playerCurrentHealth -= damage;
                HitPause = hitPauseDuration;
                return HitResult.Land;
            }
        }
        
    }

    public void MoveTransform(float t, Location previous, Location newLoc)
    {
       t = Mathf.Min(1,t);
        
       PlayerTransform.transform.position = Vector3.Lerp(previous.gameObject.transform.position,newLoc.gameObject.transform.position,t);
    }

    public void flip()
    {
        PlayerTransform.transform.localScale = new Vector3(-PlayerTransform.transform.localScale.x, PlayerTransform.transform.localScale.y,PlayerTransform.transform.localScale.z);
    }

    public void ScreenShake(float duration, float XMag, float YMag)
    {
        if (screenShakeControl.shakeDuration > duration && screenShakeControl.shakeXMag > XMag && screenShakeControl.shakeYMag > YMag)
        {

        }
        else
        {
            screenShakeControl.shakeDuration = duration;
            screenShakeControl.shakeXMag = XMag;
            screenShakeControl.shakeYMag = YMag;
        }
        
    }
        
    public void ControllerRumble(float duration, float strength)
    {
        if (screenShakeControl.rumbleDuration < duration || screenShakeControl.rumbleIntensity < strength)
        {
            screenShakeControl.rumbleIntensity = strength;
            screenShakeControl.rumbleDuration = duration;
        }
    }
}
