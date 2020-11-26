using UnityEngine;

public class BuildingModeObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private Color[] colors = null;
    [SerializeField] private Material transparentMaterial = null;

    public void ChangeMaterial(bool isOccupied)
    {
        }
    }

    public void NewMaterialsArray(int numberOfMeshes)
    {
        //Create a new array based on how many materials the object has
        meshRenderer.materials = new Material[numberOfMeshes];
        //Create a copy of the materials array, change it's materials and then past if back into the main array
        //No I can't just change the original array because Unity is a bitch
        var mats = meshRenderer.materials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = transparentMaterial;
        }
        meshRenderer.materials = mats;
    }
}
