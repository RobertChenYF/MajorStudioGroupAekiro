using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ScreenShakeControl : MonoBehaviour
{
    private GameObject thisCamera;
    [HideInInspector]public float shakeXMag;
    [HideInInspector] public float shakeYMag;
    [HideInInspector] public float shakeDuration;
    PlayerIndex playerIndex;
    [HideInInspector]public float rumbleIntensity;
    [HideInInspector]public float rumbleDuration;
    private Vector3 defaultCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GameObject.Find("Main Camera");
        defaultCameraPos = thisCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            shakeDuration -= Time.deltaTime;
            thisCamera.transform.position = defaultCameraPos + new Vector3(Random.Range(-shakeXMag,shakeXMag), Random.Range(-shakeXMag, shakeXMag),0);
        }
        else
        {
            thisCamera.transform.position = defaultCameraPos;
        }


        if (rumbleDuration > 0)
        {
            GamePad.SetVibration(playerIndex, rumbleIntensity, rumbleIntensity);
            rumbleDuration -= Time.deltaTime;
        }
        else
        {
            GamePad.SetVibration(playerIndex, 0, 0);
        }
        
    }
}
