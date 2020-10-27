// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NatureTileScript : BaseTileScript
// {
//     private float timerOxygen, timerSpread;
//     public float production = 10;

//     // Update is called once per frame
//     void Update()
//     {
//         // This add's oxygen every second if the tile is 100% nature and is not polluted.
//         if (timerOxygen >= 60 * Time.deltaTime)
//         {
//             if (!PolutionLevelCheck())
//             {
//                 if (NatureLevelCheck())
//                 {
//                     oxygenProduction += production / 2;
//                 }
//             }
//             timerOxygen = 0;
//         }

//         if (timerSpread >= 240 * Time.deltaTime)
//         {
//             for (int i = 0; i < neighborTiles.Count; i++)
//             {
//                 if (neighborTiles[i] is BaseTileScript baseTile)
//                 {
//                     if (baseTile.naturePollutedDegree > -7 && baseTile.naturePollutedDegree <= 10  && naturePollutedDegree == 10)
//                     {
//                         baseTile.naturePollutedDegree++;
//                     }
//                     timerSpread = 0;
//                 }
//             }
//         }

//         timerOxygen += 1 * Time.deltaTime;
//         timerSpread += 1 * Time.deltaTime;
//     }
// }

