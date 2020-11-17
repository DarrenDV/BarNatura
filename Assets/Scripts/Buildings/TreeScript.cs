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
}
