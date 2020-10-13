using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureTileScript : BaseTileScript
{
    float Timer;

    // Update is called once per frame
    void Update()
    {
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
