using UnityEngine;

public class BuildObject : OxygenUser
{
    [Header("Building Object")]
    public int BuildCost = 1;

    [Tooltip("The minimum health a tile must have bafore this building can be placed on it")]
    public int MinimumNaturePollutedDegree = 1;

    public override string GetBuildDescription()
    {
        return $"<b>Build cost:</b>\nBuilding materials: {BuildCost}\nHumans required: {HumansRequiredToBuild}\nMinimum tile health: {MinimumNaturePollutedDegree}";
    }
}
