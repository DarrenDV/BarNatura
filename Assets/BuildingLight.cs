using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLight : MonoBehaviour
{
    GameObject sun, moon;
    Light light;


    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun");
        moon = GameObject.Find("Moon");
        light = this.gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(moon.transform.position, this.transform.position) <= Vector3.Distance(sun.transform.position, this.transform.position))
        {
            light.color = Color.blue;
        }
        else if(Vector3.Distance(sun.transform.position, this.transform.position) <= Vector3.Distance(moon.transform.position, this.transform.position))
        {
            light.color = Color.red;
        }
        else
        {
            light.color = Color.white;
        }
    }
}
