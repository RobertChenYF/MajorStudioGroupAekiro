using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Display : MonoBehaviour
{
    CRT_StateManager manager;
    CRT_Boss boss;
    TextMesh text;
    public TextMesh text2;

    void Start()
    {
        text = this.GetComponent<TextMesh>();
        manager = FindObjectOfType<CRT_StateManager>();
        boss = FindObjectOfType<CRT_Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.currentState.GetName();
        text2.text = "Boss Phase: " + boss.CurrentPhase;
    }
}
