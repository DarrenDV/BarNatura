using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource buildsounds;
    [SerializeField] private AudioClip buildSound;

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


    public void PlayBuildSound()
    {
        buildsounds.Play();
    }

}
