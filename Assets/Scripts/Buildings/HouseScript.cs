using UnityEngine;

public class HouseScript : BuildingScript
{
    [Header("House Script")]
    [SerializeField] private int populationToAdd = 3;

    public override string GetName()
    {
        return "House";
    }

    protected override string GetBuildingFunction()
    {
        return $"+ {maxCapacity} {HudManager.GetIcon("House")}, + {populationToAdd} {HudManager.GetIcon("Human")}";
    }

    public override string GetDescription()
    {
        return $"This house has {maxCapacity} {HudManager.GetIcon("House")}.";
    }

    public override void OnRemove()
    {
        base.OnRemove();

        GameManager.Instance.RemoveCapacity(maxCapacity);
    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.AddPopulation(populationToAdd);
    }

}
