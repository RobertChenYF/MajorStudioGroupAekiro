﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public SpriteRenderer sp;
    //[HideInInspector]
    public bool isTargeted, isHit;

    public bool isUp, isRight, isOccupied;
    public bool OccupiedByBoss;

    void Start()
    {
        sp.color = new Color(1, 1, 1, 0.25f);
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
}
