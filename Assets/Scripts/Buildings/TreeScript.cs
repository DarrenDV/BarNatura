public class TreeScript : BuildObject
{
    public override string GetName()
    {
        return "Tree";
    }

    public override string GetDescription()
    {
        return $"This tree produces {oxygenProduction} oxygen.";
    }

    public override string GetWhileBeingBuildDescription()
    {
        return "This tree is currently growing.";
    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        parentTile.SetNaturePollutedDegree(10);
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();
    }
}
