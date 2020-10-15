using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureTileScript : BaseTileScript
{
    float Timer;

    // Update is called once per frame
    void Update()
    {
        // This add's oxygen every second if the tile is 100% nature and is not polluted.
        if (Timer >= 60 * Time.deltaTime) {
            if (!PolutionLevelCheck())
            {
                if (NatureLevelCheck())
                {
                    oxygenProduction += 5;
                }
            }
            Timer = 0;
        }
        Timer += 1 * Time.deltaTime;
    }
}
