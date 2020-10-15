using System.Collections;
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
    public bool polluted; 

    public bool PolutionLevelCheck()
    {
        if (pollutedPercentage >= maxPollutedPercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NatureLevelCheck()
    {
        if (naturePercentage >= maxNaturePercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
