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
    
    [HideInInspector]public bool occupied;
    private GameManager gameManager;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This could be in another function or method
        if(pollutedPercentage >= maxPollutedPercentage){
            //Make tile uninhabitable or toxic.
        }

        //This could be in another function or method
        if(naturePercentage >= maxNaturePercentage){
            //Make tile nature.
        }
    }

    private void OnMouseOver()
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();

        if (gameManager.objectToBuild != null)
        {
            if (Input.GetMouseButtonDown(0) && !occupied)
            {
                Instantiate(gameManager.objectToBuild.gameObject, transform.position, transform.rotation);
                occupied = true;
                gameManager.StopBuilding();
            }
        }
    }
}
