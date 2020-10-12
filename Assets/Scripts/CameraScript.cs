using UnityEngine;
public class CameraScript : MonoBehaviour
{
    public GameObject target;

    float cameraSlowDown = 0.35f;
    float cameraSlowDownMinimumSpeed = 0.03f;
    float cameraSlowDownFactor = 0.99f;


    float speed = 4.2f;
    float sensitivity = 5f;
    float minFov = 35f;
    float maxFov = 100f;

    float mouseX, mouseY;

    void Update()
    {
        // When mouse is pressed the cameraSlowDown gets reset and the mouse gets tracked.
        if (Input.GetMouseButton(0))
        {
            cameraSlowDown = 0.35f;
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        // This takes care of the sliding effect.
        if (!Input.GetMouseButton(0))
        {
            cameraSlowDown *= cameraSlowDownFactor;

            if (cameraSlowDown <= cameraSlowDownMinimumSpeed)
            {
                cameraSlowDown = 0;
            }
        }
        // This moves the camera around at all times so that the camera can continue sliding.
        transform.RotateAround(target.transform.position, transform.up, mouseX * speed * cameraSlowDown);
        transform.RotateAround(target.transform.position, transform.right, mouseY * -speed * cameraSlowDown);

        // Zoom function
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}