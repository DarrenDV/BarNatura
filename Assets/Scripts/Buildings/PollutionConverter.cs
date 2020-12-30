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

    [SerializeField] private ParticleSystem BuildingRemovingParticleEffect;

    private float conversionTimer;

    #endregion

    #region Strings

    public override string GetName()
    {
        return "Toxic cleaner";
    }

    protected override string GetBuildingFunction()
    {
        return $"Converts {HudManager.GetIcon("Pollution")} into {HudManager.GetIcon("OxygenPlus")}";
    }

    public override string GetDescription()
    {
        return $"This pollution converter converts {pollutionToRemove} {HudManager.GetIcon("Pollution")} into {oxygenToAdd} {HudManager.GetIcon("OxygenPlus")} every {conversionRate} seconds.";
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
                conversionTimer += Time.deltaTime;

                if (conversionTimer >= conversionRate)
                {
                    GameManager.Instance.RemovePollution(pollutionToRemove);
                    GameManager.Instance.AddOxygenGeneration(oxygenToAdd);

                    conversionTimer = 0;
                }
            }
        }
    }

    #endregion

    #region Building
    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();
        BuildingRemovingParticleEffect.Stop();
    }
    public override void OnRemove()
    {
        base.OnRemove();
        BuildingRemovingParticleEffect.Play();
    }
    #endregion
}
