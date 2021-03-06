using UnityEngine;

public class TileVariables : MonoBehaviour
{
    //Create a variable here and assing it on TileVariables in the inspector

    //Tile spreading variables
    [Header("Colors")]
    public Gradient gradient;

    [Header("Materials")]
    public Material infectionTexture;
    public Material barrenTexture;

    [Header("Tile spreading")]
    public int maxChance = 100;
    [Range(1, 100)]
    public int natureChance;
    [Range(1, 100)]
    public int toxicChance; 

    //Spawned tile variables
    [Header("Spawning objects")]
    public int ToxicTilesToSpawn = 3;
    public GameObject[] tileDecals;
    public GameObject startingSpaceShip;
    public GameObject lavaTile;
    public GameObject[] rubbleTiles;
    public GameObject toxicParticles;
    [Range(1, 100)]
    public int rubbleSpawnChance;
    [Range(1, 100)]
    public int lavaSpawnChance;
    [Range(1, 100)]
    public int toxicTileChance;

    //Uncategorized variables
    [Header("Time")]
    public float secondsToUpdate;
}