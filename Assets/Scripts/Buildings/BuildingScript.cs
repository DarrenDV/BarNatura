using UnityEngine;

public class BuildingScript : BuildObject
{
    [Header("Building Script")]
    [Tooltip("How many humans can be in this building at the same time?")]
    [SerializeField] protected int maxCapacity;

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();

        GameManager.Instance.BuildingCount++;
        GameManager.Instance.AddCapacity(maxCapacity);
    }

    public override void OnRemove(bool instant = false)
    {
        base.OnRemove(instant);

        GameManager.Instance.BuildingCount--;
        GameManager.Instance.RemoveCapacity(maxCapacity);
    }

    /// <summary>
    /// Remove the building without getting resources back. Used when toxic destroys the building.
    /// </summary>
    public void Demolish()
    {
        OnRemove(true);

        parentTile.DeletePlacedObject(gameObject);
        OnFinishedRemovingEvent.Invoke();
    }
}
