using System.Collections.Generic;
using System.Linq;

public class TreeScript : BuildObject
{
    private List<BaseTileScript> surroundingTiles = new List<BaseTileScript>();

    protected override void Start()
    {
        base.Start();

        FindObjectOfType<TutorialManager>().ProgressTutorial("TreeBuilt"); //Runs the ontreebuilt if the tutorial hasn't ended
    }

    public override string GetName()
    {
        return "Tree";
    }

    protected override string GetBuildingFunction()
    {
        return $"Produces {HudManager.GetIcon("OxygenPlus")}";
    }

    protected override string GetOxygenCosts()
    {
        return $"{HudManager.GetIcon("OxygenPlus")} {oxygenProduction}";
    } 

    public override string GetDescription()
    {
        return $"This {GetName()} produces {oxygenProduction} {HudManager.GetIcon("OxygenPlus")}";
    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        UpdateNatureSpreadTiles(FindObjectOfType<AtmosphereSystem>().GetCurrentAtmosphereLevel());
        parentTile.SetNaturePollutedDegree(10); //Sets the tile to full nature
    }

    /// <summary>
    /// Called when the atmosphere changes
    /// </summary>
    public void UpdateNatureSpreadTiles(int currentAtmosphereLevel)
    {
        var radius = currentAtmosphereLevel * 2 + 1; //Doubles the atmopshere level and does that +1 and applies that as radius. IE Atmopshere level 1 will gave radius 3

        surroundingTiles = parentTile.GetNeighbourTiles(radius);

        foreach (var tile in surroundingTiles)
        {
            tile.canBecomeNature = true;
        }
    }
}
