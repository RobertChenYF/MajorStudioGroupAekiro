using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Location locA, locB, locC, locD;
    public SpriteRenderer sp;
    public SpriteRenderer damageIndicator;
    public SpriteRenderer deflectIndicator;
    private CRT_Boss Boss;

    [Header("Attributes")]
    public int MaxHealth;

    [Header("Dodging")]
    public float damageBufferDelay = 0.75f;
    public float DodgeSpeed = 10;
    public float FlipSpriteBuffer = 1.2f;

    [Header("Attacking")]
    public int damage_Light = 1;
    public float Light_buffer = 0.25f;
    public int damage_Heavy = 2;
    public float Heavy_buffer = 0.25f;

    [Header("Blocking")]
    public bool isBlocking = false;
    public bool isDeflecting = false;
    public bool perfectDeflect = false;
    public float deflectBufferDelay = 0.5f;

    [Header("Take Damage")]
    public int damageNormal = 2;
    public int damageBlocking = 1;


    [HideInInspector]
    public int health;
    [HideInInspector]
    public bool isRight, isUp;
    [HideInInspector]
    public bool active = true;
    [HideInInspector]
    public bool isAlive = true;

    private PlayerStateManager Manager;
    private Location currentLoc;
    private bool canDamage = true;
    private bool aimUp;

    void Start()
    {
        Manager = this.GetComponent<PlayerStateManager>();
        Boss = FindObjectOfType<CRT_Boss>();

        health = MaxHealth;
        currentLoc = locB;

        damageIndicator.enabled = false;
    }

    void Update()
    {
        isRight = currentLoc.isRight;
        isUp = currentLoc.isUp;
        currentLoc.isOccupied = true;

        if (Input.GetKey(KeyCode.UpArrow))
            aimUp = true;
        if (Input.GetKey(KeyCode.DownArrow))
            aimUp = false;

        if (health > 0)
        {
            UpdateMovement();
            UpdateDamaged();
        }
        else
        {
            isAlive = false;
            damageIndicator.enabled = true;
        }
    }

    public Location FindTargetLocation()
    {
        // Each location has 2 possible locations you can dodge to

        if (isRight && isUp) // D
        {
            if (!aimUp)
                return locB;
            else return locC;
        }
        else if (isRight && !isUp) // B
        {
            if (aimUp)
                return locD;
            else return locA;
        }
        else if (!isRight && isUp) // C
        {
            if (!aimUp)
                return locA;
            else return locD;
        }
        else //if (!isRight && !isUp) // A
        {
            if (aimUp)
                return locC;
            else return locB;
        }
    }

    public void Dodge(Location newLoc)
    {
        if (currentLoc != newLoc)
        {
            currentLoc.isOccupied = false;
            currentLoc = newLoc;
        }
    }

    private void UpdateMovement()
    {
        if (currentLoc == locC || currentLoc == locA)
        {
            if (Vector2.Distance(this.transform.position, currentLoc.transform.position) < FlipSpriteBuffer)
                sp.flipX = true;
        }
        else
        {
            if (Vector2.Distance(this.transform.position, currentLoc.transform.position) < FlipSpriteBuffer)
                sp.flipX = false;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, currentLoc.transform.position, Time.deltaTime * DodgeSpeed);
    }

    private void UpdateDamaged()
    {
        if (currentLoc.isHit)
        {
            if (isDeflecting && active)
            {
                StartCoroutine(Deflect());
            }
            else
            {
                if (isBlocking)
                    StartCoroutine(TakeDamage(damageBlocking));
                else
                    StartCoroutine(TakeDamage(damageNormal));
            }
        }
    }

    IEnumerator Deflect()
    {
        deflectIndicator.enabled = true;
        perfectDeflect = true;
        yield return new WaitForSeconds(deflectBufferDelay);
        deflectIndicator.enabled = false;

    }

    IEnumerator TakeDamage(int damage)
    {
        if (canDamage)
        {
            health -= damage;
            damageIndicator.enabled = true;
            active = false;
        }
        canDamage = false;
        yield return new WaitForSeconds(damageBufferDelay);
        damageIndicator.enabled = false;
        canDamage = true;
        active = true;
    }

    public void DealDamageLight()
    {
        StartCoroutine(DealDamage(damage_Light, Light_buffer));
    }

    public void DealDamageHeavy()
    {
        StartCoroutine(DealDamage(damage_Heavy, Heavy_buffer));
    }

    IEnumerator DealDamage(int damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        Boss.health -= damage;
    }
}
