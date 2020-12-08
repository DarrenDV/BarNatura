using Assets.Scripts;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
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

        CameraScript.Instance.TransitionFromMainMenuToInGamePos();
    }

    public void OnQuitButtonClicked()
    {
        Utils.QuitGame();
    }
}
