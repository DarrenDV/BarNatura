﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTileScript : MonoBehaviour
{
    [Header("Default tile production variables")]
    [SerializeField] protected float oxygenProduction; 
    [SerializeField] protected float pollutionProduction;

    [Header("Tile pollution state variables")]
    protected float maxPollutedPercentage = 100f;
    [SerializeField] protected float pollutedPercentage;  

    [Header("Tile nature state variables")]
    protected float maxNaturePercentage = 100f;
    [SerializeField] protected float naturePercentage;      
    
    [Header("Checks if the tile is occupied")]
    public bool occupied;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This could be in another function or method
        if(pollutedPercentage >= maxPollutedPercentage){
            //Make tile uninhabitable or toxic.
        }

        //This could be in another function or method
        if(naturePercentage >= maxNaturePercentage){
            //Make tile nature.
        }
    }
}
