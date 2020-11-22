using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeControl : MonoBehaviour
{
    private GameObject thisCamera;
    public float shakeXMag;
    public float shakeYMag;
    public float shakeDuration;
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
    }
}
