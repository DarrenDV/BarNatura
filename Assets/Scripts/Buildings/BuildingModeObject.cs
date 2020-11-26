using UnityEngine;

public class BuildingModeObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private Color[] colors = null;

    public void ChangeMaterial(bool isOccupied)
    {
        if (!isOccupied)
        {
            meshRenderer.sharedMaterial.SetColor("_Color", colors[0]);
        }
        else
        {
            meshRenderer.sharedMaterial.SetColor("_Color", colors[1]);
        }
    }
}
