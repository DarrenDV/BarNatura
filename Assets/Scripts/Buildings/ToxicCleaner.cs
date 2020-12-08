using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicCleaner : BuildingScript
{
    [Header("Toxic Cleaner")]
    [SerializeField] private float cleanRate;
    private float timeSinceLastClean;
    [SerializeField] private int cleanDistance;

    private List<BaseTileScript> surroundingTiles = new List<BaseTileScript>();


    public override string GetName()
    {
        return "Toxic cleaner";
    }

    public override string GetDescription()
    {
        return $"This toxic cleaner cleans toxicity up to {cleanDistance} tiles away every {cleanRate} seconds.";
    }

    protected override void Update()
    {
        base.Update();

        timeSinceLastClean += Time.deltaTime;

        if (timeSinceLastClean >= cleanRate)
        {
            foreach (var tile in surroundingTiles)
            {
                if (tile.naturePollutedDegree < 0)
                {
                    tile.UpdateNaturePollutedDegree(1);
                }
            }
            timeSinceLastClean = 0;
        }

    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();
        surroundingTiles = parentTile.GetNeighbourTiles(cleanDistance);
    }

}
