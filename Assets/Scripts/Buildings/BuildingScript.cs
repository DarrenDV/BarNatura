using UnityEngine;

public class BuildingScript : BuildObject
{
    [Header("Building Script")]
    [Tooltip("How many humans can be in this building at the same time?")]
    [SerializeField] protected int maxCapacity;

    protected bool isProducing;

    #region Construction
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

    #endregion

    #region Progress

    protected virtual string ShowProgress()
    {
        if (isProducing)
        {
            return GetProgressBar();
        }
        else
        {
            Popup.Instance.HideProgressBar();
            return GetNoProducingString();
        }
    }

    protected virtual string GetProgressBar()
    {
        Popup.Instance.SetProgressBar(GetMaxTime(), GetTimer(), GetResourceDrained(), GetResourceGain());
        return "";

        var maxArrows = Mathf.RoundToInt(GetMaxTime());
        var arrows = string.Empty;

        for (var i = 0; i < Mathf.RoundToInt(GetTimer()); i++)
        {
            arrows += ">";
        }

        while (arrows.Length < maxArrows)
        {
            arrows += "_";
        }

        return $"[{GetResourceDrained()} {arrows} {GetResourceGain()}]";
    }

    protected virtual string GetNoProducingString()
    {
        return "";
    }

    protected virtual float GetMaxTime()
    {
        return 10;
    }

    protected virtual float GetTimer()
    {
        return 1;
    }

    protected virtual string GetResourceDrained()
    {
        return $"{HudManager.GetIcon("OxygenPlus")}";
    }

    protected virtual string GetResourceGain()
    {
        return $"{HudManager.GetIcon("OxygenMin")}";
    }

    #endregion
}
