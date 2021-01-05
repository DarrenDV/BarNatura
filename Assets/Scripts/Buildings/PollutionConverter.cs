using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionConverter : BuildingScript
{

    #region Variables

    [Header("Pollution Converter")]
    [SerializeField] private int pollutionToRemove;
    [SerializeField] private int oxygenToAdd;

    //Conversion timer variables
    [Tooltip("Time in seconds for every conversion")]
    [SerializeField] private float conversionRate;

    private float timeSinceLastConversion;

    #endregion

    #region Strings

    public override string GetName()
    {
        return "Pollution Converter";
    }

    protected override string GetBuildingFunction()
    {
        return $"Converts {HudManager.GetIcon("Pollution")} into {HudManager.GetIcon("OxygenPlus")}";
    }

    public override string GetDescription()
    {
        return $"This {GetName()} converts {pollutionToRemove} {HudManager.GetIcon("Pollution")} into {oxygenToAdd} {HudManager.GetIcon("OxygenPlus")} every {conversionRate} seconds.\n\n{ShowProgress()}";
    }

    #endregion

    #region Default

    protected override void Update()
    {
        base.Update();

        UpdatePollution();
    }

    /// <summary>
    /// Updates pullution in the atmosphere using the pollution converter.
    /// </summary>
    private void UpdatePollution()
    {
        if (!IsBeingBuild && !IsBeingRemoved) //Starts the cleaning if the tile is done and not removing
        {
            
            if(GameManager.Instance.GetPollution() > 0)
            {
                isProducing = true;
                timeSinceLastConversion += Time.deltaTime;

                if (timeSinceLastConversion >= conversionRate) //If enough time has passed, remove x pollution and add y oxygen to atmosphere
                {
                    GameManager.Instance.RemovePollution(pollutionToRemove); 
                    GameManager.Instance.AddOxygenGeneration(oxygenToAdd);

                    timeSinceLastConversion = 0;
                }
            }

            else
            {
                isProducing = false;
            }
        }
    }

    #endregion

    #region Production

    protected override float GetMaxTime()
    {
        return conversionRate;
    }

    protected override float GetTimer()
    {
        return timeSinceLastConversion;
    }

    protected override string GetResourceDrained()
    {
        return $"{HudManager.GetIcon("Pollution")}";
    }

    protected override string GetResourceGain()
    {
        return $"{HudManager.GetIcon("OxygenPlus")}";
    }

    protected override string GetNoProducingString()
    {
        return "There is no longer any pollution in the air!";
    }

    #endregion

}
