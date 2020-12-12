using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance;

    private Text oxygenCounter, drainCounter, pollutionCounter, surplusCounter;
    private Slider oxygenBar;
    private Text buildMaterialCounter, rawMaterialCounter;
    private Text humanCounter, workerCounter, capacityCounter;

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

        
    }

    private void Start()
    {
        gameObject.SetActive(false);
        UpdateBuildMaterialCounter();
        UpdateRawMaterialCounter();
    }

    public void UpdateOxygenCounter()
    {
        oxygenCounter.text = GameManager.Instance.GetOxygenGeneration().ToString();
        UpdateSurplusCounter();
    }

    public void UpdateDrainCounter()
    {
        drainCounter.text = GameManager.Instance.GetOxygenUsage().ToString();
        UpdateSurplusCounter();
    }

    public void UpdatePollutionCounter()
    {
        pollutionCounter.text = GameManager.Instance.GetPollution().ToString();
        UpdateSurplusCounter();
    }

    public void UpdateBuildMaterialCounter()
    {
        buildMaterialCounter.text = GameManager.Instance.GetBuildingMaterials().ToString();
    }

    public void UpdateRawMaterialCounter()
    {
        rawMaterialCounter.text = GameManager.Instance.GetRawMaterials().ToString();
    }

    public void UpdateHumanCounter()
    {
        humanCounter.text = (GameManager.Instance.GetPopulationAmount() - GameManager.Instance.GetWorkerAmount()).ToString();
    }

    public void UpdateWorkerCounter()
    {
        workerCounter.text = GameManager.Instance.GetWorkerAmount().ToString();
    }

    public void UpdateCapacityCounter()
    {
        capacityCounter.text = GameManager.Instance.GetCapacityAmount().ToString();
    }

    public void UpdateSurplusCounter()
    {
        if (GameManager.Instance.GetOxygenSurplus() > 0)
        {
            surplusCounter.color = Color.green;
            surplusCounter.text = "+" + GameManager.Instance.GetOxygenSurplus().ToString();
        }
        else
        {
            surplusCounter.color = Color.red;
            surplusCounter.text = GameManager.Instance.GetOxygenSurplus().ToString();
        }

        oxygenBar.maxValue = GameManager.Instance.GetOxygenGeneration();
        oxygenBar.value = GameManager.Instance.GetOxygenUsage() + GameManager.Instance.GetPollution();
    }
}
