using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialMessage
{
    public string Title;
    [TextArea]
    public string Message;

    public UnityEvent TutorialEvent = new UnityEvent();
}
