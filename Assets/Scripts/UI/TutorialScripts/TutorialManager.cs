using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialSequence> Tutorials;

    public void Start()
    {
        TutorialPopupScript.Instance.ShowTutorial("Mission 1: dont die");
    }
}