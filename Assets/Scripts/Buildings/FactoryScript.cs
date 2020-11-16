using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryScript : MonoBehaviour
{
    protected int factoryPollutionProduction = 20;
    protected int factoryOxygenUsage = 10;
    protected int rawResourceToMaterialsTimer = 0;
    void Start()
    {
        // This adds the base pollution exhaust and oxygen up keep for 1 factory.
        GameManager.Instance.AddPollution(factoryPollutionProduction);
        GameManager.Instance.AddOxygenUsage(factoryOxygenUsage);
    }

    // Update is called once per frame
    void Update()
    {
        rawResourceToMaterialsTimer++;
        if (rawResourceToMaterialsTimer >= 45 * Time.deltaTime) {
            if (GameManager.Instance.GetRawMaterials() >= 2) {
                GameManager.Instance.RemoveRawMaterial(2);
                GameManager.Instance.AddBuildingMaterial(1);
                rawResourceToMaterialsTimer = 0;
            }
        }
        else
        {
            rawResourceToMaterialsTimer = 0;
        }
    }
}
