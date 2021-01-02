using UnityEngine;

public class MineScript : BuildingScript
{
    [SerializeField] private int rawMaterialsOverTime = 1;
    [SerializeField] private float maxMineTimer = 18;

    private float mineTimer;
    
    protected override void Update()
    {
        base.Update();

        if (mineTimer > maxMineTimer && !IsBeingBuild && !IsBeingRemoved)
        {
            GameManager.Instance.AddRawMaterial(rawMaterialsOverTime);
            mineTimer = 0;
        }

        mineTimer += Time.deltaTime;
        isProducing = true;
    }

    #region Strings

    public override string GetName()
    {
        return "Mine";
    }

    protected override string GetBuildingFunction()
    {
        return $"Mines for {raw}";
    }

    public override string GetDescription()
    {
        return $"This mine slowly produces {raw}  over time.\n\n{ShowProgress()}";
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
        return $"{""}";
    }

    protected override string GetResourceGain()
    {
        return $"{raw}";
    }

    #endregion

}
