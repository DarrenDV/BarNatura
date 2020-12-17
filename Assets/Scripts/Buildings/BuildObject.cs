using UnityEngine;

public class BuildObject : OxygenUser
{
    [Header("Building Object")]
    public int BuildCost = 1;

    [Tooltip("The minimum health a tile must have bafore this building can be placed on it")]
    public int MinimumNaturePollutedDegree = 1;

    [Tooltip("Divides the build cost by this int. How much materials are returned on demolish.")]
    [SerializeField] int buildCostReturnDivider = 2;

    public override string GetBuildDescription()
    {
        return $"<b>Build cost:</b>\nBuilding materials: {BuildCost}\nHumans required: {HumansRequiredToBuild}\nMinimum tile health: {MinimumNaturePollutedDegree}";
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        //Gives resources back
        GameManager.Instance.AddBuildingMaterial(BuildCost / buildCostReturnDivider);
    }
}
