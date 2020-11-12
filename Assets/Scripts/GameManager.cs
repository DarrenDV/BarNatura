using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int oxygenGeneration;
    private int oxygenUsage;
    private int rawMaterial;
    private int buildingMaterial;

    [Header("Building")]
    public GameObject previewObjectParent;
    [HideInInspector] public GameObject buildObject;
    [HideInInspector] public GameObject buildObjectPreview;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Game Manager instance already set!");
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopBuilding();
        }
    }

    /// <summary>
    /// Use this when a new object has been placed that produces oxygen, like a new tree.
    /// </summary>
    /// <param name="oxygenGenerationToAdd">The amount of oxygen the object generates.</param>
    public void AddOxygenGeneration(int oxygenGenerationToAdd)
    {
        oxygenGeneration += oxygenGenerationToAdd;
    }

    /// <summary>
    /// Use this when an object that generates oxygen gets removed, when a tree gets removed for example.
    /// </summary>
    /// <param name="oxygenGenerationToRemove">The amount of oxygen generation to remove</param>
    public void RemoveOxygenGeneration(int oxygenGenerationToRemove)
    {
        oxygenGeneration -= oxygenGenerationToRemove;
    }

    /// <summary>
    /// Use this when a new object has been placed that uses oxygen, like a new human.
    /// </summary>
    /// <param name="oxygenUsageToAdd">The amount of oxygen the object uses.</param>
    public void AddOxygenUsage(int oxygenUsageToAdd)
    {
        oxygenUsage += oxygenUsageToAdd;
    }

    /// <summary>
    /// Use this when an object that uses oxygen gets removed, like when a human dies.
    /// </summary>
    /// <param name="oxygenUsageToRemove"></param>
    public void RemoveOxygenUsage(int oxygenUsageToRemove)
    {
        oxygenUsage -= oxygenUsageToRemove;
    }

    /// <summary>
    /// Get the surplus of oxygen
    /// </summary>
    public int OxygenSurplus()
    {
        return oxygenGeneration - oxygenUsage;
    }

    #region Building 

    public void ChangeBuildObject(GameObject newObject, GameObject previewObject)
    {
        buildObject = newObject;
        buildObjectPreview = previewObject;
        Instantiate(buildObjectPreview.gameObject, transform.position, transform.rotation, previewObjectParent.transform);
    }
        
    public void StopBuilding()
    {
        buildObject = null;

        if (previewObjectParent.transform.childCount != 0)
        {
            Destroy(previewObjectParent.transform.GetChild(0).gameObject);
        }

        previewObjectParent.transform.position = Vector3.zero;
        previewObjectParent.transform.rotation = Quaternion.identity;
    }

    #endregion
}
