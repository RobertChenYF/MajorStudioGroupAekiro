using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location_Boss : MonoBehaviour
{
    SpriteRenderer sp;

    public Location[] validLocs;

    public bool isUp;
    public bool isOccupied;
    public bool isOffscreen;

    void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        //sp.color = new Color(1, 1, 1, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Location[] GetValidLocations()
    {
        if (validLocs != null)
            return validLocs;
        else
            return null;
    }
}
