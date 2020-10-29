﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CRT_State
{
    protected CRT_StateManager SM;
    protected CRT_Boss Boss;

    public CRT_State(CRT_StateManager myStateManager, CRT_Boss thisBoss)
    {
        SM = myStateManager;
        Boss = thisBoss;
    }

    public abstract void DoState(); // State behavior if NOT an attack

    public virtual void Enter()
    {

    }
    public virtual void Leave()
    {

    }
}
