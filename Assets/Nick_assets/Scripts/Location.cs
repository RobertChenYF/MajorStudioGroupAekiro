using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    PlayerStateManager player;

    public GameObject OilSplash_obj;
    public int damage = 10;

    public SpriteRenderer sp;
    public SpriteRenderer MortarTargetIndicator;
    //[HideInInspector]
    public bool isTargeted, isHit;
    public bool isTargetMortar;
    public bool canBeHitDrive = true;

    public bool isUp, isRight, isOccupied;
    public bool OccupiedByBoss;

    public bool isOiled;
    bool canFire = true;
    bool doFire;
    public float fireDelay = 0.5f;
    public float fireDur = 0.5f;
    public float OilDuration = 2f;


    void Start()
    {
        player = FindObjectOfType<PlayerStateManager>();

        sp.color = new Color(1, 1, 1, 0.25f);
        if (MortarTargetIndicator != null)
        {
            MortarTargetIndicator.enabled = false;
        }
    }

    private void Update()
    {
        if (isOiled && isOccupied && canFire)
        {
            canFire = false;
            StartCoroutine(SetFire());
        }
    }

    public void Target()
    {
        sp.color = new Color(1, 1, 0, 0.5f);
        isTargeted = true;
    }

    public void Hit()
    {
        sp.color = new Color(1, 0, 0, 0.75f);
        isHit = true;
        if (isOccupied)
            player.getMeleeAttacked(this, damage);
    }

    public void ClearHit()
    {
        sp.color = new Color(1, 1, 1, 0.25f);
        isHit = false;

    }

    public void Clear()
    {
        sp.color = new Color(1, 1, 1, 0.25f);
        isTargeted = false;
        isHit = false;
    }

    // MORTAR TARGET
    public void TargetMortar()
    {
        MortarTargetIndicator.enabled = true;
        isTargetMortar = true;
    }
    public void ClearTargetMortar()
    {
        MortarTargetIndicator.enabled = false;
        isTargetMortar = false;
    }
    public void HitMortar(float delay)
    {
        StartCoroutine(HitMortarDelay(delay));
    }
    IEnumerator HitMortarDelay(float delay)
    {
        ClearTargetMortar();
        Hit();
        yield return new WaitForSeconds(delay);
        ClearHit();
    }
    // DRIVE HIT
    public void HitDrive(float delay)
    {
        canBeHitDrive = false;
        StartCoroutine(HitDriveDelay(delay));
    }
    IEnumerator HitDriveDelay(float delay)
    {
        Hit();
        yield return new WaitForSeconds(delay);
        ClearHit();
        canBeHitDrive = true;
    }

    // OIL
    public void OilOn()
    {
        isOiled = true;
        Vector2 loc = new Vector2(this.transform.position.x, this.transform.position.y - 1);
        Instantiate(OilSplash_obj, loc, Quaternion.identity);
        StartCoroutine(OilTimer());
    }
    public void ClearOil()
    { 
        isOiled = false;
    }
    IEnumerator OilTimer()
    {
        yield return new WaitForSeconds(OilDuration);

        if (isOiled && !doFire)
            ClearOil();
    }
    IEnumerator SetFire()
    {
        doFire = true;
        yield return new WaitForSeconds(fireDelay);
        // FIRE
        Hit();
        yield return new WaitForSeconds(fireDur);
        ClearHit();
        ClearOil();
        doFire = false;

    }


}
