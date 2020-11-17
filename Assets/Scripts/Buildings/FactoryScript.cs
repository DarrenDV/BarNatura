using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryScript : BuildingScript
{
    [Header("Factory Script")]
    [SerializeField] private float maxFactoryConvertTimer;
    [SerializeField]private float factoryConvertTimer;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        factoryConvertTimer += Time.deltaTime;

        if (factoryConvertTimer >= maxFactoryConvertTimer) {
            if (GameManager.Instance.GetRawMaterials() >= 2) {
                GameManager.Instance.RemoveRawMaterial(2);
                GameManager.Instance.AddBuildingMaterial(1);  
            }
            factoryConvertTimer = 0;
        }
    }
}
