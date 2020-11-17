public class HouseScript : BuildingScript
{
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
    }
}
