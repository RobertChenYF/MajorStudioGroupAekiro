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


    //[HideInInspector]
    public int health;
    [HideInInspector]
    public bool isRight, isUp;
    public bool active = true;

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
        /*if (currentLoc.isHit && isDeflecting)
        {
            StartCoroutine(Deflect());
        }*/

        if (currentLoc.isHit && !isBlocking && !isDeflecting)
        {
            StartCoroutine(TakeDamage());
        }
    }



    IEnumerator TakeDamage()
    {
        if (canDamage)
        {
            health--;
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
