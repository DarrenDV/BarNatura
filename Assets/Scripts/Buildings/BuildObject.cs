using UnityEngine;

public class BuildObject : OxygenUser
{
    [Header("Building Object")]
    public int BuildCost = 1;

    [Tooltip("The minimum health a tile must have bafore this building can be placed on it")]
    public int MinimumNaturePollutedDegree = 1;

    #region Build tool tip info

    /// <summary>
    /// Get the description when the player wants to build this object.
    /// </summary>
    /// <returns></returns>
    public string GetBuildButtonInformation()
    {
        return $"{GetBuildingFunction()}\n" +
            $"{GetBuildRequirements()}\n" +
            $"{GetOxygenCosts()}";
    }

    /// <summary>
    /// Describe what the building does
    /// </summary>
    /// <returns></returns>
    protected virtual string GetBuildingFunction()
    {
        return "Allan please add details";
    }

    protected virtual string GetBuildRequirements()
    {
        return $"{HudManager.GetIcon("Human")} {HumansRequiredToBuild} {HudManager.GetIcon("Brick")} {BuildCost}";
    }

    protected virtual string GetOxygenCosts()
    {
        return $"{HudManager.GetIcon("OxygenMin")} {oxygenUsage} {HudManager.GetIcon("Pollution")} {pollutionProduction}";
    }

    #endregion
}
