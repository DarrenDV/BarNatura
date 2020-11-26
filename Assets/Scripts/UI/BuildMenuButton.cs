using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    [SerializeField] private BuildMenuItem buildMenuItem = null;
    [SerializeField] private Image buttonImage = null;

    private void Start()
    {
        buttonImage.sprite = buildMenuItem.sprite;
    }

    public void OnClick()
    {
        if (buildMenuItem.buildItem.buildCost <= GameManager.Instance.GetBuildingMaterials())
        {
            GameManager.Instance.ChangeBuildObject(buildMenuItem.buildItem.gameObject, buildMenuItem.buildingItemMesh, buildMenuItem.numberOfMeshes, buildMenuItem.meshScale, buildMenuItem.offset);
        }
    }
}
