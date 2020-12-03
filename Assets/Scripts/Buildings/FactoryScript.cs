using UnityEngine;

public class FactoryScript : BuildingScript
{
    [Header("Factory Script")]
    public ParticleSystem particleSystem;
    [SerializeField] private float maxFactoryConvertTimer = 0;

    [SerializeField] private int rawMaterialConsumption = 2;
    [SerializeField] private int buildingMaterialProduction = 1;

    private float factoryConvertTimer;
    private bool isProducing;
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // we can't produce materials when we are being build or removed.
        if (IsBeingBuild || IsBeingRemoved)
        {
            return;
        }

        if (GameManager.Instance.GetRawMaterials() >= rawMaterialConsumption)
        {
            factoryConvertTimer += Time.deltaTime;
            isProducing = true;

            if (factoryConvertTimer >= maxFactoryConvertTimer)
            {
                GameManager.Instance.RemoveRawMaterial(rawMaterialConsumption);
                GameManager.Instance.AddBuildingMaterial(buildingMaterialProduction);

                factoryConvertTimer = 0;
            }
        }
        else
        {
            isProducing = false;
        }
    }

    public override string GetName()
    {
        return "Factory";
    }

    public override string GetDescription()
    {
        return $"This factory converts {rawMaterialConsumption} raw materials to {buildingMaterialProduction} building materials every {maxFactoryConvertTimer} seconds.\n\n{ShowProgress()}";
    }

    private string ShowProgress()
    {
        if (isProducing)
        {
            return GetProgressBar();
        }
        else
        {
            return "Not enough raw materials!";
        }
    }

    private string GetProgressBar()
    {
        var maxArrows = Mathf.RoundToInt(maxFactoryConvertTimer);
        var arrows = string.Empty;

        for (var i = 0; i < Mathf.RoundToInt(factoryConvertTimer); i++)
        {
            arrows += ">";
        }

        while (arrows.Length < maxArrows)
        {
            arrows += "_";
        }

        return $"[RM {arrows} BM]";
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();
    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();
        particleSystem.Play();
    }
}
