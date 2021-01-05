using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{
    public static ToolTip Instance;

    public Text TitleText = null;
    public TextMeshProUGUI DescriptionText = null;

    [HideInInspector] public bool CanOpen = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("ToolTip instance already set!");
        }
    }

    /// <summary>
    /// Hide the tooltip when the game starts.
    /// </summary>
    private void Start()
    {
        Hide();
    }

    /// <summary>
    /// Show the tooltip for the selected build item.
    /// </summary>
    /// <param name="buildMenuButton"></param>
    public void Show(BuildMenuButton buildMenuButton)
    {
        if(!CanOpen)
        {
            return;
        }

        transform.position = new Vector3(buildMenuButton.transform.position.x, transform.position.y, 0);

        // update the contents of the tooltip with the data of the selected building
        TitleText.text = buildMenuButton.BuildMenuItem.buildItem.GetName();
        DescriptionText.text = buildMenuButton.BuildMenuItem.buildItem.GetBuildButtonInformation();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
