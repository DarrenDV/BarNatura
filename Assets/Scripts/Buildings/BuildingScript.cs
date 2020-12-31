using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : BuildObject
{
    [Header("Building Script")]
    [Tooltip("How many humans can be in this building at the same time?")]
    [SerializeField] protected int maxCapacity;

    protected bool isProducing;

    protected string house, raw, brick, toxic, human,
        oxygenPlus, oxygenMin, pollution, nature;

    protected override void Start()
    {
        base.Start();

        house = HudManager.GetIcon("House");
        raw = HudManager.GetIcon("Raw");
        brick = HudManager.GetIcon("Toxic");
        toxic = HudManager.GetIcon("Human");
        human = HudManager.GetIcon("Brick");
        oxygenPlus = HudManager.GetIcon("OxygenPlus");
        oxygenMin = HudManager.GetIcon("OxygenMin");
        pollution = HudManager.GetIcon("Pollution");
        nature = HudManager.GetIcon("Nature");
    }


    #region Construction

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
            return GetNoProducingString();
        }
    }

    protected virtual string GetProgressBar()
    {
        var maxArrows = Mathf.RoundToInt(GetMaxArrows());
        var arrows = string.Empty;

        for (var i = 0; i < Mathf.RoundToInt(GetTimer()); i++)
        {
            arrows += ">";
        }

        while (arrows.Length < maxArrows)
        {
            arrows += "_";
        }

        return $"[{GetResousreDrained()} {arrows} {GetResousreGain()}]";
    }

    protected virtual float GetMaxArrows()
    {
        return 10;
    }

    protected virtual float GetTimer()
    {
        return 1;
    }


    protected virtual string GetNoProducingString()
    {
        return "";
    }

    protected virtual string GetResousreDrained()
    {
        return $"{oxygenPlus}";
    }

    protected virtual string GetResousreGain()
    {
        return $"{oxygenMin}";
    }

    #endregion
}
