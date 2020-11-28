using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : BuildObject
{
    [Header("Building Script")]
    [Tooltip("How many humans can be in this building at the same time?")]
    [SerializeField] protected int maxCapacity;

    private List<Human> occupants = new List<Human>();

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.BuildingCount++;
        GameManager.Instance.AddCapacity(maxCapacity);
    }

    public override void OnRemove()
    {
        base.OnRemove();

        // testing
        GameManager.Instance.BuildingCount--;
        GameManager.Instance.RemoveCapacity(maxCapacity);
    }
}
