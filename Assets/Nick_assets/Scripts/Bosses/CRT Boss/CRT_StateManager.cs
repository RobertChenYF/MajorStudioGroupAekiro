using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_StateManager : MonoBehaviour
{
    [SerializeField]
    public CRT_State currentState;
    protected static CRT_StateManager Main_SM;
    protected static CRT_Boss Boss;
    protected static Player player;

    public CRT_State Idle;
    public CRT_State PrepareAttack;
    public CRT_State SingleAttack_normal;
    public CRT_State SingleAttack_quick;
    public CRT_State SingleAttack_long;
    public CRT_State DoubleAttack_hoz;
    public CRT_State DoubleAttack_ver;
    public CRT_State AllAttack_simul;

    public CRT_State ComboState;
    public CRT_State[] Attack_1;
    public CRT_State[] Attack_2_h;
    public CRT_State[] Attack_2_v;
    public CRT_State[] Attack_3;
    public CRT_State[] Combo_1;
    public CRT_State[] Combo_2;
    public CRT_State[] Combo_3;
    public CRT_State[] Combo_4;
    public CRT_State[] Combo_5;
    public CRT_State[] Combo_6;
    public CRT_State[] Combo_7;
    public CRT_State[] Combo_8;

    public CRT_State[] attackStateList;
    public CRT_State[][] MovesList_1;
    public CRT_State[][] MovesList_2;
    public CRT_State[][] MovesList_3;



    [HideInInspector]
    public bool comboActive;
    [HideInInspector]
    public int comboStep, comboLength;
    public CRT_State comboNextState;

    private void Start()
    {
        Main_SM = this;
        Boss = this.GetComponent<CRT_Boss>();
        player = FindObjectOfType<Player>();

        Idle = new CRT_State_Idle(Main_SM, Boss, "Idle", Boss.TimeBetweenAttacks);
        PrepareAttack = new CRT_State_PrepareAttack(Main_SM, Boss, "Prepare", 0);
        SingleAttack_normal = new CRT_State_Attack(Main_SM, Boss, "Single", Boss.SingleDuration_norm, Boss.SingleDuration_normPrep, Boss.SingleCooldown_norm, false);
        SingleAttack_quick = new CRT_State_Attack(Main_SM, Boss, "Single", Boss.SingleDuration_quick, Boss.SingleDuration_quickPrep, Boss.SingleCooldown_quick, false);
        SingleAttack_long = new CRT_State_Attack(Main_SM, Boss, "Single", Boss.SingleDuration_long, Boss.SingleDuration_longPrep, Boss.SingleCooldown_long, false);
        DoubleAttack_hoz = new CRT_State_Attack(Main_SM, Boss, "DoubleHoz", Boss.DoubleDuration_norm, Boss.DoubleDuration_normPrep, Boss.DoubleCooldown_norm, false);
        DoubleAttack_ver = new CRT_State_Attack(Main_SM, Boss, "DoubleVer", Boss.DoubleDuration_norm, Boss.DoubleDuration_normPrep, Boss.DoubleCooldown_norm, false);
        AllAttack_simul = new CRT_State_Attack(Main_SM, Boss, "AllSimul", Boss.AllDuration_norm, Boss.AllDuration_normPrep, Boss.AllCooldown_norm, false);

        attackStateList = new CRT_State[] { SingleAttack_normal, DoubleAttack_hoz, DoubleAttack_ver, AllAttack_simul };

        ComboState = new CRT_State_Combo(Main_SM, Boss, "Combo", Idle);

        Attack_1 = new CRT_State[] { SingleAttack_normal };
        Attack_2_h = new CRT_State[] { DoubleAttack_hoz };
        Attack_2_v = new CRT_State[] { DoubleAttack_ver };
        Attack_3 = new CRT_State[] { AllAttack_simul };

        Combo_1 = new CRT_State[] { SingleAttack_normal, SingleAttack_normal, DoubleAttack_hoz };
        Combo_2 = new CRT_State[] { SingleAttack_normal, SingleAttack_normal, SingleAttack_long };
        Combo_3 = new CRT_State[] { SingleAttack_normal, SingleAttack_quick, SingleAttack_quick, SingleAttack_long };
        Combo_4 = new CRT_State[] { DoubleAttack_hoz, DoubleAttack_hoz, AllAttack_simul };
        Combo_5 = new CRT_State[] { DoubleAttack_hoz, SingleAttack_quick, SingleAttack_quick, DoubleAttack_hoz, SingleAttack_quick, SingleAttack_quick };
        Combo_6 = new CRT_State[] { DoubleAttack_ver, DoubleAttack_ver, DoubleAttack_hoz, DoubleAttack_hoz, AllAttack_simul };
        Combo_7 = new CRT_State[] { DoubleAttack_ver, SingleAttack_quick, SingleAttack_quick, DoubleAttack_hoz, SingleAttack_long, SingleAttack_quick };
        Combo_8 = new CRT_State[] { AllAttack_simul, SingleAttack_long, SingleAttack_quick, SingleAttack_quick, DoubleAttack_hoz };

        // PHASE 1: MOVE LIST
        MovesList_1 = new CRT_State[][] { Attack_1, Attack_2_v, Combo_1, Combo_2 };
        // PHASE 2: MOVE LIST
        MovesList_2 = new CRT_State[][] { Attack_3, Combo_1, Combo_2, Combo_3, Combo_4 };
        // PHASE 2: MOVE LIST
        MovesList_3 = new CRT_State[][] { Combo_4, Combo_5, Combo_6, Combo_7, Combo_8 };

        ChangeState(Idle); // Starting state is idle
    }

    private void Update()
    {
        if(Boss.isAlive)
            currentState.DoState();

        /*if (player.perfectDeflect)
        {
            StopAllCoroutines();
            player.PerfectDeflect(false);
            ChangeState(Idle);
        }*/
    }

    public void ChangeState(CRT_State newState)
    {
        if (currentState != null) currentState.Leave();
        currentState = newState;
        currentState.Enter();
    }

    public void PerformCombo(CRT_State[] attackList)
    {
        StartCoroutine(Combo(attackList));
    }

    IEnumerator Combo(CRT_State[] attacks)
    {
        comboStep = 0;
        comboLength = attacks.Length;
        comboActive = true;

        foreach (CRT_State state in attacks)
        {
            CRT_State S = attacks[comboStep];

            comboNextState = S;
            ChangeState(PrepareAttack); // Take Aim
            yield return new WaitForSeconds(S.GetPrepDuration());
            ChangeState(S); // Attack!! 
            yield return new WaitForSeconds(S.GetDuration());
            ComboState.SetPrevious(S);
            ChangeState(ComboState);
            yield return new WaitForSeconds(ComboState.GetPrevious().GetCooldown());
            comboStep++;
        }
        //yield return new WaitForSeconds(attacks[comboStep - 1].GetCooldown());
        ChangeState(Idle);
        comboActive = false;
    }
}
