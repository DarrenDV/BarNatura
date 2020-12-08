﻿using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    public BuildMenuItem BuildMenuItem = null;

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
            && GameManager.Instance.AreWorkersAvailable(BuildMenuItem.buildItem.HumansRequiredToBuild))
        {
            GameManager.Instance.ChangeBuildObject(BuildMenuItem.buildItem.gameObject, BuildMenuItem.buildingItemMesh, BuildMenuItem.numberOfMeshes, BuildMenuItem.meshScale);
            AudioManager.Instance.PlayUiSelectSound();
        }

        ToolTip.Instance.Hide();
    }
}
