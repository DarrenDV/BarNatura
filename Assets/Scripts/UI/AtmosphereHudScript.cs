using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereHudScript : MonoBehaviour
{
    [SerializeField] private Vector3 extendedPosition;
    [SerializeField] private float extentionSpeed = 1;

    private bool isOpen = false;
    private Vector3 basePosition;
    private void Start()
    {
        basePosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //This animates the pop-up depending on the current desired location (extended or closed).
        if (isOpen)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, extendedPosition, Time.deltaTime * extentionSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, basePosition, Time.deltaTime * extentionSpeed);
        }
    }

    //Toggles the popup to be open or closed.
    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
