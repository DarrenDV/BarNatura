using UnityEngine;

public class HouseScript : BuildingScript
{
    [Header("House Script")]
    [Tooltip("How many humans spawn when this house is done building")]
    [SerializeField] private int populationToAdd;
    [SerializeField] private ParticleSystem BuildingRemovingParticleEffect;
    public override string GetName()
    {
        return "House";
    }

    protected override string GetBuildingFunction()
    {
        return $"+ {maxCapacity} {HudManager.GetIcon("House")}, + {populationToAdd} {HudManager.GetIcon("Human")}";
    }

    public override string GetDescription()
    {
        return $"This house has {maxCapacity} {HudManager.GetIcon("House")}.";
    }

    #region Building
    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.AddPopulation(populationToAdd);
        BuildingRemovingParticleEffect.Stop();
    }
    public override void OnRemove()
    {
        base.OnRemove();
        BuildingRemovingParticleEffect.Play();
    }
    #endregion
}
