using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour
{

    public int tileCount;

    //Tile required variables
    //Nature
    public int requiredNatureTiles;
    [Range (0, 1)]
    public float requiredNatureTilePercent;
    public int currentNatureTiles;

    //Toxic
    public int requiredToxicTiles;
    [Range(0, 1)]
    public float requiredToxicTilePercent;
    public int currentToxicTiles;

    // Start is called before the first frame update
    void Start()
    {
        TileAmountCalculation();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckWin();
    }

    void TileAmountCalculation()
    {
        tileCount = GameObject.Find("Planet").GetComponent<Hexsphere>().TileCount;

        requiredNatureTiles = Mathf.RoundToInt(tileCount * requiredNatureTilePercent);
        requiredToxicTiles = Mathf.RoundToInt(tileCount * requiredToxicTilePercent);
    }

    public void AddTile(bool natureTile, bool toxicTile)
    {
        if (natureTile)
        {
            currentNatureTiles++;
        }
        if (toxicTile)
        {
            currentToxicTiles++;
        }
    }

    void CheckWin()
    {
        if(currentNatureTiles >= requiredNatureTiles)
        {
            Debug.Log("WINNER WINNER CHICKEN DINNER");
        }
    }
}
