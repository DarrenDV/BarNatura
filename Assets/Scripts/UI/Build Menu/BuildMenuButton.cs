using UnityEngine;

public class BuildMenuButton : MonoBehaviour
{
    public BuildMenuItem BuildMenuItem = null;

    /// <summary>
    /// Show the tooltip when the player hovers over a build button.
    /// </summary>
    public void MouseEnter()
    {
        ToolTip.Instance.Show(this);
    }

    /// <summary>
    /// Hide the tooltip when the player exists a build button.
    /// </summary>
    public void MouseExit()
    {
        ToolTip.Instance.Hide();
    }

    /// <summary>
    /// Used when the player wants to build a building and presses a build button.
    /// </summary>
    public void OnClick()
    {
        var gameManager = GameManager.Instance;

        if (BuildMenuItem.buildItem.BuildCost <= gameManager.GetBuildingMaterials() && gameManager.AreWorkersAvailable(BuildMenuItem.buildItem.HumansRequiredToBuild))
        {
            gameManager.ChangeBuildObject(BuildMenuItem.buildItem.gameObject, BuildMenuItem.buildingItemMesh, BuildMenuItem.numberOfMeshes, BuildMenuItem.meshScale);
            AudioManager.Instance.PlayUiSelectSound();
        }

        ToolTip.Instance.Hide();
    }
}
