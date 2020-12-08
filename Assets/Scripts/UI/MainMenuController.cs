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

    // quit game or stop play mode in editor
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
