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
        return $"Remove this rubble to gain a randomly gain {rawMaterialMin} to {rawMaterialMax} {HudManager.GetIcon("Raw")}.";
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        GameManager.Instance.AddRawMaterial(Random.Range(rawMaterialMin, rawMaterialMax));

        FindObjectOfType<TutorialManager>().OnFirstRubbleRemoved();
    }
}
