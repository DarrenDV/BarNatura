using UnityEngine;

public class SelectStartingLocationUiManager : MonoBehaviour
{
    public static SelectStartingLocationUiManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("SelectStartingLocationUi Manager instance already set!");
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
