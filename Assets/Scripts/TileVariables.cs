using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVariables : MonoBehaviour
{
    //Create a variable here and assing it on TileVariables in the inspector
    //TileVariables is a child under the GameManager
    

    //Tile spreading variables
    public Gradient gradient;

    //Spawned tile variables
    public GameObject rubbleTile;
    public int rubbleSpawnChance;
    public GameObject lavaTile;
    public int lavaSpawnChance;

    //Uncategorized variables
    public float secondsToUpdate;
}