using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    public BuildMenuItem BuildMenuItem = null;
    [SerializeField] private Image buttonImage = null;

    private void Start()
    {
        buttonImage.sprite = BuildMenuItem.sprite;
    }

    public void MouseEnter()
    {
        ToolTip.Instance.Show(this);
    }

    public void MouseExit()
    {
        ToolTip.Instance.Hide();
    }

    public void OnClick()
    {
        if (BuildMenuItem.buildItem.BuildCost <= GameManager.Instance.GetBuildingMaterials()
            && BuildMenuItem.buildItem.HumansRequiredToBuild <= GameManager.Instance.AvailableWorkers)
        {
            GameManager.Instance.ChangeBuildObject(BuildMenuItem.buildItem.gameObject, BuildMenuItem.buildingItemMesh, BuildMenuItem.numberOfMeshes, BuildMenuItem.meshScale);
            AudioManager.Instance.PlayUiSelectSound();
        }

        ToolTip.Instance.Hide();
    }
}
