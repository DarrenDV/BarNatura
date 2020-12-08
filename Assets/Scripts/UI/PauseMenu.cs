using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    private GameObject panel;

    [Header("Sound")]
    [SerializeField] private float volume = 1f;
    [SerializeField] private AudioClip appearSound, disappearSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("PauseMenu instance already set!");
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;

        panel = gameObject.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        Hide(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(panel.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void Show()
    {
        GameManager.Instance.StopBuildingMode();
        Time.timeScale = 0f;
        BuildMenu.Instance.DisableButtons();

        ToolTip.Instance.Hide();
        ToolTip.Instance.CanOpen = false;

        Popup.Instance.Hide(false);
        Popup.Instance.CanOpen = false;

        PlaySound(appearSound);

        panel.SetActive(true);
    }

    public void Hide(bool playSound = true)
    {
        Time.timeScale = 1f;
        BuildMenu.Instance.EnableButtons();

        ToolTip.Instance.CanOpen = true;
        Popup.Instance.CanOpen = true;

        if (playSound)
        {
            PlaySound(disappearSound);
        }

        panel.SetActive(false);
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void OnResumeButtonClicked()
    {
        Hide();
    }

    public void OnRestartButtonClicked()
    {
        Hide(false);

        SceneManager.LoadScene("Main");
    }

    public void OnQuitButtonClicked()
    {
        Utils.QuitGame();
    }
}
