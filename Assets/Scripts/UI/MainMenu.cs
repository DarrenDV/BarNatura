using Assets.Scripts;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("MainMenu instance already set!");
        }
    }

    private void Start()
    {
        CameraScript.Instance.OnTransitionFinished.AddListener(OnCameraTransitionFinished);
    }

    /// <summary>
    /// When the camere stops transitioning, go to the next phase.
    /// </summary>
    private void OnCameraTransitionFinished()
    {
        CameraScript.Instance.OnTransitionFinished.RemoveListener(OnCameraTransitionFinished);

        GameManager.Instance.GoToLocationSelect();
        SelectStartingLocationUiManager.Instance.gameObject.SetActive(true);
    }

    /// <summary>
    /// When the play button is clicked, turn the main menu off and zoom the cam to the planet
    /// </summary>
    public void OnPlayButtonClicked()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayUiSelectSound();
        CameraScript.Instance.TransitionFromMainMenuToInGamePos();
    }

    public void OnQuitButtonClicked()
    {
        Utils.QuitGame();
    }
}
