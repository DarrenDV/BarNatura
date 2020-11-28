using UnityEngine;

public class TileVariables : MonoBehaviour
{
    //Create a variable here and assing it on TileVariables in the inspector

    //Tile spreading variables
    [Header("Colors")]
    public Gradient gradient;

    //Spawned tile variables
    [Header("Spawning objects")]
    public GameObject startingSpaceShip;
    public GameObject lavaTile;
    public GameObject rubbleTile;
    [Range(1, 100)]
    public int rubbleSpawnChance;
    [Range(1, 100)]
    public int lavaSpawnChance;

    //Uncategorized variables
    [Header("Time")]
    public float secondsToUpdate;
}