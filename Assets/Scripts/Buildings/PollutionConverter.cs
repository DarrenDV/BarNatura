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
    private float conversionTimer;

    #endregion

    #region Strings

    public override string GetName()
    {
        return "Toxic cleaner";
    }

    protected override string GetBuildingFunction()
    {
        return $"Converts {pollution} into {oxygenPlus}";
    }

    public override string GetDescription()
    {
        return $"This pollution converter converts {pollutionToRemove} {pollution} into {oxygenToAdd} {oxygenPlus} every {conversionRate} seconds.\n\n {ShowProgress()}";
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
        if (!IsBeingBuild && !IsBeingRemoved)
        {
            if(GameManager.Instance.GetPollution() > 0)
            {
                isProducing = true;
                conversionTimer += Time.deltaTime;

                if (conversionTimer >= conversionRate)
                {
                    GameManager.Instance.RemovePollution(pollutionToRemove);
                    GameManager.Instance.AddOxygenGeneration(oxygenToAdd);

                    conversionTimer = 0;
                }
            }
            else
            {
                isProducing = false;
            }
        }
    }

    #endregion

    #region Progress

    //protected override float GetMaxArrows()
    //{
    //    return conversionRate;
    //}

    protected override float GetTimer()
    {
        return conversionTimer;
    }

    protected override string GetResousreDrained()
    {
        return $"{pollution}";
    }

    protected override string GetResousreGain()
    {
        return $"{oxygenPlus}";
    }

    protected override string GetNoProducingString()
    {
        return $"No {pollution} left in the air!";
    }

    #endregion
}
