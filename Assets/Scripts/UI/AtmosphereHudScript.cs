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
        if (isOpen)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, extendedPosition, Time.deltaTime * extentionSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, basePosition, Time.deltaTime * extentionSpeed);
        }
    }
    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
