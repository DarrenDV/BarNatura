﻿using UnityEngine;
using UnityEngine.UI;

public class AtmosphereSystem : MonoBehaviour
{
    [SerializeField] private int atmosphereLevel1Threshhold = 1300, atmosphereLevel2Threshhold = 2600, atmosphereLevel3Threshhold = 3900;
    [SerializeField] private float maxAtmopshereTimer = 5;
    [SerializeField] private Text currentOxygenCounter;
    private int currentOxygen, currentAtmosphereLevel;
    private float atmosphereTimer = 0;
    // Update is called once per frame
    void Update()
    {
        //This counts up the total amount of oxygen produced by the player.
        if (atmosphereTimer > maxAtmopshereTimer)
        {
            currentOxygen += GameManager.Instance.GetOxygenSurplus();
            LevelCheck();
            atmosphereTimer = 0;
        }
        UpdateCurrentOxygenCounter();
        atmosphereTimer += Time.deltaTime;
    }
    void LevelCheck()
    {
        //level check, this could be a switch case but i cant fill in atmosphereLevel1Threshhold as a case, with no magic numbers this is all i could think of.
        if (currentOxygen > atmosphereLevel3Threshhold && currentAtmosphereLevel != 3)
        {
            currentAtmosphereLevel = 3;
            UpdateTreeNatureRadius();
        } else
        if (currentOxygen > atmosphereLevel2Threshhold && currentAtmosphereLevel != 2)
        {
            currentAtmosphereLevel = 2;
            UpdateTreeNatureRadius();
        } else
        if (currentOxygen > atmosphereLevel1Threshhold && currentAtmosphereLevel != 1)
        {
            currentAtmosphereLevel = 1;
            UpdateTreeNatureRadius();
        }
    }

    public int GetCurrentAtmosphereLevel()
    {
        return currentAtmosphereLevel;
    }

    private void UpdateTreeNatureRadius()
    {
        foreach (var tile in FindObjectsOfType<BaseTileScript>())
        {
            tile.canBecomeNature = false;
        }

        foreach(var tree in FindObjectsOfType<TreeScript>())
        {
            tree.UpdateNatureSpreadTiles(currentAtmosphereLevel);
        }
    }
    private void UpdateCurrentOxygenCounter()
    {
        currentOxygenCounter.text = currentOxygen.ToString();
    }
}
