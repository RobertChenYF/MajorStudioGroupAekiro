using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Boss : MonoBehaviour
{
    public Transform BossTransform;

    [HideInInspector]
    public SpriteRenderer sp;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public List<Location> targets;
    [HideInInspector]
    public Player player;

    Tank_StateManager Manager;

    public bool isUp;
    public Location[] PlayerLocations;
    [HideInInspector]
    public Location pLocA, pLocB, pLocC, pLocD;
    public Location_Boss BossLocA, BossLocB, BossLocC, BossLocD, BossLocAB, BossLocCD;
    [HideInInspector]
    public Location_Boss[] BossLocations;
    public Location_Boss Off_A, Off_B, Off_C, Off_D;
    [HideInInspector]
    public Location_Boss currentBossLocation, targetBossLocation;

    [Header("Attributes")]
    public int CurrentPhase;
    public float CollisionRange = 1f;
    public float StunDuration = 2f;
    public int MaxHealth = 50;
    public int health;
    public int Phase2_Thresh;
    public bool isAlive;
    public bool CanBeAttacked;
    public bool lookingRight;

    [Header("IDLE")]
    public float TimeBetweenAttacks_1 = 2.5f;

    [Header("Prepare")]
    public float TimeBeforeDriveLaunch = 1.5f;

    [Header("DriveBy")]
    public float DriveSpeed_1 = 4f;
    public float DriveOffDelay_1 = 0.5f;
    private float DriveSpeed;
    private float DriveOffDelay;
    public float ArrivalThreshhold = 0.1f;
    public float DriveComboWaitDuration = 0.25f;
    public float DriveComboCounter;
    public float DriveComboNumber = 3f; // NUMBER OF "DRIVE BY'S" PER START
    public float HitDuration_Drive = 0.5f;
    [HideInInspector]
    public Location_Boss startDriveLoc, endDriveLoc;
    [HideInInspector]
    public bool isDriveBy, isDriving, isOffLeft;

    [Header("Reposition")]
    public float ReposDuration = 2f;
    public float ReposPrepDuration = 1f;

    [Header("AttackJump")]
    public float JumpPrepDuration_S = 0.5f;
    public float JumpDuration_S = 1f;
    public float JumpHitDuration_S = 0.2f;
    [Space(10)]
    public float JumpPrepDuration_M = 0.2f;
    public float JumpDuration_M = 0.75f;
    public float JumpHitDuration_M = 0.1f;
    [Space(10)]
    public float JumpReturnDuration = 0.25f;
    public AnimationCurve JumpCurve;
    public Vector3 Jump_LerpOffset;
    [HideInInspector]
    public int JumpAttacksCount = 1;

    [Header("Shoot Gun")]
    public float GunPrepDuration_S = 0.75f;
    public float GunHitDuration_S = 0.1f;
    public float GunRetDuration_S = 0.25f;
    [Space(10)]
    public float GunPrepDuration_M = 0.5f;
    public float GunHitDuration_M = 0.1f;
    public float GunRetDuration_M = 0.25f;
    //[HideInInspector]
    public int GunAttackCount = 1;

    [Header("Shoot Mortar")]
    public float MortarPrepDuration = 1f;
    public float MortarTimeBtwnShot = 1f;
    public float MortarRetDuration = 1f;
    public int MortarAttackCount = 1;
    public GameObject MortarTarget;
    public GameObject MortarShot;

    [Header("Shoot Oil")]
    public float OilPrepDur = 1f;
    public float OilHitDuration = 1f;
    public float OilRetDur = 1f;
    public float OilTimeBtwnShot = 0.25f;
    public GameObject OilShot;


    [Header("Sounds")]
    public AudioSource SoundSource;
    public AudioClip[] audioClips;

    private void Awake()
    {
        sp = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        Manager = this.GetComponent<Tank_StateManager>();
        player = FindObjectOfType<Player>();
        pLocA = PlayerLocations[0];
        pLocB = PlayerLocations[1];
        pLocC = PlayerLocations[2];
        pLocD = PlayerLocations[3];
        BossLocations = new Location_Boss[] { BossLocA, BossLocB, BossLocC, BossLocD, BossLocAB, BossLocCD };
        currentBossLocation = BossLocC;

        DriveSpeed = DriveSpeed_1;
        DriveOffDelay = DriveOffDelay_1;

        health = MaxHealth;
        Phase2_Thresh = MaxHealth / 2;
        isAlive = true;
    }

    public void PlaySound()
    {
        SoundSource.Play();
    }
    public void SetSound(int i)
    {
        SoundSource.clip = audioClips[i];
    }

    public void GetStunned()
    {
        Manager.ChangeState(Manager.Stunned);

    }

    public bool GetPlayerIsUp()
    {
        return player.isUp;
    }

    private void Update()
    {
        if (health > Phase2_Thresh)
            CurrentPhase = 1;
        else if (health <= Phase2_Thresh)
            CurrentPhase = 2;
        else
            isAlive = false;

        //Debug.Log(currentBossLocation.gameObject.name);
        UpdateLocationInfo();
        CheckCollideLocations();

        if (isDriveBy && startDriveLoc != null)
            DriveBy(startDriveLoc, endDriveLoc);
    }

    private void UpdateLocationInfo()
    {
        if (sp.flipX)
            lookingRight = false;
        else
            lookingRight = true;

            if (currentBossLocation.isUp)
            isUp = true;
        else
            isUp = false;


        if (player.currentLoc == player.locA)
        {
            if (currentBossLocation == BossLocA || currentBossLocation == BossLocAB)
                CanBeAttacked = true;
            else
                CanBeAttacked = false;
        }
        else if (player.currentLoc == player.locB)
        {
            if (currentBossLocation == BossLocB || currentBossLocation == BossLocAB)
                CanBeAttacked = true;
            else
                CanBeAttacked = false;
        }
        else if (player.currentLoc == player.locC)
        {
            if (currentBossLocation == BossLocC || currentBossLocation == BossLocCD)
                CanBeAttacked = true;
            else
                CanBeAttacked = false;
        }
        else if (player.currentLoc == player.locD)
        {
            if (currentBossLocation == BossLocD || currentBossLocation == BossLocCD)
                CanBeAttacked = true;
            else
                CanBeAttacked = false;
        }
    }

    public void LookLeft()
    {
        if (lookingRight)
            sp.flipX = true;
            //BossTransform.localScale = new Vector3(-BossTransform.localScale.x, BossTransform.localScale.y, BossTransform.localScale.z);
    }
    public void LookRight()
    {
        if (!lookingRight)
            sp.flipX = false;
            //BossTransform.localScale = new Vector3(-BossTransform.localScale.x, BossTransform.localScale.y, BossTransform.localScale.z);
    }

    public bool CheckCanMortar()
    {
        bool canFire = true;
        foreach (Location loc in PlayerLocations)
        {
            if (loc.isTargetMortar)
                canFire = false;
        }
        return canFire;
    }

    public void CheckCollideLocations()
    {
        foreach (Location loc in PlayerLocations)
        {
            if (Vector2.Distance(this.transform.position, loc.transform.position) < CollisionRange)
            {
                if(isDriving && loc.canBeHitDrive)
                    loc.HitDrive(HitDuration_Drive);
            }
        }

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
        foreach (Location l in targets)
        {
            l.Hit();
        }
    }

    public void ClearTargets()
    {
        foreach (Location l in targets)
        {
            l.Clear(); // Clear each target's "targeted" value
        }
        targets.Clear(); // Clear the list itself
    }

    public void ClearAllLocations()
    {
        foreach (Location l in PlayerLocations)
        {
            l.Clear(); // Clear each target's "targeted" value
        }
    }

    public void StartDriveBy(Location_Boss start, Location_Boss end)
    {
        isDriveBy = true;
        startDriveLoc = start;
        endDriveLoc = end;
        this.transform.position = start.transform.position;

        if (start.transform.position.x > end.transform.position.x)
            LookLeft();
        else
            LookRight();

    }

    private void DriveBy(Location_Boss start, Location_Boss end)
    {
        //this.transform.position = Vector2.MoveTowards(this.transform.position, end.transform.position, step);

        Vector2 direction = (end.transform.position - this.transform.position).normalized;
        rb.velocity = DriveSpeed * direction;

        if (Vector2.Distance(this.transform.position, end.transform.position) < ArrivalThreshhold)
        {
            StopDriveBy(end);
        }
    }

    private void StopDriveBy(Location_Boss end)
    {
        isDriveBy = false;
        startDriveLoc = null;
        endDriveLoc = null;
        currentBossLocation = end;
        this.transform.position = currentBossLocation.transform.position;
    }

    /*public void CreateMortarTarget(Vector3 loc)
    {
        Instantiate(MortarTarget, loc, Quaternion.identity);
    }*/

    public void ShootMortar(List<Location> locs)
    {
        StartCoroutine(ShootMortarCo(locs.Count, locs));
    }

    IEnumerator ShootMortarCo(int count, List<Location> locs)
    {
        foreach (Location loc in locs)
        {
            loc.TargetMortar();
            GameObject M = Instantiate(MortarShot, this.transform.position, Quaternion.identity) as GameObject;
            M.SendMessage("SetTargetLoc", loc);
            yield return new WaitForSeconds(MortarTimeBtwnShot / count);
        }

        /*for (int i = 0; i < count; i++)
        {
            Debug.Log(targets[count]);
            //GameObject M = Instantiate(MortarShot, this.transform.position, Quaternion.identity) as GameObject;
            //M.SendMessage("SetTargetLoc", targets[count]);
            yield return new WaitForSeconds(MortarShootDuration/count);
        }*/
        ClearTargets();
    }

    public void ShootOil(List<Location> locs)
    {
        StartCoroutine(ShootOilCo(locs.Count, locs));
    }

    IEnumerator ShootOilCo(int count, List<Location> locs)
    {
        foreach (Location loc in locs)
        {
            Debug.Log("FIRE OIL");

            GameObject O = Instantiate(OilShot, this.transform.position, Quaternion.identity) as GameObject;
            O.SendMessage("SetTargetLoc", loc);
            yield return new WaitForSeconds(OilTimeBtwnShot / count);
        }

        yield return null;
    }
}
