using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Boss : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sp;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public List<Location> targets;

    public bool isUp;
    public Location[] PlayerLocations;
    [HideInInspector]
    public Location pLocA, pLocB, pLocC, pLocD;
    public Location_Boss BossLocA, BossLocB, BossLocC, BossLocD, BossLocAB, BossLocCD;
    public Location_Boss Off_A, Off_B, Off_C, Off_D;
    [HideInInspector]
    public Location_Boss currentBossLocation, targetBossLocation;

    [Header("Attributes")]
    public float CollisionRange = 1f;

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
    public float DriveComboNumber = 3f;
    [HideInInspector]
    public Location_Boss startDriveLoc, endDriveLoc;
    [HideInInspector]
    public bool isDriveBy, isDriving, isOffLeft;

    [Header("AttackJump")]
    public float JumpPrepDuration = 0.5f;
    public float JumpDuration = 1f;
    public float JumpHitDuration = 0.2f;
    public float JumpReturnDuration = 0.25f;
    [HideInInspector]
    public bool isJumping = false;
    public AnimationCurve JumpCurve;
    public Vector3 LerpOffset;


    private void Awake()
    {
        sp = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        pLocA = PlayerLocations[0];
        pLocB = PlayerLocations[1];
        pLocC = PlayerLocations[2];
        pLocD = PlayerLocations[3];
        currentBossLocation = BossLocC;

        DriveSpeed = DriveSpeed_1;
        DriveOffDelay = DriveOffDelay_1;
    }

    private void Update()
    {
        //Debug.Log(currentBossLocation.gameObject.name);
        UpdateLocationInfo();
        CheckCollideLocations();

        if (isDriveBy && startDriveLoc != null)
            DriveBy(startDriveLoc, endDriveLoc);
    }

    public void UpdateLocationInfo()
    {
        if (currentBossLocation.isUp)
            isUp = true;
        else
            isUp = false;
    }

    public void CheckCollideLocations()
    {
        if (!isJumping) // does not collision check while Jump Attack
        {
            foreach (Location loc in PlayerLocations)
            {
                if (Vector2.Distance(this.transform.position, loc.transform.position) < CollisionRange)
                {
                    loc.Hit();
                }
                else
                {
                    if (loc.isHit)
                        loc.ClearHit();
                }
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

    public void StartDriveBy(Location_Boss start, Location_Boss end)
    {
        isDriveBy = true;
        startDriveLoc = start;
        endDriveLoc = end;
        this.transform.position = start.transform.position;

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

    /*public void DriveComboDelayStart(Location_Boss target)
    {
        StartCoroutine(DelayStartDrive(target, DriveOffDelay));
    }

    IEnumerator DelayStartDrive(Location_Boss target, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartDriveBy(currentBossLocation, target);
        isDriving = true;
    }*/
}
