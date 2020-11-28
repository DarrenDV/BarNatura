using UnityEngine;

public class HouseScript : BuildingScript
{

    [Header("House Script")]
    [SerializeField] private int populationToAdd;

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddPopulation(populationToAdd);
    }

    public override string GetName()
    {
        return "House";
    }

    public override string GetDescription()
    {
        return $"This building houses a max of {maxCapacity} people.";
    }

    public override void Remove()
    {
        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();
        GameManager.Instance.RemoveCapacity(maxCapacity);
    }
}
