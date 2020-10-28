using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_Boss : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sp;

    [Header("SingleAttack")]
    public float SingleAttackDuration = 1f;



    private void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
    }
}
