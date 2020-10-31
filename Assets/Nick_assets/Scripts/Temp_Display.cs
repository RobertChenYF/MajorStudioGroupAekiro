using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Display : MonoBehaviour
{
    CRT_StateManager manager;
    CRT_Boss boss;
    TextMesh text;
    Temp_Player player;
    public TextMesh text2;
    public TextMesh text3;


    void Start()
    {
        text = this.GetComponent<TextMesh>();
        manager = FindObjectOfType<CRT_StateManager>();
        boss = FindObjectOfType<CRT_Boss>();
        player = FindObjectOfType<Temp_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.currentState.GetName();
        text2.text = "Boss Phase: " + boss.CurrentPhase;
        text3.text = "Player Damaged " + player.Damage + " times";
    }
}
