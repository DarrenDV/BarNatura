using UnityEngine;

public class Rubble : BaseObject
{
    public int RawMaterialMin;
    public int RawMaterialMax;

    public override string GetName()
    {
        return "Rubble";
    }

    public override string GetDescription()
    {
        return $"Remove this rubble to gain {RawMaterialMin} to {RawMaterialMax} raw material.";
    }

    public override void Remove()
    {
        GameManager.Instance.AddRawMaterial(Random.Range(RawMaterialMin, RawMaterialMax));

        base.Remove();
    }
}
