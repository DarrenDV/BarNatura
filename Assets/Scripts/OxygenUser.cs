using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenUser : BaseObject
{

    [Header("Oxygen varibles")]
    [Tooltip("How much oxygen does this object produce?")]
    [SerializeField] protected float oxygenProduction = 1f;
    [Tooltip("How much oxygen does this object drain?")]
    [SerializeField] protected float oxygenUsage = 1f;

    [Tooltip("How much oxygen does this object polution?")]
    [SerializeField] protected float pollutionProduction = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
