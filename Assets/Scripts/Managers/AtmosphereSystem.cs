using UnityEngine;
using UnityEngine.UI;

public class AtmosphereSystem : MonoBehaviour
{
    [SerializeField] private int atmosphereLevel1Threshhold = 300;
    [SerializeField] private int atmosphereLevel2Threshhold = 1300;
    [SerializeField] private int atmosphereLevel3Threshhold = 2600;
    [SerializeField] private int atmosphereLevel4Threshhold = 3900;

    [SerializeField] private float maxAtmopshereTimer = 5;
    [SerializeField] private Text currentOxygenCounter;
    [SerializeField] private Sprite level1, level2, level3, level4;
    [SerializeField] private Image atmosphereLevelImage;
    private int currentOxygen, currentAtmosphereLevel;
    private float atmosphereTimer;

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

    private void LevelCheck()
    {
        //level check, this could be a switch case but i cant fill in atmosphereLevel1Threshhold as a case, with no magic numbers this is all i could think of.
        if (currentOxygen > atmosphereLevel4Threshhold && currentAtmosphereLevel != 4)
        {
            currentAtmosphereLevel = 4;
            atmosphereLevelImage.sprite = level4;
            UpdateTreeNatureRadius();
        }
        else if (currentOxygen > atmosphereLevel3Threshhold && currentAtmosphereLevel != 3)
        {
            currentAtmosphereLevel = 3;
            atmosphereLevelImage.sprite = level3;
            UpdateTreeNatureRadius();
        }
        else if (currentOxygen > atmosphereLevel2Threshhold && currentAtmosphereLevel != 2)
        {
            currentAtmosphereLevel = 2;
            atmosphereLevelImage.sprite = level2;
            UpdateTreeNatureRadius();
        }
        else if (currentOxygen > atmosphereLevel1Threshhold && currentAtmosphereLevel != 1)
        {
            currentAtmosphereLevel = 1;
            atmosphereLevelImage.sprite = level1;
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

        foreach (var tree in FindObjectsOfType<TreeScript>())
        {
            tree.UpdateNatureSpreadTiles(currentAtmosphereLevel);
        }
    }

    private void UpdateCurrentOxygenCounter()
    {
        if (currentOxygenCounter != null)
        {
            currentOxygenCounter.text = currentOxygen.ToString();
        }
    }
}
