using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TutorialPopupScript : MonoBehaviour
{
    public static TutorialPopupScript Instance;
    private GameObject panel;
    private TutorialSequence currentTutorial;
    private TutorialManager tutorialManager;
    private int tutorialIndex = 0;
    [SerializeField] private Text titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Sound")]
    [SerializeField] private int volume;
    [SerializeField] private AudioClip appearSound, disappearSound;
    private AudioSource audioSource;

    void Awake()
    {
        //Makes an instance of this popupscript. 
        tutorialManager = GetComponent<TutorialManager>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Popup instance already set!");
        }

        audioSource = GetComponent<AudioSource>();
        panel = gameObject.transform.GetChild(0).gameObject;

        //Hide();
    }

    /// <summary>
    /// Go to the next message in the tutorial, if there are no more messages in the current mission, go to the next mission.
    /// </summary>
    /// <param name="playSound"></param>
    public void Next(bool playSound = true)
    {

        //Go to next tutorial if there are no more messages. 
        if(tutorialIndex == currentTutorial.Messages.Count)
        {
            ShowNextTutorial();
            tutorialIndex = 0;
        }

        //Update title and message 
        descriptionText.text = currentTutorial.Messages[tutorialIndex].Message;
        titleText.text = currentTutorial.Messages[tutorialIndex].Title;
        currentTutorial.Messages[tutorialIndex].TutorialEvent.Invoke(); //Run all the unity events associated with the message 
        tutorialIndex++;

        if (playSound)
        {
            PlaySound(disappearSound);
        }
    }

    private void Hide()
    {
        panel.SetActive(false);
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Go to the next tutorial mission. 
    /// </summary>
    public void ShowNextTutorial()
    {
        currentTutorial = tutorialManager.Tutorials[tutorialManager.TutorialIndex];
        tutorialManager.TutorialIndex++;
    }
}
