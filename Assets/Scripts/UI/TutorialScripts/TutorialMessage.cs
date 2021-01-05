using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialMessage
{
    //These are the individual messages within the tutorial. 
    //Each message consists of a title, message and possibly a unityEvent in case the messages comes with a function.

    public string Title;
    [TextArea(10, 20)]
    public string Message;

    public UnityEvent TutorialEvent = new UnityEvent();
}
