using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLavaTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0, 100) == 0){
            placeObject(Instantiate(lavaTile, Vector3.zero, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
