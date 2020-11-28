using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public static ToolTip Instance;

    public Text TitleText = null;
    public Text DescriptionText = null;
    public Vector3 DisplayOffset = Vector3.zero;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Popup instance already set!");
        }

        Hide();
    }

    public void Show(BuildMenuButton buildMenuButton)
    {
        transform.position = buildMenuButton.transform.position + DisplayOffset;

        TitleText.text = buildMenuButton.BuildMenuItem.buildItem.GetName();
        DescriptionText.text = buildMenuButton.BuildMenuItem.buildItem.GetBuildDescription();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
