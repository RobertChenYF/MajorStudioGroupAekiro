using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Loc : MonoBehaviour
{
    SpriteRenderer sp;
    [HideInInspector]
    public bool isOccupied, isTargeted, isHit;

    public bool isUp;
    public bool isRight;

    void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        sp.color = Color.white;
    }

    public void Target()
    {
        sp.color = Color.yellow;
        isTargeted = true;
    }

    public void Hit()
    {
        sp.color = Color.red;
        isHit = true;
    }

    public void Clear()
    {
        sp.color = Color.white;
        isTargeted = false;
        isHit = false;
    }
}
