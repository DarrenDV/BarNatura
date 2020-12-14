using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using static Assets.Scripts.Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    /// <summary>
    /// Start the game without going through the main menu.
    /// </summary>
    public bool DevMode = false;

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
    private int availableWorkers;
    private int workers;
    private int capacity;

    private AudioManager audioManager;

    [HideInInspector] public GameState CurrentGameState = GameState.MainMenu;

    // for testing purposes
    [HideInInspector] public int BuildingCount;

    [Header("Building")]
    public GameObject buildObjectPreview;
    [HideInInspector] public GameObject buildObject;
    [HideInInspector] public bool inBuildMode;

    [Header("Humans")]
    [SerializeField] private int humanOxygenUsage = 1;
    [SerializeField] private int maxHumansThatCanSpawn = 5;
    private float humanSpawnTimer;
    private float timeSinceLastHumanSpawn;
    [Tooltip("How long it take before the humans die when there isn't enough oxygen")]
    [SerializeField] private float humanDeathTimer = 10;
    private float timeLeftUntilHumansDie;
    [Tooltip("How many humams thar need to be alive during build mode")]
    [SerializeField] private int minHumansNeededToBuild = 5;

    private float analyticsTimer;

    private WinLose winLose;
    private AtmosphereSystem atmosphere;

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

        winLose = GetComponent<WinLose>();
        atmosphere = GetComponent<AtmosphereSystem>();
    }

    void Start()
    {
        ResetHumanTimer();
        audioManager = AudioManager.Instance;

        if (DevMode)
        {
            OnStartingLocationSelected(GetRandomFreeTile());
            CameraScript.Instance.GoToDefaultZoom();
            MainMenu.Instance.gameObject.SetActive(false);
            HudManager.Instance.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        oxygenSurplus = oxygenGeneration - (oxygenUsage + pollution);

        if (inBuildMode)
        { 
            if (Input.GetMouseButtonDown(1))
            {
                AudioManager.Instance.PlayStopBuildingSound();
                StopBuildingMode();
            }

            //Stop buidling if humans are dying while in build mode
            if (population < minHumansNeededToBuild)
            {
                StopBuildingMode();
            }
        }

        HandleAnalytics();

        //Start the human spawn timer when we have enough oxygen and living space to do so
        if (oxygenSurplus > 2 && capacity > population)
        {
            timeSinceLastHumanSpawn += Time.deltaTime;

            if (timeSinceLastHumanSpawn >= humanSpawnTimer)
            {
                int minHumanSpawn = 1;
                int maxHumanSpawn = maxHumansThatCanSpawn;

                //Spawn the max amount of humans possible
                if (capacity - population >= maxHumansThatCanSpawn && oxygenSurplus >= maxHumansThatCanSpawn)
                {
                    maxHumanSpawn = maxHumansThatCanSpawn;
                }
                //Checks if we have enough oxygen but not enough living space
                else if (capacity - population < maxHumansThatCanSpawn && oxygenSurplus >= maxHumansThatCanSpawn)
                {
                    maxHumanSpawn = capacity - population;
                }
                //Checks if we have enough living space but not enough oxygen
                else if (capacity - population >= maxHumansThatCanSpawn && oxygenSurplus < maxHumansThatCanSpawn)
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

        //Start the countdown timer when the player doesn't have enoug oxygen or housing
        if (oxygenSurplus <= 0 && population > 0 || population > capacity)
        {
            audioManager.PlayDangerLoopSound();

            timeLeftUntilHumansDie += Time.deltaTime;

            if (timeLeftUntilHumansDie > humanDeathTimer)
            {
                timeLeftUntilHumansDie = humanDeathTimer - Random.Range(1f, 2f);
                RemovePopulation(1);
                audioManager.PlayHumanDeathSound();
            }
        } 
        else
        {
            if (timeLeftUntilHumansDie != 0)
            {
                timeLeftUntilHumansDie = 0;
            }
            
            audioManager.StopDangerLoopSound();
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

        if (HudManager.Instance == null)
        {
            return;
        }

        HudManager.Instance.UpdateOxygenCounter();
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
        HudManager.Instance.UpdateOxygenCounter();
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

        if (HudManager.Instance == null)
        {
            return;
        }

        HudManager.Instance.UpdateDrainCounter();
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
        HudManager.Instance.UpdateDrainCounter();
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
            AddOxygenUsage(humanOxygenUsage);
        }
            
        population += populationToAdd;
        availableWorkers += populationToAdd;

        winLose.CheckPopulation(population);

        if (HudManager.Instance == null)
        {
            return;
        }

        HudManager.Instance.UpdateHumanCounter();
    }

    /// <summary>
    /// Use this when a human dies.
    /// </summary>
    /// <param name="populationToRemove">The amount of humans that died.</param>
    public void RemovePopulation(int populationToRemove)
    {
        for (int i = 0; i < populationToRemove; i++)
        {
            RemoveOxygenUsage(humanOxygenUsage);
        }

        population -= populationToRemove;
        availableWorkers -= populationToRemove;
        HudManager.Instance.UpdateHumanCounter();

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
    /// Use this when workers start working on something.
    /// </summary>
    /// <param name="workersToAdd">The amount of humans that died.</param>
    public void AddWorkers(int workersToAdd)
    {
        availableWorkers -= workersToAdd;
        workers += workersToAdd;
        HudManager.Instance.UpdateHumanCounter();
    }

    /// <summary>
    /// Use this when workers start have completed their task.
    /// </summary>
    /// <param name="workersToRemove">The amount of humans that died.</param>
    public void RemoveWorkers(int workersToRemove)
    {
        availableWorkers += workersToRemove;
        workers -= workersToRemove;
        HudManager.Instance.UpdateHumanCounter();
    }


    /// <summary>
    /// Get the current amount of workers.
    /// </summary>
    /// <returns></returns>
    public int GetWorkerAmount()
    {
        return workers;
    }

    /// <summary>
    /// Calculate if we have enough workers
    /// </summary>
    /// <returns></returns>
    /// /// <param name="requestedWorkers">The amount of humans that we need.</param>
    public bool AreWorkersAvailable(int requestedWorkers)
    {
        return !(availableWorkers - requestedWorkers <= -1);
    }

    /// <summary>
    /// Use this when a new human is born
    /// </summary>
    /// <param name="capacityToAdd">The amount of humans born.</param>
    public void AddCapacity(int capacityToAdd)
    {
        capacity += capacityToAdd;

        if (HudManager.Instance == null)
        {
            return;
        }

        HudManager.Instance.UpdateCapacityCounter();
    }

    /// <summary>
    /// Use this when a human dies.
    /// </summary>
    /// <param name="capacityToRemove">The amount of humans that died.</param>
    public void RemoveCapacity(int capacityToRemove)
    {
        capacity -= capacityToRemove;
        HudManager.Instance.UpdateCapacityCounter();
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

        if (HudManager.Instance == null)
        {
            return;
        }

        HudManager.Instance.UpdatePollutionCounter();
    }

    /// <summary>
    /// Use this when pollution gets removed, like when a geyser or factory gets removed.
    /// </summary>
    /// <param name="pollutionToRemove"></param>
    public void RemovePollution(int pollutionToRemove)
    {
        pollution -= pollutionToRemove;
        HudManager.Instance.UpdatePollutionCounter();
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
        HudManager.Instance.UpdateRawMaterialCounter();
    }

    /// <summary>
    /// Use this when the player uses raw material, like when a factory converts it.
    /// </summary>
    /// <param name="rawMaterialToRemove"></param>
    public void RemoveRawMaterial(int rawMaterialToRemove)
    {
        rawMaterial -= rawMaterialToRemove;
        HudManager.Instance.UpdateRawMaterialCounter();
    }

    /// <summary>
    /// Use this when the player gains building material, like when a factory is done converting materials.
    /// </summary>
    /// <param name="buildingMaterialToAdd"></param>
    public void AddBuildingMaterial(int buildingMaterialToAdd)
    {
        buildingMaterial += buildingMaterialToAdd;
        HudManager.Instance.UpdateBuildMaterialCounter();
    }

    /// <summary>
    /// Use this when the player uses building material, like when the player builds a building.
    /// </summary>
    /// <param name="buildMaterialToRemove"></param>
    public void RemoveBuildingMaterial(int buildMaterialToRemove)
    {
        buildingMaterial -= buildMaterialToRemove;
        HudManager.Instance.UpdateBuildMaterialCounter();
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

    #region Game Flow

    public void GoToLocationSelect()
    {
        CurrentGameState = GameState.SelectLocation;
    }

    public void OnStartingLocationSelected(BaseTileScript selectedTile)
    {
        CurrentGameState = GameState.InGame;
        Pointer.instance.unsetPointer();

        SelectStartingLocationUiManager.Instance.gameObject.SetActive(false);
        HudManager.Instance.gameObject.SetActive(true);

        winLose.enabled = true;
        atmosphere.enabled = true;

        selectedTile.PlaceStartingSpaceShip();

        // should happen in the tutorial somewhere
        foreach (var tile in FindObjectsOfType<BaseTileScript>())
        {
            tile.SpawnRandomPolution();
        }
    }

    #endregion

    #region Utils

    private BaseTileScript GetRandomFreeTile()
    {
        var tiles = FindObjectsOfType<BaseTileScript>();

        return tiles.First(tile => !tile.isOccupied);
    }

    #endregion
}
