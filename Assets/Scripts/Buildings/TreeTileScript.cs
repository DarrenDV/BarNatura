using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTileScript : MonoBehaviour
{
    int treeOxygenProduction = 8;
    void Start()
    {
        // This adds the base production value of 1 tree to the Oxygen Production
        GameManager.Instance.AddOxygenGeneration(treeOxygenProduction);
    }
}
