using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance;

    private Text oxygenCounter, drainCounter, pollutionCounter, surplusCounter;
    private Slider oxygenBar;
    private Text buildMaterialCounter, rawMaterialCounter;
    private Text humanCounter, workerCounter, capacityCounter;

    private GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Hud Manager instance already set!");
        }

        oxygenCounter = GameObject.Find("OxygenCounter").GetComponent<Text>();
        drainCounter = GameObject.Find("DrainCounter").GetComponent<Text>();
        pollutionCounter = GameObject.Find("PollutionCounter").GetComponent<Text>();
        surplusCounter = GameObject.Find("SurplusCounter").GetComponent<Text>();

        buildMaterialCounter = GameObject.Find("BuildMaterialCounter").GetComponent<Text>();
        rawMaterialCounter = GameObject.Find("RawMaterialCounter").GetComponent<Text>();

        humanCounter = GameObject.Find("HumanCounter").GetComponent<Text>();
        workerCounter = GameObject.Find("WorkerCounter").GetComponent<Text>();
        capacityCounter = GameObject.Find("CapacityCounter").GetComponent<Text>();
        oxygenBar = GameObject.Find("OxygenSlider").GetComponent<Slider>();

        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        UpdateBuildMaterialCounter();
        UpdateRawMaterialCounter();

        gameObject.SetActive(false);
    }

    public void UpdateOxygenCounter()
    {
        oxygenCounter.text = gameManager.GetOxygenGeneration().ToString();
        UpdateSurplusCounter();
    }

    public void UpdateDrainCounter()
    {
        drainCounter.text = gameManager.GetOxygenUsage().ToString();
        UpdateSurplusCounter();
    }

    public void UpdatePollutionCounter()
    {
        pollutionCounter.text = gameManager.GetPollution().ToString();
        UpdateSurplusCounter();
    }

    public void UpdateBuildMaterialCounter()
    {
        buildMaterialCounter.text = gameManager.GetBuildingMaterials().ToString();
    }

    public void UpdateRawMaterialCounter()
    {
        rawMaterialCounter.text = gameManager.GetRawMaterials().ToString();
    }

    public void UpdateHumanCounter()
    {
        humanCounter.text = (gameManager.GetPopulationAmount() - gameManager.GetWorkerAmount()).ToString();
    }

    public void UpdateWorkerCounter()
    {
        workerCounter.text = gameManager.GetWorkerAmount().ToString();
    }

    public void UpdateCapacityCounter()
    {
        capacityCounter.text = gameManager.GetCapacityAmount().ToString();
    }

    public void UpdateSurplusCounter()
    {
        if (gameManager.GetOxygenSurplus() > 0)
        {
            surplusCounter.color = Color.green;
            surplusCounter.text = "+" + gameManager.GetOxygenSurplus().ToString();
        }
        else
        {
            surplusCounter.color = Color.red;
            surplusCounter.text = gameManager.GetOxygenSurplus().ToString();
        }

        oxygenBar.maxValue = gameManager.GetOxygenGeneration();
        oxygenBar.value = gameManager.GetOxygenUsage() + gameManager.GetPollution();
    }
}
