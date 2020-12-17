using UnityEngine;

public class BuildObject : OxygenUser
{
    [Header("Building Object")]
    public int BuildCost = 1;

    [Tooltip("The minimum health a tile must have bafore this building can be placed on it")]
    public int MinimumNaturePollutedDegree = 1;

    #region Build tool tip info

    /// <summary>
    /// Return a icon to use in the tooltip
    /// </summary>
    /// <param name="IconName">The name of the icon</param>
    /// <returns></returns>
    protected string GetIcon(string IconName)
    {
        switch(IconName)
        {
            case "House": return "<sprite=0>";
            case "Raw": return "<sprite=1>";
            case "Toxic": return "<sprite=2>";
            case "Human": return "<sprite=3>";
            case "Brick": return "<sprite=4>";
            case "Nature": return "<sprite=5>";
            case "OxygenPlus": return "<sprite=6>";
            case "OxygenMin": return "<sprite=7>";
            case "Pollution": return "<sprite=8>";
            default: return "Icon not found";
        };
        
    }

    /// <summary>
    /// Get the description when the player wants to build this object.
    /// </summary>
    /// <returns></returns>
    public string GetBuildButtonInformation()
    {
        return $"{GetBuildingFunction()}\n" +
            $"{GetBuildRequirements()}\n" +
            $"{GetOygenCosts()}";
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
        return $"{GetIcon("Human")} {HumansRequiredToBuild} {GetIcon("Brick")} {BuildCost} {GetIcon("Nature")} {MinimumNaturePollutedDegree}";
    }

    protected virtual string GetOygenCosts()
    {
        return $"{GetIcon("OxygenMin")} {oxygenUsage} {GetIcon("Pollution")} {pollutionProduction}";
    }

    #endregion
}
