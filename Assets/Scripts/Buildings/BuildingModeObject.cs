using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModeObject : MonoBehaviour
{

    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material[] buildMaterials;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaterial(bool canBuild)
    {
        for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++) {
            if (canBuild == false) meshRenderer.sharedMaterials[i] = buildMaterials[0];
            else meshRenderer.sharedMaterials[i] = buildMaterials[1];
        }

    }
}
