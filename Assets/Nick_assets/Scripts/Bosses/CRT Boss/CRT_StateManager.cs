using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT_StateManager : MonoBehaviour
{
    [SerializeField]
    private CRT_State currentState;
    protected static CRT_StateManager Main_SM;
    protected static CRT_Boss Main_B;

    public CRT_State Idle;
    public CRT_State PrepareAttack;
    public CRT_State SingleAttack;

    private void OnEnable()
    {
        Main_SM = this;
        Main_B = this.GetComponent<CRT_Boss>();
        Idle = new CRT_State_Idle(Main_SM, Main_B);
        PrepareAttack = new CRT_State_PrepareAttack(Main_SM, Main_B);
        SingleAttack = new CRT_State_SingleAttack(Main_SM, Main_B);


        currentState = Idle;
    }

    private void Update()
    {
        currentState.DoState();
    }

    public void ChangeState(CRT_State newState)
    {
        if (currentState != null) currentState.Leave();
        currentState = newState;
        currentState.Enter();
    }
}
