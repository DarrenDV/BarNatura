using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : BuildingScript
{
    [SerializeField] private int rawMaterialsOverTime = 1;
    [SerializeField] private float maxMineTimer = 18;

    private float mineTimer;
    
    // Update is called once per frame
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

    public override string GetDescription()
    {
        return $"This mine slowly produces raw materials over time.";
    }

    public override void OnFinishedRemoving()
    {
        base.OnFinishedRemoving();

        transform.parent.GetComponent<BaseTileScript>().DeletePlacedObjects();
    }

    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();
    }
}
