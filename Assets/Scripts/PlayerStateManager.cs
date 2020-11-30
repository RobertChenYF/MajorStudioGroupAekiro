using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState currentPlayerState;
    public SpriteRenderer BossSpriteRenderer;
    private Material BossMat;
    [SerializeField]private GameObject DeathScreen;
    [SerializeField] private AudioSource playerAudioSource;
    [HideInInspector] public Material characterMaterial;
    [HideInInspector] public float swordGlowAmount = 1.1f;
    private Color defaultSwordColor;
    private Color defaultSwordMidColor;
    private ScreenShakeControl screenShakeControl;

    private float bossFlashAmount = 0;
    [SerializeField] private GameObject PlayerTransform;
    [SerializeField] private ParticleSystem Spark;
    [SerializeField] private Animator ImpactRing;
    [SerializeField] public CRT_Boss Boss1;
    [SerializeField] public Tank_Boss Boss2;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip deflectSound;
    [SerializeField] private AudioClip blockSound;
    [SerializeField] private AudioClip getHitSound;
    [SerializeField] private AudioClip landHitSound;


    [SerializeField]private Image playerHealthBar;
    public Location locA, locB, locC, locD;
    public Location currentLoc;
    [HideInInspector]public Location previousLoc;
    public enum AnimationState {Idle, LightHit, Block, Roll, BlockPoint, BlockBackswing, ChargeHeavyHit, HeavyHit, Shuffle, Stun};
    public enum HitResult {Land, Miss, Block, Deflect};

    private AnimationState currentAnimationState;
    public Animator mainCharacterAnimation;
    public Animator BossAnimation;

    private float HitPause;
    private float playerCurrentHealth;
    [HideInInspector]public float heavyHitDamage;

    [Header("Player Stats")]
    public float playerFullHealth;
    public float playerLightHitDamage;
    public float playerHeavyHitDamageIncreasePerSecond;


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

    private bool canHitBoss;
    [HideInInspector]
    public bool lookingRight, isDodging;

    // Start is called before the first frame update
    void Start()
    {
        characterMaterial = GetComponentsInChildren<SpriteRenderer>()[0].material;
        defaultSwordColor = characterMaterial.GetColor("_SwordColor");
        screenShakeControl = GameObject.Find("ScreenShakerManager").GetComponent<ScreenShakeControl>();
        currentLoc = locA;
        currentLoc.isOccupied = true;
        playerCurrentHealth = playerFullHealth;
        //BossAnimation = Boss.gameObject.GetComponent<Animator>();
        BossMat = BossSpriteRenderer.material;
        ChangeState(new Idle(this));
        currentAnimationState = AnimationState.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerCurrentHealth<=0&&currentPlayerState.ToString() != "Death")
        {
            ChangeState(new Death(this));
        }
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
        swordGlowAmount -= Time.deltaTime * 1.3f;
        swordGlowAmount = Mathf.Max(1.1f,swordGlowAmount);
        characterMaterial.SetColor("_SwordColor", defaultSwordColor*swordGlowAmount);

        if (Boss2 != null)
        {
            if(!isDodging)
            LookAtBoss();

            if (Boss2.CanBeAttacked)
                canHitBoss = true;
            else
                canHitBoss = false;
        }
        if (Boss1 != null)
            canHitBoss = true;

    }

    private void LookAtBoss()
    {
        if (PlayerTransform.transform.localScale.x < 0)
            lookingRight = true;
        else
            lookingRight = false;

        if (Boss2.transform.position.x < this.transform.position.x && lookingRight)
            flip();
        else if (Boss2.transform.position.x > this.transform.position.x && !lookingRight)
            flip();
            
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
        if (Boss1 != null)
            Boss1.health -= (int)playerLightHitDamage;
        if (Boss2 != null && Boss2.CanBeAttacked)
            Boss2.health -= (int)playerLightHitDamage;

        if (canHitBoss)
        {
            HitPause = hitPauseDuration;
            //particle       
            Spark.Emit(100);
            bossFlashAmount += 0.9f;
            ScreenShake(lightAttackScreenShakeDuration, lightAttackScreenShakeMagnitude, lightAttackScreenShakeMagnitude);
            ControllerRumble(0.10f, 0.4f);
            swordGlowAmount += 0.8f;
            //impact ring
            ImpactRing.SetTrigger("Hit");
            PlayPlayerSound(landHitSound, Random.Range(0.8f, 1.2f), 0.7f);
            return HitResult.Land;
        }
        else
            return HitResult.Miss;
    }

    public HitResult HeavyMeleeAttack()
    {
        if (Boss1 != null)
            Boss1.health -= Mathf.RoundToInt(heavyHitDamage);
        if (Boss2 != null && Boss2.CanBeAttacked)
            Boss2.health -= Mathf.RoundToInt(heavyHitDamage);

        if (canHitBoss)
        {
            float a = ((heavyHitDamage - playerLightHitDamage) / playerHeavyHitDamageIncreasePerSecond) + 1;
            HitPause = hitPauseDuration * a;
            Spark.Emit((int)(100 * a));
            bossFlashAmount += 0.9f;
            ScreenShake(lightAttackScreenShakeDuration, lightAttackScreenShakeMagnitude * a, lightAttackScreenShakeMagnitude * a);
            ControllerRumble(0.15f, 0.4f * a);
            swordGlowAmount += 0.8f;
            PlayPlayerSound(landHitSound, Random.Range(0.8f, 1.2f), 0.9f);
            //impact ring
            ImpactRing.SetTrigger("Hit");
            return HitResult.Land;
        }
        else
            return HitResult.Miss;
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
                
                //
                Debug.Log("deflect Succefully");
                Spark.Emit(150);
                swordGlowAmount += 1;
                ScreenShake(0.1f,0.2f,0.2f);
                ControllerRumble(0.15f, 0.4f);
                PlayPlayerSound(deflectSound,Random.Range(0.9f,1.1f),1);
                HitPause = hitPauseDuration;
                //if (Boss1 != null)
                   // Boss1.GetStunned();
               // if (Boss2 != null)
                   // Boss2.GetStunned();
                return HitResult.Deflect;
                
            }
            else if (currentPlayerState.ToString() == "Blocking")
            {
                //play Block effect, health--
                Spark.Emit(50);
                Debug.Log("block Succefully");
                playerCurrentHealth -= damage * BlockDamagePercentage;
                PlayPlayerSound(blockSound, Random.Range(0.9f, 1.1f), 0.7f);
                ScreenShake(0.13f, 0.3f, 0.3f);
                HitPause = hitPauseDuration;
                return HitResult.Block;
            }
            else
            {
                //play get hit effect, health--, interprut current state
                playerCurrentHealth -= damage;
                PlayPlayerSound(getHitSound, Random.Range(0.9f, 1.1f), 0.7f);
                ScreenShake(0.13f, 0.3f, 0.3f);
                HitPause = hitPauseDuration;
                ChangeState(new Stun(this));
                return HitResult.Land;
            }
        }
        
    }

    public void MoveTransform(float t, Location previous, Location newLoc)
    {
       t = Mathf.Min(1,t);
        
       PlayerTransform.transform.position = Vector3.Lerp(previous.gameObject.transform.position,newLoc.gameObject.transform.position,t);
        previous.isOccupied = false;
        newLoc.isOccupied = true;
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

    public void ChargeAttack()
    {
        swordGlowAmount += Time.deltaTime * 2.1f;
        heavyHitDamage += Time.deltaTime * playerHeavyHitDamageIncreasePerSecond;

    }
    public void PlayPlayerSound(AudioClip clip,float pitch,float volume)
    {
        playerAudioSource.pitch = pitch;
        playerAudioSource.volume = volume;
        playerAudioSource.PlayOneShot(clip);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TurnOnDeathScreen()
    {
        DeathScreen.SetActive(true);
    }
}
