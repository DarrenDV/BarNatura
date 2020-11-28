using UnityEngine;

public class BuildObject : OxygenUser
{
    [Header("Building Object")]
    public int BuildCost = 1;

    /// <summary>
    /// This building can only be placed on tiles with a naturePollutedDegree of this or higher
    /// </summary>
    public int MinimumNaturePollutedDegree = 1;

    public override string GetBuildDescription()
    {
        return $"<b>Build cost:</b>\nBuilding materials: {BuildCost}\nHumans required: {HumansRequiredToBuild}\nMinimum tile health: {MinimumNaturePollutedDegree}";
    }
}
