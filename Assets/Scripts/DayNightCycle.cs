using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
