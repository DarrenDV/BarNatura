using UnityEngine;

public class AtmosphereSystem : MonoBehaviour
{
    [SerializeField] private int atmosphereLevel1Threshhold = 1300, atmosphereLevel2Threshhold = 2600, atmosphereLevel3Threshhold = 3900;
    [SerializeField] private float maxAtmpshereTimer = 5;
    private int currentOxygen, currentAtmosphereLevel;
    private float atmosphereTimer = 0;
    // Update is called once per frame
    void Update()
    {
        //This counts up the total amount of oxygen produced by the player.
        if (atmosphereTimer > maxAtmpshereTimer)
        {
            currentOxygen += GameManager.Instance.GetOxygenSurplus();
            LevelCheck();
            atmosphereTimer = 0;
        }
        atmosphereTimer += Time.deltaTime;
    }
    void LevelCheck()
    {
        //level check, this could be a switch case but i cant fill in atmosphereLevel1Threshhold as a case, with no magic numbers this is all i could think of.
        if (currentOxygen > atmosphereLevel3Threshhold)
        {
            currentAtmosphereLevel = 3;
        } else
        if (currentOxygen > atmosphereLevel2Threshhold)
        {
            currentAtmosphereLevel = 2;
        } else
        if (currentOxygen > atmosphereLevel1Threshhold)
        {
            currentAtmosphereLevel = 1;
        }
    }
}
