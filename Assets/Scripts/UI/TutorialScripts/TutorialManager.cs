using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    [Header("Tutorial")]
    [Tooltip("Current tutorial mission index")]
    public int TutorialIndex;
    [Tooltip("List of tutorials")]
    public List<TutorialSequence> Tutorials;

    //bools for general missions
    private bool enoughTreesBuilt, firstHouseBuilt, firstFactoryBuilt, firstRubbleRemoved;

    [Header("Various mission variables")]
    //Tree mission variables
    [Tooltip("Trees needed to progress tree mission")]
    [SerializeField] private int neededTrees = 2;
    private int currentTrees;

    //Factory mission variables
    private bool hasBoosted = false, hasUnboosted = false;

    //Toxic mission variables
    [Header("Toxic mission variables")]
    [SerializeField] private GameObject mainCam;
    private GameObject toxicTile;
    private bool canAssignToxic = true;
    private float cameraPanTimer;
    [Tooltip("After how many seconds the camera starts to pan")]
    [SerializeField] private float whenCameraPans;
    public bool camIsMoving;
    Vector3 savedNormal;

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

        //Quick debug style way to go trough the tutorial, remove this when done
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            TutorialPopupScript.Instance.Next();
        }
        #endif
    }

    #endregion

    #region Checks

    /// <summary>
    /// Runs an switch check ("action") on which mission is completed. 
    /// </summary>
    /// <param name="action"></param>
    public void ProgressTutorial(string action)
    {
        if (!GameManager.Instance.tutorialEnded) //Checks if the tutorial has ended
        {
            switch (action)
            {
                case "SpaceShipBuilt":
                    TutorialPopupScript.Instance.Next();
                    break;

                case "TreeBuilt":
                    if (currentTrees + 1 == neededTrees && !enoughTreesBuilt)
                    {
                        enoughTreesBuilt = true;
                        TutorialPopupScript.Instance.Next();
                    }

                    if (!enoughTreesBuilt) currentTrees++;
                    break;

                case "HouseBuilt":
                    if (!firstHouseBuilt)
                    {
                        TutorialPopupScript.Instance.Next();
                        firstHouseBuilt = true;
                    }
                    break;

                case "RubbleRemoved":
                    if (!firstRubbleRemoved && TutorialIndex == 4) //Needs to be tutorialIndex 4, player can just remove rubble at any time.
                    {
                        TutorialPopupScript.Instance.Next();
                        firstRubbleRemoved = true;
                    }
                    break;

                case "FactoryBuilt":
                    if (!firstFactoryBuilt)
                    {
                        TutorialPopupScript.Instance.Next();
                        firstFactoryBuilt = true;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Check when the factory is boosting and unboosting
    /// </summary>
    /// <param name="boosting"></param>
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
    /// Starts everything related to the toxic mission.
    /// </summary>
    public void ToxicMission()
    {
        SpawnToxicTiles();
    }

    /// <summary>
    /// Spawns multiple toxic tiles.
    /// </summary>
    public void SpawnToxicTiles()
    {
        var placedToxic = 0;

        while (placedToxic < FindObjectOfType<TileVariables>().ToxicTilesToSpawn) //Check if enough tiles are spawned, else spawn more
        {
            var randomFreeTile = GameManager.Instance.GetRandomFreeTile();

            if (randomFreeTile.TrySpawnPollution())
            {
                placedToxic++;
            }
        }
    }

    /// <summary>
    /// Gets the first toxic tile created and assings it here for rotation.
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

    #region OnClick and OnPanel

    /// <summary>
    /// Ends the tutorial
    /// </summary>
    public void EndTutorial()
    {
        gameObject.SetActive(false);
        GameManager.Instance.tutorialEnded = true;
    }

    /// <summary>
    /// Function to stop automatic camera movement, needs to be a solo function because unity doesn't allow direct boolean editing.
    /// </summary>
    public void StopCamMovement()
    {
        camIsMoving = false;
    }

    #endregion
}