using UnityEngine;

public class HouseScript : BuildingScript
{
    [Header("House Script")]
    [Tooltip("How many humans spawn when this house is done building")]
    [SerializeField] private int populationToAdd;

    public override string GetName()
    {
        return "House";
    }

    public override string GetDescription()
    {
        return $"This building houses a max of {maxCapacity} people.";
    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.AddPopulation(populationToAdd);
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();
    }
}
