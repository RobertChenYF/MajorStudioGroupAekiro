using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tank_State
{
    protected Tank_StateManager SM;
    protected Tank_Boss Boss;
    protected string Name;
    protected float Duration;
    protected float PrepDuration;
    protected float HitDuration;
    protected float ReturnDuration;

    public Tank_State(Tank_StateManager myManager, Tank_Boss thisBoss, string name, float dur)
    {
        SM = myManager;
        Boss = thisBoss;
        Name = name;
        Duration = dur;
    }

    public Tank_State(Tank_StateManager myManager, Tank_Boss thisBoss, string name, float prepDur, float dur, float hitDur, float retDur)
    {
        SM = myManager;
        Boss = thisBoss;
        Name = name;
        Duration = dur;
        PrepDuration = prepDur;
        HitDuration = hitDur;
        ReturnDuration = retDur;
    }

    public abstract void DoState(); // State Behavior Loop

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
    public float GetHitDuration()
    {
        return HitDuration;
    }
    public float GetReturnDuration()
    {
        return ReturnDuration;
    }

    public virtual void Enter()
    {

    }
    public virtual void Leave()
    {

    }
}
