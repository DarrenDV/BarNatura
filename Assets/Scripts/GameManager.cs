using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int oxygenGeneration;
    private int oxygenUsage;
    private int pollution;
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

    #region Oxygen

    /// <summary>
    /// Use this when a new object has been placed that produces oxygen, like a new tree.
    /// </summary>
    /// <param name="oxygenGenerationToAdd">The amount of oxygen the object generates.</param>
    public void AddOxygenGeneration(int oxygenGenerationToAdd)
    {
        oxygenGeneration += oxygenGenerationToAdd;
    }

    /// <summary>
    /// Use this when an object that generates oxygen gets removed, when a tree dies for example.
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
    public int GetOxygenSurplus()
    {
        return oxygenGeneration - oxygenUsage;
    }

    /// <summary>
    /// Get the current amount of generated oxygen.
    /// </summary>
    /// <returns></returns>
    public int GetOxygenGeneration()
    {
        return oxygenGeneration;
    }

    /// <summary>
    /// Get the current amount of oxygen usage.
    /// </summary>
    /// <returns></returns>
    public int GetOxygenUsage()
    {
        return oxygenUsage;
    }

    #endregion

    #region Pollution

    /// <summary>
    /// Use this when gets added to the world, like when a geyser gets spawned or when a factory gets build.
    /// </summary>
    /// <param name="pollutionToAdd"></param>
    public void AddPollution(int pollutionToAdd)
    {
        oxygenGeneration += pollutionToAdd;
    }

    /// <summary>
    /// Use this when pollution gets removed, like when a geyser or factory gets removed.
    /// </summary>
    /// <param name="pollutionToRemove"></param>
    public void RemovePollution(int pollutionToRemove)
    {
        oxygenGeneration -= pollutionToRemove;
    }

    /// <summary>
    /// Get the current amount of pollution.
    /// </summary>
    /// <returns></returns>
    public int GetPollution()
    {
        return pollution;
    }

    #endregion

    #region Material

    /// <summary>
    /// Use this when the player gets raw materials, like when rubble gets removed.
    /// </summary>
    /// <param name="rawMaterialToAdd"></param>
    public void AddRawMaterial(int rawMaterialToAdd)
    {
        rawMaterial += rawMaterialToAdd;
    }

    /// <summary>
    /// Use this when the player uses raw material, like when a factory converts it.
    /// </summary>
    /// <param name="rawMaterialToRemove"></param>
    public void RemoveRawMaterial(int rawMaterialToRemove)
    {
        rawMaterial -= rawMaterialToRemove;
    }

    /// <summary>
    /// Use this when the player gains building material, like when a factory is done converting materials.
    /// </summary>
    /// <param name="buildingMaterialToAdd"></param>
    public void AddBuildingMaterial(int buildingMaterialToAdd)
    {
        buildingMaterial += buildingMaterialToAdd;
    }

    /// <summary>
    /// Use this when the player uses building material, like when the player builds a building.
    /// </summary>
    /// <param name="buildingMaterialToRemove"></param>
    public void RemoveBuildingMaterial(int buildingMaterialToRemove)
    {
        buildingMaterial -= buildingMaterialToRemove;
    }

    /// <summary>
    /// Get the current amount of raw materials.
    /// </summary>
    /// <returns></returns>
    public int GetRawMaterials()
    {
        return rawMaterial;
    }

    /// <summary>
    /// Get the current amount of building materials.
    /// </summary>
    /// <returns></returns>
    public int GetBuildingMaterials()
    {
        return buildingMaterial;
    }

    #endregion

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

    #region UI
    
    public bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    #endregion
}
