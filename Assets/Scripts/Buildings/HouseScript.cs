using UnityEngine;

public class HouseScript : BuildingScript
{
    [Header("House Script")]
    [Tooltip("How many humans spawn when this house is done building")]
    [SerializeField] private int populationToAdd;

    protected override void Start()
    {
        base.Start();

        // if the tutorial hasnt ended then check for the first house that was build.
        FindObjectOfType<TutorialManager>().ProgressTutorial("HouseBuilt");
    }

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
        return $"This {GetName()} has {maxCapacity} {HudManager.GetIcon("House")}";
    }

    #region Building
    /// <summary>
    /// This adds humans after finnishing building the house
    /// </summary>
    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.AddPopulation(populationToAdd);
    }
    #endregion
}
