using UnityEngine;

public class AtmosphereHudScript : MonoBehaviour
{
    [SerializeField] private RectTransform basePosition;
    [SerializeField] private RectTransform extendedPosition;
    [SerializeField] private float expandSpeed = 3;

    private bool isOpen;

    private void Start()
    {
        transform.position = basePosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        //This animates the pop-up depending on the current desired location (extended or closed).
        if (isOpen)
        {
            // expand
            transform.position = Vector3.Lerp(transform.position, extendedPosition.position, Time.deltaTime * expandSpeed);
        }
        else
        {
            // retract
            transform.position = Vector3.Lerp(transform.position, basePosition.position, Time.deltaTime * expandSpeed);
        }
    }

    //Toggles the popup to be open or closed.
    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
