using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [HideInInspector]public GameObject objectToBuild;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Building 
        
    public void StopBuilding()
    {
        objectToBuild = null;
    }

    #endregion
}
