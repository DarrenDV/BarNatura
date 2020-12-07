using System.Collections.Generic;
using System.Linq;

public class TreeScript : BuildObject
{
    private List<BaseTileScript> surroundingTiles = new List<BaseTileScript>();
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
    public override void OnBuild()
    {
        base.OnBuild();
        surroundingTiles = GetNeigbourTiles(FindObjectOfType<AtmosphereSystem>().GetCurrentAtmosphereLevel() * 2 + 1);
    }
    public void UpdateSurroundingTiles(int radius)
    {
        surroundingTiles = GetNeigbourTiles(radius);
        foreach (BaseTileScript tile in surroundingTiles)
        {
            tile.canBecomeNature = true;
        }
    }

    private List<BaseTileScript> GetNeigbourTiles(int radius)
    {
        List<Tile> surroundingTiles = new List<Tile>();
        List<Tile> tempSurroundingTileList = new List<Tile>();
        List<Tile> tempTempList = new List<Tile>();

        surroundingTiles.Add(parentTile);
        tempSurroundingTileList.Add(parentTile);

        for (int i = 0; i< radius; i++) {
            foreach (Tile tile in tempSurroundingTileList)
            {
                List<Tile> otherTiles = tile.neighborTiles;
                foreach (Tile otherTile in otherTiles) {
                    if (!surroundingTiles.Contains(otherTile))
                    {
                        surroundingTiles.Add(otherTile);
                        tempTempList.Add(otherTile);
                    }
                }
            }
            tempSurroundingTileList.Clear();
            tempSurroundingTileList.AddRange(tempTempList);
            tempTempList.Clear();
        }
        return surroundingTiles.Cast<BaseTileScript>().ToList();
    }
}
