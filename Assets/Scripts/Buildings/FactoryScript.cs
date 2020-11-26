using UnityEngine;

public class FactoryScript : BuildingScript
{
    [Header("Factory Script")] [SerializeField]
    private float maxFactoryConvertTimer = 0;

    [SerializeField] private int rawMaterialConsumption = 2;
    [SerializeField] private int buildingMaterialProduction = 1;

    private float factoryConvertTimer;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        factoryConvertTimer += Time.deltaTime;

        if (factoryConvertTimer >= maxFactoryConvertTimer)
        {
            if (GameManager.Instance.GetRawMaterials() >= rawMaterialConsumption)
            {
                GameManager.Instance.RemoveRawMaterial(rawMaterialConsumption);
                GameManager.Instance.AddBuildingMaterial(buildingMaterialProduction);
            }

            factoryConvertTimer = 0;
        }
    }

    public override string GetName()
    {
        return "Factory";
    }

    public override string GetDescription()
    {
        return $"This factory converts {rawMaterialConsumption} raw materials to {buildingMaterialProduction} building materials every {maxFactoryConvertTimer} seconds.";
    }

    public override void Remove()
    {
        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();

        //base.Remove();
    }
}
