using UnityEngine;

public class HouseScript : BuildingScript
{
    [Header("House Script")]
    [Tooltip("How many humans spawn when this house is done building")]
    [SerializeField] private int populationToAdd;

    protected override void Start()
    {
        base.Start();

        if (!GameManager.Instance.tutorialEnded) FindObjectOfType<TutorialManager>().OnFirstHouseBuilt();
    }

    public override string GetName()
    {
        return "House";
    }

    protected override string GetBuildingFunction()
    {
        return $"+ {maxCapacity} {house}, + {populationToAdd} {human}";
    }

    public override string GetDescription()
    {
        return $"This house has {maxCapacity} {house}";
    }

    #region Building
    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.AddPopulation(populationToAdd);
    }
    #endregion
}
