﻿using System.Collections.Generic;
using System.Linq;

public class TreeScript : BuildObject
{
    private List<BaseTileScript> surroundingTiles = new List<BaseTileScript>();

    protected override void Start()
    {
        base.Start();

        if (!GameManager.Instance.tutorialEnded) FindObjectOfType<TutorialManager>().OnTreeBuilt();
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
        parentTile.SetNaturePollutedDegree(10);
    }

    /// <summary>
    /// Called when the atmosphere changes
    /// </summary>
    public void UpdateNatureSpreadTiles(int currentAtmosphereLevel)
    {
        var radius = currentAtmosphereLevel * 2 + 1;

        surroundingTiles = parentTile.GetNeighbourTiles(radius);

        foreach (var tile in surroundingTiles)
        {
            tile.canBecomeNature = true;
        }
    }
}
