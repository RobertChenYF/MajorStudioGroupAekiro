using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilShot : MonoBehaviour
{
    Location targetLoc;
    public float TimeToReachTarget;
    public AnimationCurve curve;
    public Vector3 LerpOffset;

    Rigidbody2D rb;
    float t1;
    Vector2 startPos, targetPos;


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        startPos = this.transform.position;
        t1 = 0;
    }

    void Update()
    {
        if (t1 < TimeToReachTarget)
        {
            t1 += Time.deltaTime;
            float lerpRatio = t1 / TimeToReachTarget;
            Vector2 positionOffset = curve.Evaluate(lerpRatio) * LerpOffset;
            this.transform.position = Vector2.Lerp(startPos, targetPos, lerpRatio) + positionOffset;
        }
        else
        {
            Debug.Log("ARRIVED");
            targetLoc.OilOn();
            Destroy(this.gameObject);
        }
    }

    public void SetTargetLoc(Location loc)
    {
        targetLoc = loc;
        targetPos = targetLoc.transform.position;
    }
}
