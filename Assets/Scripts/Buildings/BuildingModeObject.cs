using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModeObject : MonoBehaviour
{

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Color[] colors;

    public void ChangeMaterial(bool isOccupied)
    {
        if (isOccupied == false) meshRenderer.sharedMaterial.SetColor("_Color", colors[0]);
        else meshRenderer.sharedMaterial.SetColor("_Color", colors[1]);
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
