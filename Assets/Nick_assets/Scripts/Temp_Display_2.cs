using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp_Display_2 : MonoBehaviour
{
    Tank_StateManager manager;
    Tank_Boss boss;
    //Player player;

    TextMesh text;
    //public Text text2;

    //public Image playerHealth;
    public Image bossHealth;

    void Start()
    {
        text = this.GetComponent<TextMesh>();
        manager = FindObjectOfType<Tank_StateManager>();
        boss = FindObjectOfType<Tank_Boss>();
        //player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.currentState.GetName();
        //text2.text = "" + boss.CurrentPhase;

        //playerHealth.fillAmount = (float)player.health / player.MaxHealth;
        bossHealth.fillAmount = (float)boss.health / boss.MaxHealth;
    }
}
