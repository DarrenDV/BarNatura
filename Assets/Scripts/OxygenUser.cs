using UnityEngine;

public class OxygenUser : BaseObject
{
    [Header("Oxygen User")]
    [Tooltip("How much oxygen does this object produce?")]
    [SerializeField] protected int oxygenProduction;
    [Tooltip("How much oxygen does this object drain?")]
    [SerializeField] protected int oxygenUsage;

    [Tooltip("How much oxygen does this object polution?")]
    [SerializeField] protected int pollutionProduction;

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddOxygenGeneration(oxygenProduction);
        GameManager.Instance.AddOxygenUsage(oxygenUsage);
        GameManager.Instance.AddPollution(pollutionProduction);
    }

    public override void OnRemove(bool instant = false)
    {
        base.OnRemove(instant);

        GameManager.Instance.RemoveOxygenGeneration(oxygenProduction);
        GameManager.Instance.RemoveOxygenUsage(oxygenUsage);
        GameManager.Instance.RemovePollution(pollutionProduction);
    }
}
