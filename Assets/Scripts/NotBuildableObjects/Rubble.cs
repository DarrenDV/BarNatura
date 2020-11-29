using UnityEngine;

public class Rubble : BaseObject
{
    [Header("Rubble")]
    [SerializeField] private int rawMaterialMin = 5;
    [SerializeField] private int rawMaterialMax = 20;

    public override string GetName()
    {
        return "Rubble";
    }

    public override string GetDescription()
    {
        return $"Remove this rubble to gain {rawMaterialMin} to {rawMaterialMax} raw material.";
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        GameManager.Instance.AddRawMaterial(Random.Range(rawMaterialMin, rawMaterialMax));
        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();
    }
}
