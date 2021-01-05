using UnityEngine;
using UnityEngine.UI;

public class SelectStartingLocationUiManager : MonoBehaviour
{
    public static SelectStartingLocationUiManager Instance;

    public Text BuildOnFreeSpaceText;
    public float MaxRemoveTimer = 2f;
    private float removeTimer;

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

    /// <summary>
    /// Hide this ui when the game starts.
    /// </summary>
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(BuildOnFreeSpaceText.gameObject.activeSelf)
        {
            removeTimer += Time.deltaTime;

            if(removeTimer > MaxRemoveTimer)
            {
                removeTimer = 0f;

                HideBuildOnFreeSpaceMessage();
            }
        }
    }

    /// <summary>
    /// If the player selects a spot that is not free, show a help message.
    /// </summary>
    public void ShowBuildOnFreeSpaceMessage()
    {
        removeTimer = 0f;

        BuildOnFreeSpaceText.gameObject.SetActive(true);
    }

    public void HideBuildOnFreeSpaceMessage()
    {
        BuildOnFreeSpaceText.gameObject.SetActive(false);
    }
}
