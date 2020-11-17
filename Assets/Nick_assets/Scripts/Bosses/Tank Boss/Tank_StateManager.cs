using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_StateManager : MonoBehaviour
{
    [SerializeField]
    public Tank_State currentState;
    protected static Tank_StateManager Main_SM;
    protected static Tank_Boss Boss;
    protected static Temp_Player player;

    public Tank_State Idle;
    public Tank_State Prepare;
    public Tank_State DriveBy;
    public Tank_State DriveOff;
    public Tank_State DriveOn;
    public Tank_State DriveCombo;
    public Tank_State DriveIn;
    public Tank_State AttackJump;
    public Tank_State Dead;

    public Tank_State[] Attack_1;

    public Tank_State[] attackStateList;
    public Tank_State[][] MoveList_1;


    [HideInInspector]
    public bool comboActive;
    [HideInInspector]
    public int comboStep, comboLength;
    public Tank_State comboNextState;


    private void Start()
    {
        Main_SM = this;
        Boss = this.GetComponent<Tank_Boss>();
        player = FindObjectOfType<Temp_Player>();

        Idle = new Tank_State_Idle(Main_SM, Boss, "Idle", Boss.TimeBetweenAttacks_1);
        Prepare = new Tank_State_Prepare(Main_SM, Boss, "Prepare", Boss.TimeBeforeDriveLaunch);
        DriveBy = new Tank_State_DriveBy(Main_SM, Boss, "DriveBy", 0);
        DriveOff = new Tank_State_DriveOff(Main_SM, Boss, "DriveOff", 0);
        DriveIn = new Tank_State_DriveIn(Main_SM, Boss, "DriveIn", 0);
        DriveCombo = new Tank_State_DriveCombo(Main_SM, Boss, "DriveCombo", Boss.DriveComboWaitDuration);
        AttackJump = new Tank_State_AttackJump(Main_SM, Boss, "AttackJump", Boss.JumpPrepDuration, Boss.JumpDuration, Boss.JumpHitDuration, Boss.JumpReturnDuration);

        Attack_1 = new Tank_State[] { AttackJump };
        MoveList_1 = new Tank_State[][] { Attack_1 };

        ChangeState(Idle);
    }

    private void Update()
    {
        currentState.DoState();
    }

    public void ChangeState(Tank_State newState)
    {
        if (currentState != null) currentState.Leave();
        currentState = newState;
        currentState.Enter();
    }

    public void PerformCombo(Tank_State[] attackList)
    {
        StartCoroutine(Combo(attackList));
    }

    public void StopCombo()
    {
        comboActive = false;
        StopAllCoroutines();
    }

    IEnumerator Combo(Tank_State[] attacks)
    {
        comboStep = 0;
        comboLength = attacks.Length;
        comboActive = true;

        foreach (Tank_State state in attacks)
        {
            Tank_State S = attacks[comboStep];
            ChangeState(Prepare);
            yield return new WaitForSeconds(S.GetPrepDuration());
            ChangeState(S);
            yield return new WaitForSeconds(S.GetDuration());
            float returnDur = S.GetReturnDuration();
            // combo state
            yield return new WaitForSeconds(returnDur);
            comboStep++;
        }
        ChangeState(Idle);
        comboActive = false;
    }
}
