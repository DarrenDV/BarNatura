using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public int TutorialIndex;

    public List<TutorialSequence> Tutorials;

    public void Start()
    {
        TutorialPopupScript.Instance.ShowNextTutorial();
        TutorialPopupScript.Instance.Next();
    }
}