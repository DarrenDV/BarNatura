using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    /// <summary>
    /// This updates the rotation of the day night cycle
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
