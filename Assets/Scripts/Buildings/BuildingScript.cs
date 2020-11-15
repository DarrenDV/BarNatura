using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : BuildObject
{

    [Tooltip("How many humans can be in this building at the same time?")]
    [SerializeField] protected int maxCapacity;

    private List<Human> occupants = new List<Human>();

    //protected virtual void Start()
    //{
        
    //}

    //protected virtual void Update()
    //{
        
    //}
}
