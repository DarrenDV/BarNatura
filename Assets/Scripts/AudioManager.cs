using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audomsources")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioSource buildAudioSource, demolishAudioSource;
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip build, buildFalied;

    // Start is called before the first frame update
    void Start()
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

    public void PlayDemolishSound()
    {
        demolishAudioSource.Play();
    }


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

}
