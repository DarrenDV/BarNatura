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

    private void OnCameraTransitionFinished()
    {
        CameraScript.Instance.OnTransitionFinished.RemoveListener(OnCameraTransitionFinished);

        GameManager.Instance.GoToLocationSelect();
        SelectStartingLocationUiManager.Instance.gameObject.SetActive(true);
    }

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
