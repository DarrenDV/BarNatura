using UnityEngine;
public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject target = null;

    [SerializeField] private float cameraSlowDown = 0.35f;
    [SerializeField] private float cameraSlowDownMinimumSpeed = 0f;
    [SerializeField] private float cameraSlowDownFactor = 0.99f;

    [SerializeField] private float dragSpeed = 4.2f;
    [SerializeField] private float zoomSensitivity = 10f;
    [SerializeField] private float minZoomFov = 25f;
    [SerializeField] private float maxZoomFov = 100f;
    [SerializeField] private float mainMenuRotationSpeed = 5f;

    private float currentCameraSlowDown;
    private float mouseX, mouseY;

    private void Start()
    {
        currentCameraSlowDown = cameraSlowDown;
    }

    void Update()
    {
        // don't allow turning the camera in main menu
        if (GameManager.Instance.CurrentGameState == Assets.Scripts.Enums.GameState.MainMenu)
        {
            mouseX = mainMenuRotationSpeed;

            Rotate();

            return;
        }

        // When right mouse is pressed the cameraSlowDown gets reset and the mouse gets tracked.
        if (Input.GetMouseButton(1))
        {
            currentCameraSlowDown = cameraSlowDown;
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        // This takes care of the sliding effect.
        if (!Input.GetMouseButton(1))
        {
            currentCameraSlowDown *= cameraSlowDownFactor;

            if (currentCameraSlowDown <= cameraSlowDownMinimumSpeed)
            {
                currentCameraSlowDown = 0;
            }
        }

        Rotate();
        Zoom();
    }

    // This moves the camera around at all times so that the camera can continue sliding.
    private void Rotate()
    {
        transform.RotateAround(target.transform.position, transform.up, mouseX * dragSpeed * currentCameraSlowDown);
        transform.RotateAround(target.transform.position, transform.right, mouseY * -dragSpeed * currentCameraSlowDown);
    }

    private void Zoom()
    {
        float fov = Camera.main.fieldOfView;

        fov += Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
        fov = Mathf.Clamp(fov, minZoomFov, maxZoomFov);

        Camera.main.fieldOfView = fov;
    }
}