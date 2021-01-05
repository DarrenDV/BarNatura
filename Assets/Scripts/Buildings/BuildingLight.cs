using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLight : MonoBehaviour
{
    GameObject sun, moon;
    Light light;

    float checkTimer = 5f; //Standard 5 so the first check is instantly
    [SerializeField] float timeBetweenChecks = 5f;

    void Start()
    {
        sun = GameObject.Find("Sun");
        moon = GameObject.Find("Moon");
        light = gameObject.GetComponent<Light>();
    }

    private void Update()
    {
        //Simple timer to only run the check function every x seconds
        if (checkTimer <= timeBetweenChecks) checkTimer += Time.deltaTime;
        if (checkTimer >= timeBetweenChecks)
        {
            CheckLightPos();
            checkTimer = 0;
        }
    }

    /// <summary>
    /// Checks if the position of the moon is closer than the sun, thus turning on the light.
    /// </summary>
    void CheckLightPos()
    {
        if (Vector3.Distance(moon.transform.position, transform.position) <= Vector3.Distance(sun.transform.position, transform.position))
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
    }
}
