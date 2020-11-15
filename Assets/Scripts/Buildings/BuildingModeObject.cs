using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModeObject : MonoBehaviour
{

    //[SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color[] colors;

    public void ChangeMaterial(bool isOccupied)
    {

        //for (int i = 0; i < meshRenderers.Length; ++i)
        //{
        //    if (isOccupied == false) meshRenderers[i].sharedMaterial.SetColor("_Color", colors[0]);
        //    else meshRenderers[i].sharedMaterial.SetColor("_Color", colors[1]);
        //}

        if (isOccupied == false) meshRenderer.sharedMaterial.SetColor("_Color", colors[0]);
        else meshRenderer.sharedMaterial.SetColor("_Color", colors[1]);

        //for (int i = 0; i < meshRenderer.materials.Length; ++i)
        //{
        //    if (isOccupied == false) meshRenderer.sharedMaterial[0].SetColor("_Color", colors[0]);
        //    else meshRenderer.sharedMaterial[i].SetColor("_Color", colors[1]);
        //}


    }
}
