﻿using UnityEngine;
using UnityEngine.UI;

public class FactoryScript : BuildingScript
{
    #region Variables

    [Header("Factory Script")]
    [SerializeField] private ParticleSystem smokeTrail;
    [SerializeField] private float maxFactoryConvertTimer;

    [SerializeField] private int rawMaterialConsumption = 2;
    [SerializeField] private int buildingMaterialProduction = 1;

    [SerializeField] private int workersNeededForBoost;

    private float factoryConvertTimer;

    public bool boostOn = false;

    #endregion

    #region Strings

    public override string GetName()
    {
        return "Factory";
    }

    protected override string GetBuildingFunction()
    {
        return $"Converts {brick} to {raw}";
    }

    public override string GetDescription()
    {
        return $"This factory converts {rawMaterialConsumption} {raw} to {brick} building materials every {maxFactoryConvertTimer} seconds.\n\n{ShowProgress()}";
    }

    #endregion

    #region Default

    protected override void Start()
    {
        base.Start();

        if (!GameManager.Instance.tutorialEnded) FindObjectOfType<TutorialManager>().OnFirstFactoryBuilt();
    }

    protected override void Update()
    {
        base.Update();

        // we can't produce materials when we are being build or removed.
        if (IsBeingBuild || IsBeingRemoved)
        {
            return;
        }

        if (GameManager.Instance.GetRawMaterials() >= rawMaterialConsumption)
        {
            factoryConvertTimer += Time.deltaTime;
            isProducing = true;

            if (factoryConvertTimer >= maxFactoryConvertTimer)
            {
                GameManager.Instance.RemoveRawMaterial(rawMaterialConsumption);
                GameManager.Instance.AddBuildingMaterial(buildingMaterialProduction);

                factoryConvertTimer = 0;
            }
        }
        else
        {
            isProducing = false;
        }

        //turns of the factory boost if the oxygen surplus is negative
        if (GameManager.Instance.GetOxygenSurplus() < 0)
        {
            if(boostOn) EndBoost();
        }
    }

    #endregion

    #region Progress

    protected override float GetMaxTime()
    {
        return maxFactoryConvertTimer;
    }

    protected override float GetTimer()
    {
        return factoryConvertTimer;
    }

    protected override string GetResourceDrained()
    {
        return $"{raw}";
    }

    protected override string GetResourceGain()
    {
        return $"{brick}";
    }

    protected override string GetNoProducingString()
    {
        return "Not enough raw materials!";
    }

    public void ToggleBoost()
    {
        if (!boostOn)
        {
            if (GameManager.Instance.AreWorkersAvailable(workersNeededForBoost))
            {
                GameManager.Instance.AddWorkers(workersNeededForBoost);

                rawMaterialConsumption *= 2;
                buildingMaterialProduction *= 2;

                if(!GameManager.Instance.tutorialEnded) FindObjectOfType<TutorialManager>().FactoryBoosted(true);

                boostOn = true;

            }
        }
        else
        {
            EndBoost();
        }

    }

    private void EndBoost()
    {
        GameManager.Instance.RemoveWorkers(workersNeededForBoost);

        rawMaterialConsumption /= 2;
        buildingMaterialProduction /= 2;

        if (!GameManager.Instance.tutorialEnded) FindObjectOfType<TutorialManager>().FactoryBoosted(false);

        boostOn = false;
    }

    #endregion

    #region Building
    public override void OnFinishedBuilding()
    {
        base.OnFinishedBuilding();
        smokeTrail.Play();
    }

    #endregion
}
