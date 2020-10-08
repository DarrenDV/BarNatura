using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    [Header("Default building variables")]
    [Tooltip("How much oxygen does this building produce?")]
    [SerializeField] protected float oxygenProduction = 1f;
    [Tooltip("How much oxygen does this building drain?")]
    [SerializeField] protected float oxygenUsage = 1f;

    [Tooltip("How much oxygen does this building polution?")]
    [SerializeField] protected float polution = 1f;

    [Tooltip("How many humans can be in this building at the same time?")]
    [SerializeField] protected int maxCapacity;
    protected int currentCapacity;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }
}
