using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    public int TutorialIndex;

    //bools for general missions
    private bool enoughTreesBuilt, firstHouseBuilt, firstFactoryBuilt, firstRubbleRemoved;

    //Tree mission variables`
    private int currentTrees;
    [Tooltip("How many trees are needed for the tree mission to progress?")]
    [SerializeField] private int neededTrees = 2;

    //Factory mission variables
    private bool hasBoosted = false, hasUnboosted = false;

    //Toxic mission variables
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject toxicTile;
    private bool canAssignToxic = true;
    private float cameraPanTimer;
    [Tooltip("After how many seconds does the camera start to pan?")]
    [SerializeField] private float whenCameraPans;
    private bool camIsMoving;
    Vector3 savedNormal;

    //List of all the tutorials
    public List<TutorialSequence> Tutorials;

    #endregion

    #region Default
    public void Start()
    {
        TutorialPopupScript.Instance.ShowNextTutorial();
        TutorialPopupScript.Instance.Next();
    }

    private void Update()
    {
        CameraMovement();
    }

    #endregion

    #region Checks
    //All these functions are to check when a certain milestone has been passed and progress the tutorial.

    public void OnSpaceShipBuilt()
    {
        TutorialPopupScript.Instance.Next();
    }

    public void OnTreeBuilt()
    {
        if (currentTrees + 1 == neededTrees) 
        {
            enoughTreesBuilt = true;
            TutorialPopupScript.Instance.Next();
        }

        if (!enoughTreesBuilt) currentTrees++;
    }

    public void OnFirstHouseBuilt()
    {
        if (!firstHouseBuilt)
        {
            TutorialPopupScript.Instance.Next();
            firstHouseBuilt = true;
        }
    }

    public void OnFirstFactoryBuilt()
    {
        if (!firstFactoryBuilt)
        {
            TutorialPopupScript.Instance.Next();
            firstFactoryBuilt = true;
        }
    }

    public void OnFirstRubbleRemoved()
    {
        if (!firstRubbleRemoved)
        {
            TutorialPopupScript.Instance.Next();
            firstRubbleRemoved = true;
        }
    }

    public void FactoryBoosted(bool boosting)
    {
        if (!hasBoosted && boosting)
        {
            TutorialPopupScript.Instance.Next();
            hasBoosted = true;
        }
        if(!hasUnboosted && !boosting)
        {
            TutorialPopupScript.Instance.Next();
            hasUnboosted = true;
        }
    }

    #endregion

    #region ToxicMission

    /// <summary>
    /// Starts everything related to the toxic mission
    /// </summary>
    public void ToxicMission()
    {
        SpawnToxicTile();
    }

    /// <summary>
    /// Spawns multiple toxic tiles
    /// </summary>
    public void SpawnToxicTile()
    {
        foreach (var tile in FindObjectsOfType<BaseTileScript>())
        {
            tile.SpawnRandomPolution();
        }
    }

    /// <summary>
    /// Gets the first toxic tile created and assings it here for rotation
    /// </summary>
    /// <param name="givenToxicTile"></param>
    public void AssignToxicTile(GameObject givenToxicTile)
    {
        if (canAssignToxic)
        {
            toxicTile = givenToxicTile;
            canAssignToxic = false;
            camIsMoving = true;
        }
    }

    /// <summary>
    /// Moves the camera to the needed tile
    /// </summary>
    void CameraMovement()
    {
        if (camIsMoving)
        {
            mainCam.GetComponent<CameraScript>().enabled = false; //Turns off camera script so the player can't mess the sequence up
            CameraPan();
        }
        else
        {
            mainCam.GetComponent<CameraScript>().enabled = true; //Turns the camera script back on when the player can't mess it up anymore
        }
    }

    /// <summary>
    /// Pans the camera to the toxic tile
    /// </summary>
    private void CameraPan()
    {
        //Timer to control rotating
        if (cameraPanTimer < 1)
        {
            savedNormal = GetNormal(mainCam.transform.position, toxicTile.transform.position); //Get the original normal for later reference.
        }

        if (cameraPanTimer >= whenCameraPans)
        {
            //Rotates around the planet toward the toxic tile.
            mainCam.transform.RotateAround(toxicTile.transform.up, GetNormal(mainCam.transform.position, toxicTile.transform.position), 20 * Time.deltaTime);
        }

        cameraPanTimer += Time.deltaTime;

        //If the normal isn't the same normal anymore the camera stops moving on it's own.
        if (savedNormal != GetNormal(mainCam.transform.position, toxicTile.transform.position))
        {
            camIsMoving = false;
        }
    }

    /// <summary>
    /// Gets the normal of the cross product between the target and current camera position
    /// </summary>
    /// <param name="movingObject"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private Vector3 GetNormal(Vector3 movingObject, Vector3 target)
    {
        //Vector3 side1 = target - movingObject;
        //Vector3 side2 = Vector3.zero - movingObject;

        return Vector3.Cross(movingObject, target).normalized;
    }

    #endregion
}