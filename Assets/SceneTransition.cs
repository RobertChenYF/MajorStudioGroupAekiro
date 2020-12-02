using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public CRT_Boss boss;
    private float timer = 0;
    public GameObject continueScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.health<=0)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 2.0f)
        {
            continueScreen.SetActive(true);
            if (Input.GetButtonDown("continue"))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
