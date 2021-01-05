using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance;

    private Text oxygenCounter, drainCounter, pollutionCounter, surplusCounter;
    private Slider oxygenBar;
    private Text buildMaterialCounter, rawMaterialCounter;
    private Text humanCounter, capacityCounter;

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

        oxygenCounter = GameObject.Find("Oxygen Counter").GetComponent<Text>();
        drainCounter = GameObject.Find("Drain Counter").GetComponent<Text>();
        pollutionCounter = GameObject.Find("Pollution Counter").GetComponent<Text>();
        surplusCounter = GameObject.Find("Surplus Counter").GetComponent<Text>();

        buildMaterialCounter = GameObject.Find("BuildMaterialCounter").GetComponent<Text>();
        rawMaterialCounter = GameObject.Find("RawMaterialCounter").GetComponent<Text>();

        humanCounter = GameObject.Find("HumanCounter").GetComponent<Text>();
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
        humanCounter.text = GameManager.Instance.GetPopulationAmount() - GameManager.Instance.GetWorkerAmount() + " / " + GameManager.Instance.GetPopulationAmount();
    }

    public void UpdateCapacityCounter()
    {
        capacityCounter.text = GameManager.Instance.GetCapacityAmount().ToString();
    }

    public void CapacityDanger(bool inDanger = true)
    {
        if (inDanger) capacityCounter.color = Color.red;
        else capacityCounter.color = Color.white;
    }

    public void UpdateSurplusCounter()
    {
        if (GameManager.Instance.GetOxygenSurplus() > 0)
        {
            surplusCounter.color = Color.green;
            surplusCounter.text = "+" + GameManager.Instance.GetOxygenSurplus();
        }
        else
        {
            surplusCounter.color = Color.red;
            surplusCounter.text = GameManager.Instance.GetOxygenSurplus().ToString();
        }

        oxygenBar.maxValue = GameManager.Instance.GetOxygenGeneration();
        oxygenBar.value = GameManager.Instance.GetOxygenUsage() + GameManager.Instance.GetPollution();
    }

    /// <summary>
    /// Return a icon to use in the tooltip
    /// </summary>
    /// <param name="IconName">The name of the icon</param>
    /// <returns></returns>
    public static string GetIcon(string IconName)
    {
        switch (IconName)
        {
            case "House": return "<sprite=0 color= CE6A17>";
            case "Raw": return "<sprite=1 color= 63330B>";
            case "Toxic": return "<sprite=2 color= 5A1960>";
            case "Human": return "<sprite=3 color= 03BCC0>";
            case "Brick": return "<sprite=4 color= BE1351>";
            case "OxygenPlus": return "<sprite=5 color= 00A0FF>";
            case "OxygenMin": return "<sprite=5 color= ff0000>";
            case "Pollution": return "<sprite=6 color= ffff00>";
            case "Nature": return "<sprite=7 color= 00ff00>";
            case "None": return "";
            default: return "Icon not found!";
        }
    }
}
