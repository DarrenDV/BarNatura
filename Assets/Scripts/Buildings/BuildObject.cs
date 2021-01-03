using UnityEngine;
using UnityEngine.Events;

public class BuildObject : OxygenUser
{
    [Header("Building Object")]
    [Tooltip("Cost in building materials")]
    public int BuildCost = 1;
    public int HumansRequiredToBuild = 5;
    [Tooltip("The time it takes to build this object.")]
    [SerializeField] private float buildTime = 5f;

    [Tooltip("The minimum health a tile must have bafore this building can be placed on it")]
    public int MinimumNaturePollutedDegree = 1;

    [Tooltip("Divides the build cost by this int. How much materials are returned on demolish.")]
    [SerializeField] int buildCostReturnDivider = 2;

    [SerializeField] private ParticleSystem buildParticleEffect;

    public UnityEvent OnFinishedBuildingEvent;

    /// <summary>
    /// Is this object currently getting build.
    /// </summary>
    [HideInInspector] public bool IsBeingBuild;

    protected override void Update()
    {
        base.Update();

        if (IsBeingBuild)
        {
            canBeClikced = false;
            buildProgress += Time.deltaTime / buildTime;

            if (buildProgress >= 1)
            {
                buildProgress = 1f;
                IsBeingBuild = false;

                OnFinishedBuilding();
            }

            UpdateScale();
        }
    }

    #region Build tool tip info

    /// <summary>
    /// Get the description when the player wants to build this object.
    /// </summary>
    /// <returns></returns>
    public string GetBuildButtonInformation()
    {
        return $"{GetBuildingFunction()}\n" +
            $"{GetBuildRequirements()}\n" +
            $"{GetOxygenCosts()}";
    }

    /// <summary>
    /// Describe what the building does
    /// </summary>
    /// <returns></returns>

    protected virtual string GetBuildingFunction()
    {
        return "Allan please add details";
    }

    protected virtual string GetBuildRequirements()
    {
        return $"{HudManager.GetIcon("Human")} {HumansRequiredToBuild} {HudManager.GetIcon("Brick")} {BuildCost}";
    }

    protected virtual string GetOxygenCosts()
    {
        return $"{HudManager.GetIcon("OxygenMin")} {oxygenUsage} {HudManager.GetIcon("Pollution")} {pollutionProduction}";
    }

    #endregion

    #region Building

    public virtual void OnBuild()
    {
        GameManager.Instance.AddWorkers(HumansRequiredToBuild);
        buildParticleEffect.Play();
        buildProgress = 0f;
        IsBeingBuild = true;
    }

    /// <summary>
    /// Called when the object is finished being build.
    /// </summary>
    public virtual void OnFinishedBuilding()
    {
        GameManager.Instance.RemoveWorkers(HumansRequiredToBuild);
        OnFinishedBuildingEvent.Invoke();
        buildParticleEffect.Stop();
    }

    public override void OnRemove()
    {
        base.OnRemove();
        buildParticleEffect.Play();
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        //Gives resources back
        GameManager.Instance.AddBuildingMaterial(BuildCost / buildCostReturnDivider);
    }

    #endregion
}
