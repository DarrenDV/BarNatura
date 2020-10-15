using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModeObject : MonoBehaviour
{

    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material[] materials;


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
        for (int i = 0; i < meshRenderer.materials.Length; i++) {
            if (canBuild) meshRenderer.materials[i] = materials[0];
            else meshRenderer.materials[i] = materials[1];
        }

    }
}
