using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTileScript : Tile
{

    #region Spreading Variables

    [Header("Tile pollution state variables")]
    protected float maxPollutedPercentage = 100f;

    [SerializeField] protected float pollutedPercentage;

    [Header("Tile nature state variables")]
    protected float maxNaturePercentage = 100f;

    [SerializeField] protected float naturePercentage;

    [Tooltip("The degree to which a tile is either polluted or nature")] [Range(-10, 10)]
    public int naturePollutedDegree; // make property so it updates the materials automatic

    float timerSpread;
    float secondsToUpdate;
    Gradient gradient;
    bool canBecomeNature = true;

    #endregion

    GameObject RubbleTile;
    int rubbleSpawnChance;
    GameObject lavaTile;
    int lavaChance;

    private MeshRenderer meshRenderer;
    private bool doMaterialUpdate;

    protected override void Start()
    {
        base.Start();

        Assigning();

        meshRenderer = GetComponent<MeshRenderer>();
        
        Spawning();
    }

    protected override void Update()
    {
        base.Update();
        Spread();
    }

    //Toxic tile and natural tile spawning
    void Spawning()
    {
        //Toxic tile spawning
        if (Random.Range(0, 100) == 0)
        {
            naturePollutedDegree = -10;
            doMaterialUpdate = true;
        }

        //Rubble tile spawning
        if (Random.Range(0, 100) < rubbleSpawnChance)
        {
            placeObject(Instantiate(RubbleTile, Vector3.zero, Quaternion.identity));
        }

        //Places lava on 1 in every 100 tiles.
        if (Random.Range(0, lavaChance) == 0 && !isOccupied)
        {
            placeObject(Instantiate(lavaTile, Vector3.zero, Quaternion.identity));
        }
    }

    //Assigns variables from a singular variable script
    void Assigning()
    {
        //Get tileVariables.cs
        TileVariables tileVariables = GameObject.Find("TileVariables").GetComponent<TileVariables>();

        //Follow what is done here for every static variable
        gradient = tileVariables.gradient;
        RubbleTile = tileVariables.rubbleTile;
        rubbleSpawnChance = tileVariables.rubbleSpawnChance;
        lavaTile = tileVariables.lavaTile;
        lavaChance = tileVariables.lavaSpawnChance;
        secondsToUpdate = tileVariables.secondsToUpdate;

    }

    #region Tile Spreading

    void Spread()
    {
        if (timerSpread >= secondsToUpdate)
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

        //if (naturePollutedDegree != 0)
        //{
        timerSpread += Time.deltaTime;
        //}

        if (doMaterialUpdate)
        {
            meshRenderer.material.SetColor("_Color", gradient.Evaluate(Map(naturePollutedDegree, -10, 10, 0, 1)));
            doMaterialUpdate = false;
        }
    }

    void ToxicSpreading(BaseTileScript neighbour)
    {

        if (neighbour.naturePollutedDegree == -10 && naturePollutedDegree > -10)
        {
            naturePollutedDegree--;
            doMaterialUpdate = true;
        }

        if (naturePollutedDegree == -10)
        {
            canBecomeNature = false;
        }
    }

    void NatureSpreading(BaseTileScript neighbour)
    {
        if (neighbour.naturePollutedDegree == 10 && naturePollutedDegree < 10 && canBecomeNature)
        {
            naturePollutedDegree++;
            doMaterialUpdate = true;
        }
    }
    public float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    #endregion

    #region PollutionChecks

    public bool PolutionLevelCheck()
    {
        if (pollutedPercentage >= maxPollutedPercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool NatureLevelCheck()
    {
        if (naturePercentage >= maxNaturePercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region BuildingPlacement
    private void OnMouseEnter()
    {
        var gameManager = GameManager.Instance;

        if (gameManager.buildObject != null)
        {
            //Set the position of the preview building to this tile
            gameManager.buildObjectPreview.gameObject.GetComponent<BuildingModeObject>().ChangeMaterial(isOccupied);
            gameManager.previewObjectParent.transform.position = transform.position;
            gameManager.previewObjectParent.transform.rotation = transform.rotation;
        }
    }

    private void OnMouseOver()
    {
        var gameManager = GameManager.Instance;

        if (gameManager.buildObject != null)
        {
            if (Input.GetMouseButtonDown(0) && !isOccupied && naturePollutedDegree >= 0 && gameManager.IsPointerOverUIElement() == false)
            {
                //Place the new building
                placeObject(Instantiate(gameManager.buildObject.gameObject, transform.position, transform.rotation));
                gameManager.ChangeBuildingMaterial(gameManager.buildObject.gameObject.GetComponent<BuildObject>().buildCost);

                if (gameManager.buildObject.gameObject.CompareTag("Tree"))
                {
                    naturePollutedDegree = 10;
                    doMaterialUpdate = true;
                }

                gameManager.StopBuilding();
            }
        }
    }
    #endregion
}
