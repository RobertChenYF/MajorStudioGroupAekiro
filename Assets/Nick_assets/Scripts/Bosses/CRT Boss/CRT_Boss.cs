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
    public int CurrentPhase = 1;

    [Header("Idle")]
    public float TimeBetweenAttacks = 1.5f;

    //[Header("PrepareAttack")]
    //public float PrepDuration_All = 1.5f;
    //public float PrepDuration_Spin = 1.5f;
    //public float PrepDuration_Cross = 1.5f;
    //public float PrepDuration_Cross_delay = 0.5f;
    //public float PrepDur_Combo_fast = 0.5f;
    //public float PrepDur_Combo_medium = 1f;
    //public float PrepDur_Combo_slow = 1.5f;

    [Header("SingleAttack")]
    public float SingleDuration_norm = 1f;
    public float SingleDuration_normPrep = 1f;
    public float SingleCooldown_norm = 0.25f;
    [Space(10)]
    public float SingleDuration_quick = 1f;
    public float SingleDuration_quickPrep = 1f;
    public float SingleCooldown_quick = 0.15f;
    [Space(10)]
    public float SingleDuration_long = 1f;
    public float SingleDuration_longPrep = 1f;
    public float SingleCooldown_long = 1f;

    [Header("DoubleAttack")]
    public float DoubleDuration_norm = 1.5f;
    public float DoubleDuration_normPrep = 1f;
    public float DoubleCooldown_norm = 0.5f;

    [Header("AllAttack")]
    public float AllDuration_norm = 1f;
    public float AllDuration_normPrep = 1f;
    public float AllCooldown_norm = 0.75f;

    Temp_Player player;

    private void Awake()
    {
        sp = this.GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Temp_Player>();
    }

    private void Update()
    {
        // TEMP TEMP TEMP -- PHASES
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CurrentPhase = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            CurrentPhase = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            CurrentPhase = 3;
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
            l.Clear(); // Clear each target's "targeted" value
        }
        targets.Clear(); // Clear the list itself
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
