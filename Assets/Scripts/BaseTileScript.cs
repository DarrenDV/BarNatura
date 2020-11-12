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

    [HideInInspector]public bool polluted;

    [Tooltip("The degree to which a tile is either polluted or nature")]
    [Range(-10, 10)]
    public int naturePollutedDegree = 0;

    float timerSpread;
    [SerializeField] float secondsToUpdate;
    public Gradient gradient;
    bool canBecomeNature = true;

    #endregion

    public GameObject RubbleTile;

    void Start()
    {
        int rubbleSpawnChange = 20;

        if (Random.Range(0, 100) == 0)
        {
            naturePollutedDegree = -10;
        }
        if (Random.Range(0, 100) < rubbleSpawnChange)
        {
            placeObject(Instantiate(RubbleTile, Vector3.zero, Quaternion.identity) );
        }
    }

    void Update(){
        Spread();
    }

    void Spread(){

        if(timerSpread >= secondsToUpdate){

            foreach(BaseTileScript neighbour in neighborTiles){
                //Only applies the spreading if the random is met.
                if(Random.Range(0,4) == 3){
                    ToxicSpreading(neighbour);
                    NatureSpreading(neighbour);
                }
            }
            
            timerSpread = 0;
        }
        timerSpread += Time.deltaTime;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.material.SetColor("_Color", gradient.Evaluate(Map(naturePollutedDegree, -10, 10, 0, 1)));
    }

    void ToxicSpreading(BaseTileScript neighbour){
            
        if(neighbour.naturePollutedDegree == -10 && naturePollutedDegree > -10){
            naturePollutedDegree--;

        }
        if(naturePollutedDegree == -10) canBecomeNature = false;
    }

    void NatureSpreading(BaseTileScript neighbour){

        if(neighbour.naturePollutedDegree == 10 && naturePollutedDegree < 10 && canBecomeNature){
            naturePollutedDegree++;
        }
    }
    public float Map (float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

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

                if(gameManager.buildObject.gameObject.CompareTag("Tree"))
                {
                    naturePollutedDegree = 10;
                }

                gameManager.StopBuilding();
            }
        }
    }

}
