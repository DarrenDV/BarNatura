using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public int TutorialIndex;

    private bool enoughTreesBuilt, firstHouseBuilt, firstFactoryBuilt, firstRubbleRemoved;

    private int currentTrees;
    [SerializeField] private int neededTrees = 2;

    private bool hasBoosted = false, hasUnboosted = false;

    public List<TutorialSequence> Tutorials;

    public void Start()
    {
        TutorialPopupScript.Instance.ShowNextTutorial();
        TutorialPopupScript.Instance.Next();
    }

    #region Checks

    public void OnSpaceShipBuilt()
    {
        TutorialPopupScript.Instance.Next();
    }

    public void OnTreeBuilt()
    {
        if (currentTrees + 1 == neededTrees) 
        {
            enoughTreesBuilt = true;
            TutorialPopupScript.Instance.Next();
        }

        if (!enoughTreesBuilt) currentTrees++;
    }

    public void OnFirstHouseBuilt()
    {
        if (!firstHouseBuilt)
        {
            TutorialPopupScript.Instance.Next();
            firstHouseBuilt = true;
        }
    }

    public void OnFirstFactoryBuilt()
    {
        if (!firstFactoryBuilt)
        {
            TutorialPopupScript.Instance.Next();
            firstFactoryBuilt = true;
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

    public void FactoryBoosted(bool boosting)
    {
        if (!hasBoosted && boosting)
        {
            TutorialPopupScript.Instance.Next();
            hasBoosted = true;
        }
        if(!hasUnboosted && !boosting)
        {
            TutorialPopupScript.Instance.Next();
            hasUnboosted = true;
        }
    }

    #endregion
}