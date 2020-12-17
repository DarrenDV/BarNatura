using System.Collections.Generic;
using UnityEngine;

public class ToxicCleaner : BuildingScript
{
    #region Variables

    [Header("Toxic Cleaner")]
    [SerializeField] private float cleanRate;
    private float timeSinceLastClean;
    [SerializeField] private int cleanDistance;

    private List<BaseTileScript> surroundingTiles = new List<BaseTileScript>();

    #endregion

    #region Strings

    public override string GetName()
    {
        return "Toxic cleaner";
    }

    protected override string GetBuildingFunction()
    {
        return $"Cleans {GetIcon("Toxic")} tiles";
    }

    public override string GetDescription()
    {
        return $"This toxic cleaner cleans toxicity up to {cleanDistance} tiles away every {cleanRate} seconds.";
    }

    #endregion

    #region Defaults

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
                    tile.IncreaseNaturePollutedDegree(1);
                }
            }

            timeSinceLastClean = 0;
        }
    }

    #endregion

    #region Buildings

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        surroundingTiles = parentTile.GetNeighbourTiles(cleanDistance);
    }

    #endregion
}
