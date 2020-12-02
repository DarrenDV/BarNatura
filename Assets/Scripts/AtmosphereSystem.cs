using UnityEngine;

public class AtmosphereSystem : MonoBehaviour
{
    protected int atmosphereLevel1Threshhold = 1300, atmosphereLevel2Threshhold = 2600, atmosphereLevel3Threshhold = 3900;
    public int currentOxygen, currentAtmosphereLevel;
    protected float atmosphereTimer = 0;
    // Update is called once per frame
    void Update()
    {
        //TIMER IS NOT OBLIGATED, REMOVE IF NEED BE.
        //This counts up the total amount of oxygen produced by the player.
        if (atmosphereTimer * Time.deltaTime > 5)
        {
            currentOxygen += GameManager.Instance.GetOxygenSurplus();
        }

        //level check, this could be a switch case but i cant fill in atmosphereLevel1Threshhold as a case, with no magic numbers this is all i could think of.
        if (currentOxygen > atmosphereLevel1Threshhold)
        {
            currentAtmosphereLevel = 1;
        }
        if (currentOxygen > atmosphereLevel2Threshhold)
        {
            currentAtmosphereLevel = 2;
        }
        if (currentOxygen > atmosphereLevel3Threshhold)
        {
            currentAtmosphereLevel = 3;
        }
        atmosphereTimer++;
    }
}
