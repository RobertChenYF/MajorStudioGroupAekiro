using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_Boss : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sp;
    [HideInInspector]
    public int NumAttacks = 4; // How many different attacks does this boss know --> currently 4
    [HideInInspector]
    public List<Temp_Loc> targets;

    public Temp_Loc[] locations;

    [Header("Idle")]
    public float TimeBetweenAttacks = 1.5f;

    [Header("PrepareAttack")]
    public float PrepDuration_Single = 1f;
    public float PrepDuration_Double = 1f;
    public float PrepDuration_All = 1.5f;
    public float PrepDuration_Spin = 1.5f;
    public float PrepDuration_Cross = 1.5f;
    public float PrepDuration_Cross_delay = 0.5f;

    [Header("SingleAttack")]
    public float SingleAttackDuration = 1f;

    Temp_Player player;

    private void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Temp_Player>();
    }

    public void TargetLocations()
    {
        foreach (Temp_Loc l in targets)
        {
            l.Target();
        }
    }

    public void HitTargets()
    {
        foreach (Temp_Loc l in targets)
        {
            l.Hit();
        }
    }

    public void HitTargets(float delay)
    {
        StartCoroutine(HitTargets_Co(delay));
    }

    public void ClearTargets()
    {
        foreach (Temp_Loc l in targets)
        {
            l.Clear();
        }
    }

    IEnumerator HitTargets_Co(float delay)
    {
        foreach (Temp_Loc l in targets)
        {
            l.Hit();
            yield return new WaitForSeconds(delay);
        }
    }
}
