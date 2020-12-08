using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class BaseTileScript : Tile
{
    #region Spreading Variables

    [Header("Base Tile Script")]
    [Tooltip("The degree to which a tile is either polluted or nature.")]
    [Range(-10, 10)]
    [SerializeField]
    public int naturePollutedDegree;

    private float timerSpread;
    public bool canBecomeNature = false;

    #endregion

    [Tooltip("If true, the starting spaceship will be spawned on this tile.")]
    [SerializeField] private bool isStartingLocation = false;

    private MeshRenderer meshRenderer;
    private bool doMaterialUpdate;

    private TileVariables tileVariables;

    private WinLose winLose;
    private bool canAdd = true;

    bool canAddParticles = true;

    protected override void Start()
    {
        base.Start();

        tileVariables = FindObjectOfType<TileVariables>();
        winLose = FindObjectOfType<WinLose>();

        meshRenderer = GetComponent<MeshRenderer>();

        SetStartingColor();
        Spawning();
    }

    private void SetStartingColor()
    {
        meshRenderer.material.SetColor("_Color", tileVariables.gradient.Evaluate(0.5f));
    }

    protected override void Update()
    {
        base.Update();

        Spread();

        CheckTile();
    }

    public void SetNaturePollutedDegree(int newNaturePollutedDegree)
    {
        naturePollutedDegree = newNaturePollutedDegree;
        doMaterialUpdate = true;
    }

    public void UpdateNaturePollutedDegree(int newNaturePollutedDegree)
    {
        naturePollutedDegree += newNaturePollutedDegree;
        doMaterialUpdate = true;
    }

    public override void PlaceObject(GameObject obj)
    {
        obj.GetComponent<BaseObject>().parentTile = this;

        base.PlaceObject(obj);
    }

    /// <summary>
    /// Toxic tile and natural tile spawning.
    /// </summary>
    private void Spawning()
    {
        if (isStartingLocation)
        {
            PlaceObject(Instantiate(tileVariables.startingSpaceShip, Vector3.zero, Quaternion.identity));
            
            // we don't want to spawn anything else on this tile, that is why we return out of this function here.
            return;
        }

        //Toxic tile spawning
        if (Random.Range(0, 100) < tileVariables.toxicTileChance)
        {
            SetNaturePollutedDegree(-10);
            ToxicParticles();
        }

        //Rubble tile spawning
        if (Random.Range(0, 100) < tileVariables.rubbleSpawnChance)
        {
            PlaceObject(Instantiate(tileVariables.rubbleTile, Vector3.zero, Quaternion.identity));
        } 
        else
        {
            //Places lava on 1 in every 100 tiles.
            if (Random.Range(0, 100) < tileVariables.lavaSpawnChance && !isOccupied)
            {
                PlaceObject(Instantiate(tileVariables.lavaTile, Vector3.zero, Quaternion.identity));
            }
        }
    }

    #region Tile Spreading

    private void Spread()
    {
        if (timerSpread >= tileVariables.secondsToUpdate)
        {
            foreach (var tile in neighborTiles)
            {
                var neighbour = (BaseTileScript) tile;

                //Only applies the spreading if the random is met.
                if (Random.Range(0, 4) == 0)
                {
                    ToxicSpreading(neighbour);
                    NatureSpreading(neighbour);
                }
            }

            timerSpread = 0;
        }

        timerSpread += Time.deltaTime;

        if (doMaterialUpdate)
        {
            meshRenderer.material.SetColor("_Color", tileVariables.gradient.Evaluate(Utils.Map(naturePollutedDegree, -10, 10, 0, 1)));
            doMaterialUpdate = false;
        }
    }

    private void ToxicSpreading(BaseTileScript neighbour)
    {
        if (neighbour.naturePollutedDegree == -10 && naturePollutedDegree > -10)
        {
            naturePollutedDegree--;
            doMaterialUpdate = true;
        }

        if (naturePollutedDegree == -10)
        {
            ToxicParticles();
            canBecomeNature = false;
        }
    }

    private void NatureSpreading(BaseTileScript neighbour)
    {
        if (neighbour.naturePollutedDegree == 10 && naturePollutedDegree < 10 && canBecomeNature)
        {
            naturePollutedDegree++;
            doMaterialUpdate = true;
        }
    }


    private void ToxicParticles()
    {
        //Gives toxic particles to tiles when they become completely toxic
        if (canAddParticles)
        {
            GameObject ps = Instantiate(tileVariables.toxicParticles, Vector3.zero, Quaternion.identity);
            ps.transform.SetParent(gameObject.transform);
            ps.transform.localPosition = Vector3.zero;
            ps.transform.localRotation = Quaternion.identity;

            canAddParticles = false;
        }
    }

    #endregion

    #region BuildingPlacement

    private void OnMouseEnter()
    {
        var gameManager = GameManager.Instance;

        if (gameManager.inBuildMode)
        {
            //Set the position of the preview building to this tile
            gameManager.buildObjectPreview.gameObject.GetComponent<BuildingModeObject>().ChangeMaterial(isOccupied || naturePollutedDegree < 0);
            gameManager.MovePreview(transform.position, transform.rotation);
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        var gameManager = GameManager.Instance;

        if (gameManager.inBuildMode)
        {
            if (Utils.IsPointerOverUIElement())
            {
                // don't build when the user clicked on the ui

                return;
            }

            if (!isOccupied)
            {
                var plannedBuildObject = gameManager.buildObject.gameObject.GetComponent<BuildObject>();

                // check if the tile is healthy enough
                if (naturePollutedDegree >= plannedBuildObject.MinimumNaturePollutedDegree)
                {
                    // place the new building
                    var newBuilding = Instantiate(gameManager.buildObject.gameObject, transform.position, transform.rotation);
                    var newBuildingObject = newBuilding.GetComponent<BuildObject>();
                    
                    PlaceObject(newBuilding);
                    newBuildingObject.OnBuild();

                    AudioManager.Instance.PlayBuildSound();

                    gameManager.RemoveBuildingMaterial(newBuildingObject.BuildCost);
                    gameManager.StopBuildingMode();
                }
            }
            else
            {
                AudioManager.Instance.PlayBuildFaliedSound();
            }
        }
    }

    #endregion

    void CheckTile()
    {
        if (canAdd) {
            if (naturePollutedDegree == 10)
            {
                winLose.AddTile(true, false);
                canAdd = false;
            }
            else if(naturePollutedDegree == -10)
            {
                winLose.AddTile(false, true);
                canAdd = false;
            }
        }
        else
        {
            if (naturePollutedDegree != 10 && naturePollutedDegree != -10)
            {
                canAdd = true;
            }
        }
    }

    public List<BaseTileScript> GetNeighbourTiles(int radius, bool includeParentTile = true)
    {
        var surroundingTiles = new List<Tile>();
        var tempSurroundingTileList = new List<Tile>();
        var newIterationTiles = new List<Tile>();

        surroundingTiles.Add(this);
        tempSurroundingTileList.Add(this);

        for (var i = 0; i < radius; i++)
        {
            foreach (var tile in tempSurroundingTileList)
            {
                var otherTiles = tile.neighborTiles;

                foreach (var otherTile in otherTiles)
                {
                    if (!surroundingTiles.Contains(otherTile))
                    {
                        surroundingTiles.Add(otherTile);
                        newIterationTiles.Add(otherTile);
                    }
                }
            }

            tempSurroundingTileList.Clear();

            tempSurroundingTileList.AddRange(newIterationTiles);
            newIterationTiles.Clear();
        }

        if (!includeParentTile)
        {
            surroundingTiles.Remove(this);
        }

        return surroundingTiles.Cast<BaseTileScript>().ToList();
    }
}
