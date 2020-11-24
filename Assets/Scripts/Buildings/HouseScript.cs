public class HouseScript : BuildingScript
{
    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddPopulation(3);
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
        GameManager.Instance.RemovePopulation(3);
    }
}
