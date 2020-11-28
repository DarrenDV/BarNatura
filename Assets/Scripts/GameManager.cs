using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Oxygen
    private int oxygenGeneration;
    private int oxygenUsage;
    private int pollution;
    private int oxygenSurplus;

    //Materials
    private int rawMaterial;
    private int buildingMaterial = 100;

    //Population
    private int population;
    private int capacity;

    // for testing purposes
    [HideInInspector] public int BuildingCount;

    [Header("Building")]
    public GameObject buildObjectPreview;
    [HideInInspector] public GameObject buildObject;
    [HideInInspector] public bool inBuildMode;

    private Vector3 offset;

    [Header("Humans")]
    [SerializeField] private int humanOxygenUseage;
    private float humanSpawnTimer;
    private float timeSinceLastHumanSpawn;
    [Tooltip("How long it take before the humans die when there isn't enough oxygen")]
    [SerializeField] private float humanDeathTimer;
    private float timeLeftUntilHumansDie;

    [Header("UI")]
    private Text oxygenCounter, drainCounter, pollutionCounter, surplusCounter;
    private Text buildMaterialCounter, rawMaterialCounter;
    private Text humanCounter, capacityCounter;

    private float analyticsTimer;

    private WinLose winLose;

    #region default

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

        oxygenCounter = GameObject.Find("OxygenCounter").GetComponent<Text>();
        drainCounter = GameObject.Find("DrainCounter").GetComponent<Text>();
        pollutionCounter = GameObject.Find("PollutionCounter").GetComponent<Text>();
        surplusCounter = GameObject.Find("SurplusCounter").GetComponent<Text>();

        buildMaterialCounter = GameObject.Find("BuildMaterialCounter").GetComponent<Text>();
        rawMaterialCounter = GameObject.Find("RawMaterialCounter").GetComponent<Text>();

        humanCounter = GameObject.Find("HumanCounter").GetComponent<Text>();
        capacityCounter = GameObject.Find("CapacityCounter").GetComponent<Text>();
        //foodCounter = GameObject.Find("FoodCounter").GetComponent<Text>();

        winLose = GetComponent<WinLose>();
    }

    void Start()
    {
        ChangeBuildMaterialCounter();
        ChangeRawMaterialCounter();
        ResetHumanTimer();   
    }

    void Update()
    {

        oxygenSurplus = oxygenGeneration - (oxygenUsage + pollution);

        if (Input.GetMouseButtonDown(1))
        {
            StopBuildingMode();
        }

        HandleAnalytics();

        //Start the human spawn timer when we have enough oxygen and living space to do so
        if (oxygenSurplus > 2 && capacity > population)
        {
            timeSinceLastHumanSpawn += Time.deltaTime;
            if (timeSinceLastHumanSpawn >= humanSpawnTimer)
            {
                int minHumanSpawn = 1;
                int possibleMax = 5;
                int maxHumanSpawn = possibleMax;

                //Spawn the max amount of humans possible
                if (capacity - population >= possibleMax && oxygenSurplus >= possibleMax)
                {
                    maxHumanSpawn = possibleMax;
                }
                //Checks if we have enough oxygen but not enough living space
                else if (capacity - population < possibleMax && oxygenSurplus >= possibleMax)
                {
                    maxHumanSpawn = capacity - population;
                }
                //Checks if we have enough living space but not enough oxygen
                else if (capacity - population >= possibleMax && oxygenSurplus < possibleMax)
                {
                    maxHumanSpawn = oxygenSurplus - 1;
                }

                AddPopulation(Random.Range(minHumanSpawn, maxHumanSpawn + 1));
                ResetHumanTimer();
            }
        }
        else
        {
            if (timeSinceLastHumanSpawn != 0)
                ResetHumanTimer();
        }


        if (oxygenSurplus <= 0 && population > 0 || population > capacity)
        {
            timeLeftUntilHumansDie += Time.deltaTime;
            if (timeLeftUntilHumansDie > humanDeathTimer)
            {
                timeLeftUntilHumansDie = humanDeathTimer - Random.Range(1f, 2f);
                RemovePopulation(1);
            }
        } 
        else
        {
            if (timeLeftUntilHumansDie != 0)
                    timeLeftUntilHumansDie = 0;
        }

    }

    #endregion

    #region Analytics

    private void HandleAnalytics()
    {
        analyticsTimer += Time.deltaTime;

        // send analytics every minute
        if (analyticsTimer > 60)
        {
            analyticsTimer = 0;

            SendAnalytics();
        }
    }

    private void SendAnalytics()
    {
        Analytics.CustomEvent("timed_stats", new Dictionary<string, object>
        {
            {"time", Time.time},
            {"buildings", BuildingCount},
            {"population", population},
            {"raw_materials", rawMaterial},
            {"building_materials", buildingMaterial}
        });
    }

    #endregion

    #region Oxygen

    /// <summary>
    /// Use this when a new object has been placed that produces oxygen, like a new tree.
    /// </summary>
    /// <param name="oxygenGenerationToAdd">The amount of oxygen the object generates.</param>
    public void AddOxygenGeneration(int oxygenGenerationToAdd)
    {
        if (oxygenGenerationToAdd == 0)
        {
            return;
        }

        oxygenGeneration += oxygenGenerationToAdd;
        ChangeOxygenCounter();
    }

    /// <summary>
    /// Use this when an object that generates oxygen gets removed, when a tree dies for example.
    /// </summary>
    /// <param name="oxygenGenerationToRemove">The amount of oxygen generation to remove</param>
    public void RemoveOxygenGeneration(int oxygenGenerationToRemove)
    {
        if (oxygenGenerationToRemove == 0)
        {
            return;
        }

        oxygenGeneration -= oxygenGenerationToRemove;
        ChangeOxygenCounter();
    }

    /// <summary>
    /// Use this when a new object has been placed that uses oxygen, like a new human.
    /// </summary>
    /// <param name="oxygenUsageToAdd">The amount of oxygen the object uses.</param>
    public void AddOxygenUsage(int oxygenUsageToAdd)
    {
        if (oxygenUsageToAdd == 0)
        {
            return;
        }

        oxygenUsage += oxygenUsageToAdd;
        ChangeDrainCounter();
    }

    /// <summary>
    /// Use this when an object that uses oxygen gets removed, like when a human dies.
    /// </summary>
    /// <param name="oxygenUsageToRemove"></param>
    public void RemoveOxygenUsage(int oxygenUsageToRemove)
    {
        if (oxygenUsageToRemove == 0)
        {
            return;
        }

        oxygenUsage -= oxygenUsageToRemove;
        ChangeDrainCounter();
    }

    /// <summary>
    /// Get the surplus of oxygen
    /// </summary>
    public int GetOxygenSurplus()
    {
        return oxygenGeneration - (oxygenUsage + pollution);
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

    #region Population

    /// <summary>
    /// Use this when a new human is born
    /// </summary>
    /// <param name="populationToAdd">The amount of humans born.</param>
    public void AddPopulation(int populationToAdd)
    {
        //if (population + populationToAdd > capacity) populationToAdd = capacity - population;
        for (int i = 0; i < populationToAdd; i++)
        {
            AddOxygenUsage(humanOxygenUseage);
        }
            
        population += populationToAdd;
        ChangeHumanCounter();

        winLose.CheckPopulation(population);
    }

    /// <summary>
    /// Use this when a human dies.
    /// </summary>
    /// <param name="populationToRemove">The amount of humans that died.</param>
    public void RemovePopulation(int populationToRemove)
    {
        for (int i = 0; i < populationToRemove; i++)
        {
            RemoveOxygenUsage(humanOxygenUseage);
        }

        population -= populationToRemove;
        ChangeHumanCounter();

        winLose.CheckPopulation(population);
    }

    /// <summary>
    /// Get the current amount of humans.
    /// </summary>
    /// <returns></returns>
    public int GetPopulationAmount()
    {
        return population;
    }

    /// <summary>
    /// Use this when a new human is born
    /// </summary>
    /// <param name="capacityToAdd">The amount of humans born.</param>
    public void AddCapacity(int capacityToAdd)
    {
        capacity += capacityToAdd;
        ChangeCapacityCounter();
    }

    /// <summary>
    /// Use this when a human dies.
    /// </summary>
    /// <param name="capacityToRemove">The amount of humans that died.</param>
    public void RemoveCapacity(int capacityToRemove)
    {
        capacity -= capacityToRemove;
        ChangeCapacityCounter();
    }

    /// <summary>
    /// Get the current amount of idle humans.
    /// </summary>
    /// <returns></returns>
    public int GetCapacityAmount()
    {
        return capacity;
    }

    /// <summary>
    /// Resets the human Spawn Timer
    /// </summary>
    private void ResetHumanTimer()
    {
        float minHumanSpawnTimer = 10;
        float maxHumanSpawnTimer = 15;
        humanSpawnTimer = Random.Range(minHumanSpawnTimer, maxHumanSpawnTimer);

        timeSinceLastHumanSpawn = 0;
    }

    #endregion

    #region Pollution

    /// <summary>
    /// Use this when gets added to the world, like when a geyser gets spawned or when a factory gets build.
    /// </summary>
    /// <param name="pollutionToAdd"></param>
    public void AddPollution(int pollutionToAdd)
    {
        pollution += pollutionToAdd;
        ChangePollutionCounter();
    }

    /// <summary>
    /// Use this when pollution gets removed, like when a geyser or factory gets removed.
    /// </summary>
    /// <param name="pollutionToRemove"></param>
    public void RemovePollution(int pollutionToRemove)
    {
        pollution -= pollutionToRemove;
        ChangePollutionCounter();
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
        ChangeRawMaterialCounter();
    }

    /// <summary>
    /// Use this when the player uses raw material, like when a factory converts it.
    /// </summary>
    /// <param name="rawMaterialToRemove"></param>
    public void RemoveRawMaterial(int rawMaterialToRemove)
    {
        rawMaterial -= rawMaterialToRemove;
        ChangeRawMaterialCounter();
    }

    /// <summary>
    /// Use this when the player gains building material, like when a factory is done converting materials.
    /// </summary>
    /// <param name="buildingMaterialToAdd"></param>
    public void AddBuildingMaterial(int buildingMaterialToAdd)
    {
        buildingMaterial += buildingMaterialToAdd;
        ChangeBuildMaterialCounter();
    }

    /// <summary>
    /// Use this when the player uses building material, like when the player builds a building.
    /// </summary>
    /// <param name="buildMaterialToRemove"></param>
    public void RemoveBuildingMaterial(int buildMaterialToRemove)
    {
        buildingMaterial -= buildMaterialToRemove;
        ChangeBuildMaterialCounter();
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

    public void ChangeBuildObject(GameObject newObject, Mesh previewMesh, int numberOfMeshes, float size)
    {
        inBuildMode = true;
        buildObject = newObject;
        buildObjectPreview.GetComponent<MeshFilter>().sharedMesh = previewMesh;
        buildObjectPreview.GetComponent<BuildingModeObject>().NewMaterialsArray(numberOfMeshes);
        buildObjectPreview.transform.localScale = new Vector3(size, size, size);

        //Instantiate(buildObjectPreview.gameObject, transform.position, transform.rotation, previewObjectParent.transform);
    }

    public void MovePreview(Vector3 position, Quaternion rotation)
    {
        buildObjectPreview.transform.position = position ;
        buildObjectPreview.transform.rotation = rotation;
    }

    public void StopBuildingMode()
    {
        inBuildMode = false;
        buildObjectPreview.transform.position = Vector3.zero;
    }

    #endregion

    #region UI

    private void ChangeOxygenCounter()
    {
        oxygenCounter.text = GetOxygenGeneration().ToString();
        ChangeSurplusCounter();
    }

    private void ChangeDrainCounter()
    {
        drainCounter.text = GetOxygenUsage().ToString();
        ChangeSurplusCounter();
    }

    private void ChangePollutionCounter()
    {
        pollutionCounter.text = GetPollution().ToString();
        ChangeSurplusCounter();
    }

    private void ChangeBuildMaterialCounter()
    {
        buildMaterialCounter.text = GetBuildingMaterials().ToString();
    }

    private void ChangeRawMaterialCounter()
    {
        rawMaterialCounter.text = GetRawMaterials().ToString();
    }

    private void ChangeHumanCounter()
    {
        humanCounter.text = GetPopulationAmount().ToString();
    }

    private void ChangeCapacityCounter()
    {
        capacityCounter.text = GetCapacityAmount().ToString();
    }

    private void ChangeSurplusCounter()
    {
        surplusCounter.text = GetOxygenSurplus().ToString();
    }

    #endregion
}
