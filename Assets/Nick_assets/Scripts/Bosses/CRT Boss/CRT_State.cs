using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CRT_State
{
    protected CRT_StateManager SM;
    protected CRT_Boss Boss;
    protected string Name;
    protected float Duration;
    protected float PrepDuration;
    protected float Cooldown;
    protected bool isConsecutive;
    protected CRT_State Previous;

    public CRT_State(CRT_StateManager myStateManager, CRT_Boss thisBoss, string name, float dur, float prepDur, float cooldown, bool isConsec)
    {
        SM = myStateManager;
        Boss = thisBoss;
        Name = name;
        Duration = dur;
        PrepDuration = prepDur;
        Cooldown = cooldown;
        isConsecutive = isConsec;
    }

    public CRT_State(CRT_StateManager myStateManager, CRT_Boss thisBoss, string name, float dur)
    {
        SM = myStateManager;
        Boss = thisBoss;
        Name = name;
        Duration = dur;
    }

    public CRT_State(CRT_StateManager myStateManager, CRT_Boss thisBoss, string name, CRT_State previous)
    {
        SM = myStateManager;
        Boss = thisBoss;
        Name = name;
        Previous = previous;
    }

    public abstract void DoState(); // State behavior loop

    public string GetName()
    {
        return Name;
    }
    public float GetDuration()
    {
        return Duration;
    }
    public float GetPrepDuration()
    {
        return PrepDuration;
    }
    public float GetCooldown()
    {
        return Cooldown;
    }
    public CRT_State GetPrevious()
    {
        return Previous;
    }
    public void SetPrevious(CRT_State state)
    {
        Previous = state;
    }
    public bool IsConsec()
    {
        return isConsecutive;
    }

    public virtual void Enter()
    {

    }
    public virtual void Leave()
    {

    }
}
