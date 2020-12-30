using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    public int TutorialIndex;

    private bool enoughTreesBuilt, firstHouseBuilt, firstFactoryBuilt, firstRubbleRemoved;

    //Tree mission variables
    private int currentTrees;
    [SerializeField] private int neededTrees = 2;

    //Factory mission variables
    private bool hasBoosted = false, hasUnboosted = false;

    //Toxic mission variables
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject toxicTile;
    private bool canAssignToxic = true;
    private float cameraPanTimer; 
    [Header("After how many seconds does the camera start to pan?")]
    [SerializeField] private float whenCameraPans;
    bool canPan;

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
        //CameraPan in the update function so it runs alltime as that's needed for this.
        CameraPan();

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
        canPan = true;
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
        }
    }

    /// <summary>
    /// Pans the camera to the toxic tile and zooms in
    /// </summary>
    public void CameraPan() 
    {
        if (canPan)
        {
            //Simple timer so it doesn't instantly start rotating when there isn't a tile to rotate to
            if (cameraPanTimer >= whenCameraPans)
            {
                //This rotatearound seems to work
                mainCam.transform.RotateAround(toxicTile.transform.up, GetNormal(mainCam.transform.position, toxicTile.transform.position), 20 * Time.deltaTime);
            }

            cameraPanTimer += Time.deltaTime;

            //TODO
            //Camera needs to stop rotating on it own - set canPan to false

            //Test for stopping rotation on its own, doens't work.
            if(mainCam.transform.position == toxicTile.transform.up)
            {
                canPan = false;
            }
        }
        else
        {
            //TODO
            //Zoom camera using FOV
        }
    }

    /// <summary>
    /// Gets the normal of the cross product between the target and current camera position
    /// </summary>
    /// <param name="movingObject"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    Vector3 GetNormal(Vector3 movingObject, Vector3 target)
    {
        //Vector3 side1 = target - movingObject;
        //Vector3 side2 = Vector3.zero - movingObject;

        return Vector3.Cross(movingObject, target).normalized;
    }

    #endregion
}