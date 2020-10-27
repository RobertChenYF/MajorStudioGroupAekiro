using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState currentPlayerState;
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(new Idle(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerState.StateBehavior();
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        if (currentPlayerState != null) currentPlayerState.Leave();
        currentPlayerState = newPlayerState;
        currentPlayerState.Enter();
    }
}
