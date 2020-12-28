using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class BaseTileScript : Tile
{
    #region Nature / Toxic Variables

    //[Header("Base Tile Script")]
    //[Tooltip("The degree to which a tile is either polluted or nature.")]
    //[Range(-10, 10)]
    //[SerializeField]
    private int naturePollutedDegree;

    [Header("Base Tile Script")]
    public UnityEvent OnNaturePollutedDegreeChangedEvent;

    private float timerSpread;
    public bool canBecomeNature;

    private MeshRenderer meshRenderer;
    private bool doMaterialUpdate;

    private bool canAddParticles = true;
    private GameObject toxicParticles;

    #endregion

    //[Tooltip("If true, the starting spaceship will be spawned on this tile.")]
    //[SerializeField] private bool isStartingLocation = false;

    #region Other variables

    private TileVariables tileVariables;

    //Winlose variables
    private WinLose winLose;
    private bool canAddToWinLose = true;
    private bool isToxic;
    private bool isNature;

    #endregion

    #region Default

    private void Awake()
    {
        tileVariables = FindObjectOfType<TileVariables>();
        winLose = FindObjectOfType<WinLose>();

        meshRenderer = GetComponent<MeshRenderer>();

        OnNaturePollutedDegreeChangedEvent.AddListener(OnNatureDegreeChanged);
    }

    protected override void Start()
    {
        base.Start();

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
    }

    /// <summary>
    /// Get the specied neibour tiles according to the radius.
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="includeParentTile"></param>
    /// <returns></returns>
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

    private void OnNatureDegreeChanged()
    {
        UpdateDecals();
        CheckTile();
    }

    private void UpdateDecals()
    {
        if (naturePollutedDegree <= 0)
        {
            // remove all decals
            var decalsToRemove = PlacedObjects.Where(placedObject => placedObject.CompareTag("NatureDecal")).ToList();

            for (var i = 0; i < decalsToRemove.Count; i++)
            {
                DeletePlacedObject(decalsToRemove[i], false);
            }
        }
        else
        {
            var currentDecalAmount = PlacedObjects.Count(placedObject => placedObject.CompareTag("NatureDecal"));
            var decalDiff = naturePollutedDegree - currentDecalAmount;

            // we need to place more
            if (decalDiff > 0)
            {
                // we don't want any decals on tiles with a lava lake on it
                if (PlacedObjects.Count > 0 && PlacedObjects[0].GetComponent<LavaLake>() != null)
                {
                    return;
                }

                for (var i = 0; i < decalDiff; i++)
                {
                    var newDecal = Instantiate(tileVariables.tileDecals[Random.Range(0, tileVariables.tileDecals.Length)], transform);
                    PlaceObject(newDecal, false);

                    var rand = Random.insideUnitCircle * 0.065f;
                    newDecal.transform.localPosition = new Vector3(rand.x, 0f, rand.y);
                }
            }
            else // we need to remove some decals
            {
                // remove x amount
                for (var i = 0; i < Mathf.Abs(decalDiff); i++)
                {
                    // find placed decal object
                    var decalToRemove = PlacedObjects.First(x => x.CompareTag("NatureDecal"));

                    // and delete it
                    DeletePlacedObject(decalToRemove, false);
                }
            }
        }
    }

    #endregion

    #region Spawning

    /// <summary>
    /// Toxic tile and natural tile spawning.
    /// </summary>
    private void Spawning()
    {
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

    public void SpawnRandomPolution()
    {
        // can't place pollution when the tile is occupied
        if (isOccupied)
        {
            return;
        }

        //Toxic tile spawning
        if (Random.Range(0, 100) == tileVariables.toxicTileChance)
        {
            SetNaturePollutedDegree(-10);
            ToxicParticles();
        }
    }
    #endregion

    #region Tile Spreading

    /// <summary>
    /// Spreads the toxic and nature tiles every few seconds to every tile around it on a chance basis.
    /// </summary>
    private void Spread()
    {
        if (timerSpread >= tileVariables.secondsToUpdate)
        {
            foreach (var tile in neighborTiles)
            {
                var neighbour = (BaseTileScript) tile;

                //Only applies the spreading if the random is met.
                if (Random.Range(0, tileVariables.maxChance) <= tileVariables.natureChance)
                {
                    NatureSpreading(neighbour);
                }
                if(Random.Range(0, tileVariables.maxChance) <= tileVariables.toxicChance)
                {
                    ToxicSpreading(neighbour);
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
            AddNaturePollutedDegree(-1);
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
            AddNaturePollutedDegree(1);
            CheckRemoveToxicParticles();
        }
    }

    public void SetNaturePollutedDegree(int newNaturePollutedDegree)
    {
        naturePollutedDegree = newNaturePollutedDegree;
        OnNaturePollutedDegreeChangedEvent.Invoke();
        doMaterialUpdate = true;

        CheckRemoveToxicParticles();
    }

    public void AddNaturePollutedDegree(int newNaturePollutedDegree)
    {
        naturePollutedDegree += newNaturePollutedDegree;
        OnNaturePollutedDegreeChangedEvent.Invoke();
        doMaterialUpdate = true;

        CheckRemoveToxicParticles();
    }

    /// <summary>
    /// Checks if the tile isn't toxic anymore and removes the particle effect if so.
    /// </summary>
    private void CheckRemoveToxicParticles()
    {
        if (naturePollutedDegree >= 0 && toxicParticles != null)
        {
            Destroy(toxicParticles);
        }
    }

    /// <summary>
    /// Spawn toxic particles.
    /// </summary>
    private void ToxicParticles()
    {
        //Gives toxic particles to tiles when they become completely toxic
        if (canAddParticles)
        {
            toxicParticles = Instantiate(tileVariables.toxicParticles, Vector3.zero, Quaternion.identity);
            toxicParticles.transform.SetParent(gameObject.transform);
            toxicParticles.transform.localPosition = Vector3.zero;
            toxicParticles.transform.localRotation = Quaternion.identity;

            canAddParticles = false;
        }
    }

    public int NaturePollutedDegree => naturePollutedDegree;

    #endregion

    #region BuildingPlacement

    public override void OnMouseEnter()
    {
        var gameManager = GameManager.Instance;

        if (gameManager.inBuildMode)
        {
            var plannedBuildObject = gameManager.buildObject.gameObject.GetComponent<BuildObject>();

            //Set the position of the preview building to this tile
            gameManager.buildObjectPreview.gameObject.GetComponent<BuildingModeObject>().ChangeMaterial(isOccupied || plannedBuildObject.MinimumNaturePollutedDegree > naturePollutedDegree);
            gameManager.MovePreview(transform.position, transform.rotation);
        }

        if (gameManager.CurrentGameState == Enums.GameState.SelectLocation)
        {
            if (!isOccupied)
            {
                Pointer.instance.setPointer(PointerStatus.TILE, FaceCenter, transform.up);
            }
        }
    }

    public override void OnMouseExit()
    {
        if (GameManager.Instance.CurrentGameState == Enums.GameState.SelectLocation)
        {
            Pointer.instance.unsetPointer();
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        var gameManager = GameManager.Instance;

        if (gameManager.CurrentGameState == Enums.GameState.SelectLocation)
        {
            if (!isOccupied)
            {
                gameManager.OnStartingLocationSelected(this);
            }
            else
            {
                AudioManager.Instance.PlayBuildFaliedSound();
                SelectStartingLocationUiManager.Instance.ShowBuildOnFreeSpaceMessage();
            }

            return;
        }

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

    public override void PlaceObject(GameObject obj, bool updateOccupied = true)
    {
        obj.GetComponent<BaseObject>().parentTile = this;

        base.PlaceObject(obj, updateOccupied);
    }

    public void PlaceStartingSpaceShip()
    {
        PlaceObject(Instantiate(tileVariables.startingSpaceShip, Vector3.zero, Quaternion.identity));
    }

    #endregion

    #region WinLose
    /// <summary>
    /// Checks if the tile is completely nature or toxic and adds it to the WinLose calculation. Also removes it if it isn't nature or toxic anymore.
    /// </summary>
    private void CheckTile()
    {
        //Checks if the tile can be added.
        if (canAddToWinLose) {   

            //Checks for nature tile
            if (naturePollutedDegree == 10)
            {
                winLose.AddTile(0);
                isNature = true;

                canAddToWinLose = false; 
            }

            //Checks toxic tile.
            else if(naturePollutedDegree == -10)
            {
                winLose.AddTile(1);
                isToxic = true;

                canAddToWinLose = false;
            }
        }

        //Checks when the tile isn't toxic anymore and removes it if nessecary
        else
        {
            if (naturePollutedDegree != 10 && naturePollutedDegree != -10)
            {
                if (isNature)
                {
                    winLose.RemoveTile(0);
                    isNature = false;
                }
                if (isToxic)
                {
                    winLose.RemoveTile(1);
                    isToxic = false;
                }

                canAddToWinLose = true;
            }
        }
    }

    #endregion
}