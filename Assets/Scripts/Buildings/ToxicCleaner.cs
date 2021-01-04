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
        return "Toxic Cleaner";
    }

    protected override string GetBuildingFunction()
    {
        return $"Cleans {HudManager.GetIcon("Toxic")} tiles";
    }

    public override string GetDescription()
    {
        return $"This {GetName()} cleaner cleans {HudManager.GetIcon("Toxic")} up to {cleanDistance} tiles away every {cleanRate} seconds.\n\n{ShowProgress()}";
    }

    #endregion

    #region Defaults

    protected override void Update()
    {
        base.Update();

        isProducing = true;
        timeSinceLastClean += Time.deltaTime;

        if (timeSinceLastClean >= cleanRate)
        {
            foreach (var tile in surroundingTiles)
            {
                if (tile.NaturePollutedDegree < 0)
                {
                    tile.AddNaturePollutedDegree(1);
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

    #region Production

    protected override float GetMaxTime()
    {
        return cleanRate;
    }

    protected override float GetTimer()
    {
        return timeSinceLastClean;
    }

    protected override string GetResourceDrained()
    {
        return $"{HudManager.GetIcon("Toxic")}";
    }

    protected override string GetResourceGain()
    {
        return $"{HudManager.GetIcon("Nature")}";
    }

    #endregion
}
