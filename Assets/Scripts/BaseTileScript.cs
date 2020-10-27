using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTileScript : Tile
{
    [Header("Default tile production variables")]
    [SerializeField] protected float oxygenProduction;
    [SerializeField] protected float pollutionProduction;

    [Header("Tile pollution state variables")]
    protected float maxPollutedPercentage = 100f;
    [SerializeField] protected float pollutedPercentage;

    [Header("Tile nature state variables")]
    protected float maxNaturePercentage = 100f;
    [SerializeField] protected float naturePercentage;

    [HideInInspector]public bool isOccupied;
    [HideInInspector]public bool polluted;

    private GameManager gameManager;

    [Tooltip("The degree to which a tile is either polluted or nature")]
    [Range(-10, 10)]
    public int naturePollutedDegree = 0;
    public int pollutedDegree = 0;

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
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        if (gameManager.buildObject != null)
        {
            //FIX: Change the colors of the material based whether or not this tile is occupied
            //gameManager.buildObjectPreview.gameObject.GetComponent<BuildingModeObject>().ChangeMaterial(isOccupied);
            gameManager.previewObjectParent.transform.position = transform.position;
            gameManager.previewObjectParent.transform.rotation = transform.rotation;
        }
    }

    private void OnMouseOver()
    {
        if (gameManager.buildObject != null)
        {
            if (Input.GetMouseButtonDown(0) && !isOccupied)
            {
                //Place the new building
                placeObject(Instantiate(gameManager.buildObject.gameObject, transform.position, transform.rotation));

                isOccupied = true;
                gameManager.StopBuilding();
            }
        }
    }
}
