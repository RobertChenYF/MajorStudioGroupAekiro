using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    public Location targetLoc;
    public float speed_up;
    public float speed_down_front;
    public float speed_down_back;
    public float hangDuration;
    public float maxHeight;
    public float HitDelay;

    float t;
    bool goingUp;
    bool goingDown;

    Rigidbody2D rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        goingUp = true;
        goingDown = false;
        t = 0;
    }

    void Update()
    {
        CheckPosition();

        if (goingUp)
        {
            rb.MovePosition(transform.position + Vector3.up * speed_up);
        }
        else if (!goingUp && !goingDown)
        {
            transform.position = new Vector2(targetLoc.transform.position.x, maxHeight);
            if (t < hangDuration)
            {
                t += Time.deltaTime;
            }
            else
            {
                goingDown = true;
            }
        }
        else if (goingDown)
        {
            rb.MovePosition(transform.position + Vector3.down * speed_down_front);

            /*if (targetLoc.isUp)
                rb.MovePosition(transform.position + Vector3.down * speed_down_back);
            else
                rb.MovePosition(transform.position + Vector3.down * speed_down_front);*/
        }
    }

    void CheckPosition()
    {
        if (transform.position.y >= maxHeight)
        {
            goingUp = false;
        }
        else if (transform.position.y <= targetLoc.transform.position.y && goingDown)
        {
            targetLoc.HitMortar(HitDelay);
            //targetLoc.Hit();

            //if (targetLoc.isTargetMortar)
              //  targetLoc.ClearTargetMortar();
            Destroy(this.gameObject);

        }
    }

    public void SetTargetLoc(Location loc)
    {
        targetLoc = loc;
        //targetPos = loc;
    }
}
