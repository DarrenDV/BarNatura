using UnityEngine;

public class StartingSpaceShip : BuildingScript
{
    [Header("Spaceship")]
    [Tooltip("The amount of seconds the spaceship will supply oxygen.")]
    [SerializeField] private float oxygenSupplyTime = 60;
    [SerializeField] private ParticleSystem particles;
    private float particleTimer, particleEndTime = 3;
    private bool checkParticles = true;

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
            return $"The spaceship houses {maxCapacity} humans and supplies {oxygenProduction} oxygen until its tanks are empty.\n\nOxygen tanks running out in: {Mathf.Floor(oxygenSupplyTime - lifeTime)} sec.";
        }
        else
        {
            return "The spaceship has run out of oxygen.";
        }
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddCapacity(maxCapacity);
        GameManager.Instance.AddPopulation(maxCapacity);
    }

    protected override void Update()
    {
        base.Update();

        if (supplyOxygen)
        {
            SupplyOxygen();
        }

        if (checkParticles) {
            if (particleTimer > particleEndTime)
            {
                particles.Stop();
                checkParticles = false;
            }
            particleTimer += Time.deltaTime;
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
