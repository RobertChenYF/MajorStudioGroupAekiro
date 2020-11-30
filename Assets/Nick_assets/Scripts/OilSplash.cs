using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSplash : MonoBehaviour
{

    public enum AnimationState { OilSplash, OilStatic, OilAway };
    private AnimationState currentAnimationState;
    public Animator OilAnimator;

    void Start()
    {
        PlayAnimation(AnimationState.OilSplash);
    }

    void Update()
    {

    }

    public void PlayAnimation(AnimationState animationState)
    {
        if (currentAnimationState != animationState)
        {
            OilAnimator.Play(animationState.ToString());
            currentAnimationState = animationState;
        }
    }

    public void OilStatic()
    {
        PlayAnimation(AnimationState.OilStatic);
    }
    public void OilAway()
    {
        PlayAnimation(AnimationState.OilAway);
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
