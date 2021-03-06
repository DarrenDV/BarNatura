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
        return "Dropship";
    }

    public override string GetDescription()
    {
        if (supplyOxygen)
        {
            return $"The dropship houses {maxCapacity} humans and supplies {oxygenProduction} oxygen until its tanks are empty.\n\nOxygen tanks running out in: {Mathf.Floor(oxygenSupplyTime - lifeTime)} sec.";
        }
        else
        {
            return "The dropship has run out of oxygen.";
        }
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddCapacity(maxCapacity);
        GameManager.Instance.AddPopulation(maxCapacity);

        FindObjectOfType<TutorialManager>().ProgressTutorial("SpaceShipBuilt");
    }

    protected override void Update()
    {
        base.Update();

        if (supplyOxygen)
        {
            SupplyOxygen();
        }

        // update fire particles so they stop when we land
        if (checkParticles)
        {
            if (particleTimer > particleEndTime)
            {
                particles.Stop();
                checkParticles = false;
            }

            particleTimer += Time.deltaTime;
        }
    }

    // keep track how long the oxygen supply will last
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
