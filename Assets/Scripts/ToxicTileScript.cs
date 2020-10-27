using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicTileScript : BaseTileScript
{

    float Timer, timerSpread;
    List<Tile> surroundingTiles = new List<Tile>();

    void Start()
    {
        //Run hier de code om de tiles om de tile heen toe te voegen aan de list
    }


    void Update()
    {
        foreach (Tile tile in surroundingTiles)
        {
            if (Timer >= 60 * Time.deltaTime)
            {
                if (!NatureLevelCheck())
                {
                    if (PolutionLevelCheck())
                    {
                        pollutionProduction += 5;
                        polluted = true;
                    }
                }
                Timer = 0;
            }

        if (timerSpread >= 240 * Time.deltaTime)
        {
            for (int i = 0; i < neighborTiles.Count; i++)
            {
                if (neighborTiles[i] is BaseTileScript baseTile)
                {
                    if (baseTile.pollutedDegree > -7 && baseTile.pollutedDegree <= 10)
                    {
                        baseTile.pollutedDegree++;
                    }
                    timerSpread = 0;
                }
            }
        }

            Timer += 1 * Time.deltaTime;
            timerSpread += 1 * Time.deltaTime;
        }
    }
}
