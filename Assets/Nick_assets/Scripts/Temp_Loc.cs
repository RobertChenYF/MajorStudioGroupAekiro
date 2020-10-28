using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Loc : MonoBehaviour
{
    public GameObject locA, locB, locC, locD;

    SpriteRenderer spA, spB, spC, spD;

    void Start()
    {
        spA = locA.GetComponent<SpriteRenderer>();
        spB = locB.GetComponent<SpriteRenderer>();
        spC = locC.GetComponent<SpriteRenderer>();
        spD = locD.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
