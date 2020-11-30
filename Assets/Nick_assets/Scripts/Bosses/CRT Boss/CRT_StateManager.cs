using System.Collections;
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
    public CRT_State Stunned;
    public CRT_State Dead;

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

    public enum AnimationState { CRT_Idle, CRT_Single_Front, CRT_Single_Back, CRT_Double_Front, CRT_Double_Back, CRT_Double_Vertical, CRT_All, CRT_Stun, CRT_Dead };
    private AnimationState currentAnimationState;
    public Animator myBossAnimator;

    [HideInInspector]
    public bool comboActive;
    [HideInInspector]
    public int comboStep, comboLength;
    public CRT_State comboNextState;
    [HideInInspector]
    public bool attackActive;


    private void Start()
    {
        Main_SM = this;
        Boss = this.GetComponent<CRT_Boss>();
        player = FindObjectOfType<Player>();

        Idle = new CRT_State_Idle(Main_SM, Boss, "Idle", Boss.TimeBetweenAttacks_P1);
        PrepareAttack = new CRT_State_PrepareAttack(Main_SM, Boss, "Prepare", 0);
        Stunned = new CRT_State_Stunned(Main_SM, Boss, "Stunned", Boss.StunnedDuration);
        Dead = new CRT_State_Dead(Main_SM, Boss, "Dead", 0);

        SingleAttack_normal = new CRT_State_Attack(Main_SM, Boss, "Single", Boss.SingleDuration_norm, Boss.SingleDuration_normPrep, Boss.SingleCooldown_norm, true);
        SingleAttack_quick = new CRT_State_Attack(Main_SM, Boss, "Single", Boss.SingleDuration_quick, Boss.SingleDuration_quickPrep, Boss.SingleCooldown_quick, true);
        SingleAttack_long = new CRT_State_Attack(Main_SM, Boss, "Single", Boss.SingleDuration_long, Boss.SingleDuration_longPrep, Boss.SingleCooldown_long, true);
        DoubleAttack_hoz = new CRT_State_Attack(Main_SM, Boss, "DoubleHoz", Boss.DoubleDuration_norm, Boss.DoubleDuration_normPrep, Boss.DoubleCooldown_norm, true);
        DoubleAttack_ver = new CRT_State_Attack(Main_SM, Boss, "DoubleVer", Boss.DoubleDuration_norm, Boss.DoubleDuration_normPrep, Boss.DoubleCooldown_norm, true);
        AllAttack_simul = new CRT_State_Attack(Main_SM, Boss, "AllSimul", Boss.AllDuration_norm, Boss.AllDuration_normPrep, Boss.AllCooldown_norm, true);

        attackStateList = new CRT_State[] { SingleAttack_normal, DoubleAttack_hoz, DoubleAttack_ver, AllAttack_simul };

        ComboState = new CRT_State_Combo(Main_SM, Boss, "Combo", Idle);

        Attack_1 = new CRT_State[] { SingleAttack_normal };
        Attack_2_h = new CRT_State[] { DoubleAttack_hoz };
        Attack_2_v = new CRT_State[] { DoubleAttack_ver };
        Attack_3 = new CRT_State[] { AllAttack_simul };

        Combo_1 = new CRT_State[] { SingleAttack_normal, SingleAttack_normal, DoubleAttack_hoz };
        Combo_2 = new CRT_State[] { SingleAttack_normal, SingleAttack_normal, SingleAttack_normal };
        Combo_3 = new CRT_State[] { SingleAttack_normal, SingleAttack_normal, SingleAttack_normal, SingleAttack_normal };
        Combo_4 = new CRT_State[] { DoubleAttack_hoz, DoubleAttack_hoz, AllAttack_simul };
        Combo_5 = new CRT_State[] { DoubleAttack_hoz, SingleAttack_normal, SingleAttack_normal, DoubleAttack_hoz, SingleAttack_normal };
        Combo_6 = new CRT_State[] { DoubleAttack_ver, DoubleAttack_ver, DoubleAttack_hoz, DoubleAttack_hoz, AllAttack_simul };
        Combo_7 = new CRT_State[] { DoubleAttack_ver, SingleAttack_normal, SingleAttack_normal, DoubleAttack_hoz, SingleAttack_normal, SingleAttack_normal };
        Combo_8 = new CRT_State[] { AllAttack_simul, SingleAttack_normal, SingleAttack_normal, SingleAttack_normal, DoubleAttack_hoz };

        
        // PHASE 1: MOVE LIST
        MovesList_1 = new CRT_State[][] { Attack_1, Attack_2_v, Combo_1, Combo_2 };
        // PHASE 2: MOVE LIST
        MovesList_2 = new CRT_State[][] { Attack_3, Combo_1, Combo_2, Combo_3, Combo_4 };
        // PHASE 2: MOVE LIST
        MovesList_3 = new CRT_State[][] { Combo_4, Combo_5, Combo_6, Combo_7, Combo_8 };

        /*MovesList_1 = new CRT_State[][] { Attack_1 };
        // PHASE 2: MOVE LIST
        MovesList_2 = new CRT_State[][] { Attack_1 };
        // PHASE 2: MOVE LIST
        MovesList_3 = new CRT_State[][] { Attack_1 };*/

        ChangeState(Idle); // Starting state is idle
        currentAnimationState = AnimationState.CRT_Idle;
    }

    private void Update()
    {
        if (Boss.isAlive)
            currentState.DoState();
        else
        {
            StopCombo();
            PlayAnimation(AnimationState.CRT_Dead);
            ChangeState(Dead);
        }
    }

    public void GetStunned()
    {
        if (comboActive)
        {
            StopCombo();
            StartCoroutine(Stun(Boss.StunnedDuration));
        }
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

    public void StopCombo()
    {
        comboActive = false;
        StopAllCoroutines();
    }

    public void DieAnim()
    {
        PlayAnimation(AnimationState.CRT_Single_Back);
    }

    IEnumerator Combo(CRT_State[] attacks)
    {
        comboStep = 0;
        comboLength = attacks.Length;
        comboActive = true;

        foreach (CRT_State state in attacks)
        {
            CRT_State S = attacks[comboStep];
            //PlayAnimation(AnimationState.CRT_Single_Back);
            comboNextState = S;
            ChangeState(PrepareAttack); // Take Aim
            yield return new WaitForSeconds(S.GetPrepDuration());
            //Boss.PlaySound();
            ChangeState(S); // Attack!! 
            yield return new WaitForSeconds(S.GetDuration());
            ComboState.SetPrevious(S);
            //ChangeState(ComboState);
            yield return new WaitForSeconds(ComboState.GetPrevious().GetCooldown());
            comboStep++;

            //

            //Prepare, S, BackToIdle
        }
        //ChangeState(Idle);
        comboActive = false;
    }

    IEnumerator Stun(float delay)
    {
        PlayAnimation(AnimationState.CRT_Stun);
        yield return new WaitForSeconds(delay);
        EndAttack();
    }

    public void startAttack()
    {
        attackActive = true;
    }
    public void EndAttack()
    {
        attackActive = false;
        ChangeState(Idle);
    }

    public void DetermineAnimation(string name)
    {
        switch (name)
        {
            case "Single":
                if(Boss.targets[0].isUp)
                    PlayAnimation(AnimationState.CRT_Single_Back);
                else
                    PlayAnimation(AnimationState.CRT_Single_Front);
                break;

            case "DoubleHoz":
                if (Boss.targets[0].isUp)
                    PlayAnimation(AnimationState.CRT_Double_Back);
                else
                    PlayAnimation(AnimationState.CRT_Double_Front);
                break;

            case "DoubleVer":
                PlayAnimation(AnimationState.CRT_Double_Vertical);
                break;

            case "AllSimul":
                PlayAnimation(AnimationState.CRT_All);
                break;

            case "Idle":
                PlayAnimation(AnimationState.CRT_Idle);
                break;

            case "Stunned":
                PlayAnimation(AnimationState.CRT_Stun);
                break;

            default:
                //PlayAnimation(AnimationState.CRT_Idle);
                break;
        }
    }

    public void PlayAnimation(AnimationState animationState)
    {
        if (currentAnimationState != animationState)
        {
            myBossAnimator.Play(animationState.ToString());
            currentAnimationState = animationState;
        }
    }
}
