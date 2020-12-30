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

        GameManager.Instance.BuildingCount--;
        GameManager.Instance.RemoveCapacity(maxCapacity);
    }

    /// <summary>
    /// Remove the building without getting resources back. Used when toxic destroys the building.
    /// </summary>
    public void Demolish()
    {
        parentTile.DeletePlacedObject(gameObject);
        OnFinishedRemovingEvent.Invoke();
    }
}
