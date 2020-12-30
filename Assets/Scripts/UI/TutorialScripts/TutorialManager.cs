using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public int TutorialIndex;

    public List<TutorialSequence> Tutorials;

    bool firstTreeBuilt, firstHouseBuilt, firstFactoryBuilt, firstRubbleRemoved;

    public void Start()
    {
        TutorialPopupScript.Instance.ShowNextTutorial();
        TutorialPopupScript.Instance.Next();
    }

    public void OnSpaceShipBuilt()
    {
        TutorialPopupScript.Instance.Next();
    }

    public void OnFirstTreeBuilt()
    {
        if (!firstTreeBuilt)
        {
            TutorialPopupScript.Instance.Next();
            firstTreeBuilt = true;
        }
    }

    public void OnFirstHouseBuilt()
    {
        if (!firstHouseBuilt)
        {
            TutorialPopupScript.Instance.Next();
            firstHouseBuilt = true;
        }
    }

    public void OnFirstRubbleRemoved()
    {
        if (!firstRubbleRemoved)
        {
            TutorialPopupScript.Instance.Next();
            firstRubbleRemoved = true;
        }
    }
}