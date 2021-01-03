using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Danger")]
    [SerializeField] private AudioSource dangerShort;
    [SerializeField] private AudioSource dangerLoop;
    [SerializeField] private AudioClip dangerLoopSound, humanDeath;

    [Header("UI")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip selectSound;

    [Header("Temp")]
    [SerializeField] private AudioSource buildAudioSource, demolishAudioSource;
    
    [SerializeField] private AudioClip build, buildFalied, buildStopped;

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
