using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect: MonoBehaviour
{
    [SerializeField]private GameObject MainCharacter;



    public void ChangeToIdle()
    {
        MainCharacter.GetComponent<PlayerStateManager>().BackToIdle();
    }

    public void Attack(float damage)
    {
        MainCharacter.GetComponent<PlayerStateManager>().MeleeAttack(damage);
    }
}
