using UnityEngine;

public class StartingSpaceShip : BuildingScript
{
    [Header("Spaceship")]
    [Tooltip("The amount of seconds the spaceship will supply oxygen.")]
    [SerializeField] private float oxygenSupplyTime = 60;

    private float lifeTime;
    private bool supplyOxygen = true;

    public override string GetName()
    {
        return "Spaceship";
    }

    public override string GetDescription()
    {
        if (supplyOxygen)
        {
            return $"The spaceship supplies {oxygenProduction} oxygen until its tanks are empty.\n\nOxygen tanks running out in: {Mathf.Floor(oxygenSupplyTime - lifeTime)} sec.";
        }
        else
        {
            return "The spaceship has run out of oxygen.";
        }
    }

    public override void Remove()
    {
        // can't be removed
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddPopulation(maxCapacity);
    }

    protected override void Update()
    {
        base.Update();

        if (supplyOxygen)
        {
            SupplyOxygen();
        }
    }

    private void SupplyOxygen()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime > oxygenSupplyTime)
        {
            GameManager.Instance.RemoveOxygenGeneration(oxygenProduction);

            supplyOxygen = false;
        }
    }
}
