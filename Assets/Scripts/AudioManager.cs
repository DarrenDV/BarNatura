using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource buildSound, popUpSound;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBuildSound()
    {
        buildSound.Play();
    }

    public void PlayPopUpSound()
    {
        popUpSound.Play();
    }
}
