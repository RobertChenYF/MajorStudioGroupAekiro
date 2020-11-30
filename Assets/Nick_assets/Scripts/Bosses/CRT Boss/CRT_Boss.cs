using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_Boss : MonoBehaviour
{

    public PlayerStateManager playerStateManager;

    CRT_StateManager Manager;

    [HideInInspector]
    public SpriteRenderer sp;
    [HideInInspector]
    public int NumAttacks = 4; // How many different attacks does this boss know --> currently 4
    [HideInInspector]
    public List<Location> targets;

    private Transform targetTransform;
    public Transform transformDown;
    public Transform transformUp;
    public Transform transformCenter;
    public Transform transformLeft;
    public Transform transformRight;


    [Header("Attributes")]
    public int MaxHealth = 50;
    public float Phase2_Entry = 0.75f;
    public float Phase3_Entry = 0.4f;
    [HideInInspector]
    public int health;
    public Location[] locations;
    public int CurrentPhase = 1;
    private float Phase2_Thresh;
    private float Phase3_Thresh;
    [HideInInspector]
    public bool isAlive = true;

    [Header("Idle")]
    public float TimeBetweenAttacks_P1 = 2.5f;
    public float TimeBetweenAttacks_P2 = 2f;
    public float TimeBetweenAttacks_P3 = 1.5f;


    //[Header("PrepareAttack")]
    //public float PrepDuration_All = 1.5f;
    //public float PrepDuration_Spin = 1.5f;
    //public float PrepDuration_Cross = 1.5f;
    //public float PrepDuration_Cross_delay = 0.5f;
    //public float PrepDur_Combo_fast = 0.5f;
    //public float PrepDur_Combo_medium = 1f;
    //public float PrepDur_Combo_slow = 1.5f;

    [Header("SingleAttack")]
    public float SingleDuration_norm = 1f;
    public float SingleDuration_normPrep = 1f;
    public float SingleCooldown_norm = 0.25f;
    [Space(10)]
    public float SingleDuration_quick = 1f;
    public float SingleDuration_quickPrep = 1f;
    public float SingleCooldown_quick = 0.15f;
    [Space(10)]
    public float SingleDuration_long = 1f;
    public float SingleDuration_longPrep = 1f;
    public float SingleCooldown_long = 1f;

    [Header("DoubleAttack")]
    public float DoubleDuration_norm = 1.5f;
    public float DoubleDuration_normPrep = 1f;
    public float DoubleCooldown_norm = 0.5f;

    [Header("AllAttack")]
    public float AllDuration_norm = 1f;
    public float AllDuration_normPrep = 1f;
    public float AllCooldown_norm = 0.75f;

    [Header("Stun-Deflected")]
    public float StunnedDuration = 1f;

    [Header("Sounds")]
    public AudioSource SoundSource;
    public AudioClip[] audioClips;

    private void Awake()
    {
        sp = this.GetComponent<SpriteRenderer>();
        Manager = this.GetComponent<CRT_StateManager>();

        health = MaxHealth;

        Phase2_Thresh = MaxHealth * Phase2_Entry;
        Phase3_Thresh = MaxHealth * Phase3_Entry;

        //SoundSource.clip = audioClips[0];
        targetTransform = transformCenter;
    }

    public void SetSound(int i)
    {
        SoundSource.clip = audioClips[i];
    }
    public void PlaySound()
    {
        SoundSource.Play();
    }

    public void MoveToPlayer()
    {
        if (targets.Count > 0)
        {
            if (targets[0].transform.position.x < 0)
                targetTransform = transformLeft;
            else
                targetTransform = transformRight;
        }
    }
    public void MoveDown()
    {
        targetTransform = transformDown;
    }
    public void MoveCenter()
    {
        targetTransform = transformCenter;
    }
    public void MoveUp()
    {
        targetTransform = transformUp;
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetTransform.position, Time.deltaTime*5);
        
        if (health > Phase2_Thresh)
            CurrentPhase = 1;
        else if (health <= Phase2_Thresh && health > Phase3_Thresh)
            CurrentPhase = 2;
        else if (health <= Phase3_Thresh && health > 0)
            CurrentPhase = 3;
        else
            isAlive = false;

        Location Loc;
        foreach (Location l in locations)
        {
            if (l.isOccupied && !Manager.attackActive)
            {
                Loc = l;

                if (Loc.transform.position.x > 0)
                    sp.flipX = false;
                else
                    sp.flipX = true;
            }
        }        
    }

    public void GetStunned()
    {
        Manager.GetStunned();
    }

    public void TargetLocations()
    {
        foreach (Location l in targets)
        {
            l.Target();
        }
    }

    public void HitTargets()
    {
        if (targets.Count > 0)
        {
            foreach (Location l in targets)
            {
                l.Hit();
                //get hit function
                playerStateManager.getMeleeAttacked(l, 1);
            }
        }
    }

    public void HitTargets(float delay)
    {
        StartCoroutine(HitTargets_Co(delay));
    }

    public void ClearTargets()
    {
        foreach (Location l in targets)
        {
            l.Clear(); // Clear each target's "targeted" value
        }
        targets.Clear(); // Clear the list itself
    }

    IEnumerator HitTargets_Co(float delay)
    {
        foreach (Location l in targets)
        {
            l.Hit();
            playerStateManager.getMeleeAttacked(l,1);
            yield return new WaitForSeconds(delay);
        }
    }

    public void ScreenShakeBoss2(float duration1, float strength1)
    {
        playerStateManager.ScreenShake(duration1,strength1,strength1);
    }
}
