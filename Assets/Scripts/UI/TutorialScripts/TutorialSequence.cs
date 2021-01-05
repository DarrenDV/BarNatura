using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialSequence
{
    //This makes a list of the messages.
    //A sequence is a list of messages until a "Mission" is complete
    public string Id;
    public List<TutorialMessage> Messages;
}
