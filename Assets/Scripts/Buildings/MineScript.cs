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
    }

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
        return $"This mine slowly produces {HudManager.GetIcon("Raw")}  over time.";
    }

}
