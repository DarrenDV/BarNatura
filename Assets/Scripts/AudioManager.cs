using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Build sounds")]
    [SerializeField] private AudioSource buildAudioSource;
    
    [SerializeField] private AudioClip build, buildFalied, buildStopped;

    [Header("Danger sounds")]
    [SerializeField] private AudioSource dangerShort;
    [SerializeField] private AudioSource dangerLoop;
    [SerializeField] private AudioClip dangerLoopSound, humanDeath;

    [Header("Demolis sounds")]
    [SerializeField] private AudioSource demolishAudioSource;
    [SerializeField] private AudioClip demolishSound, disappearSound;

    [Header("UI sounds")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip selectSound;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Audio Manager instance already set!");
        }
    }


    #region building
    public void PlayBuildSound()
    {
        buildAudioSource.clip = build;
        buildAudioSource.Play();
    }

    public void PlayBuildFaliedSound()
    {
        buildAudioSource.clip = buildFalied;
        buildAudioSource.Play();
    }

    public void PlayStopBuildingSound()
    {
        buildAudioSource.clip = buildStopped;
        buildAudioSource.Play();
    }

    public void PlayDemolishSound()
    {
        demolishAudioSource.clip = demolishSound;
        demolishAudioSource.Play();
    }

    public void PlayDisappearSound()
    {
        demolishAudioSource.clip = disappearSound;
        demolishAudioSource.Play();
    }
    #endregion

    public void PlayDangerLoopSound()
    {
        if (!dangerLoop.isPlaying)
        {
            dangerLoop.clip = dangerLoopSound;
            dangerLoop.Play();
        }

    }

    public void StopDangerLoopSound()
    {
        if (dangerLoop.isActiveAndEnabled)
            dangerLoop.Stop();
    }

    public void PlayHumanDeathSound()
    {
        dangerShort.clip = humanDeath;
        dangerShort.Play();
    }

    public void PlayUiSelectSound() {
        uiAudioSource.clip = selectSound;
        uiAudioSource.Play();
    }

}
