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
    public string myName;

    void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        //sp.color = new Color(1, 1, 1, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return myName;
    }

    public Location[] GetValidLocations()
    {
        if (validLocs != null)
            return validLocs;
        else
            return null;
    }
}
