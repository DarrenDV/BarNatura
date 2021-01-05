using UnityEngine;

public class MineScript : BuildingScript
{
    [SerializeField] private int rawMaterialsOverTime = 1;
    [SerializeField] private float maxMineTimer = 18;

    private float mineTimer;
    
    protected override void Update()
    {
        base.Update();

        isProducing = true;

        // Checks if the timer has passed and if so adds more raw mats to the stockpile.
        if (mineTimer > maxMineTimer && !IsBeingBuild && !IsBeingRemoved)
        {
            GameManager.Instance.AddRawMaterial(rawMaterialsOverTime);
            mineTimer = 0;
        }

        mineTimer += Time.deltaTime;
    }

    #region Strings

    public override string GetName()
    {
        return "Mine";
    }

    protected override string GetBuildingFunction()
    {
        return $"Mines for {HudManager.GetIcon("Raw")}";
    }

    public override string GetDescription()
    {
        return $"This {GetName()} slowly produces {HudManager.GetIcon("Raw")}  over time.\n\n{ShowProgress()}";
    }

    #endregion

    #region Production

    protected override float GetMaxTime()
    {
        return maxMineTimer;
    }

    protected override float GetTimer()
    {
        return mineTimer;
    }

    protected override string GetResourceDrained()
    {
        return $"{HudManager.GetIcon("None")}";
    }

    protected override string GetResourceGain()
    {
        return $"{HudManager.GetIcon("Raw")}";
    }

    #endregion

}
