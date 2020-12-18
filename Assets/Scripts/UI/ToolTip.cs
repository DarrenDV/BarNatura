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

    private void Start()
    {
        Hide();
    }

    public void Show(BuildMenuButton buildMenuButton)
    {
        if(!CanOpen)
        {
            return;
        }

        transform.position = new Vector3(buildMenuButton.transform.position.x, transform.position.y, 0);

        TitleText.text = buildMenuButton.BuildMenuItem.buildItem.GetName();
        DescriptionText.text = buildMenuButton.BuildMenuItem.buildItem.GetBuildButtonInformation();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
