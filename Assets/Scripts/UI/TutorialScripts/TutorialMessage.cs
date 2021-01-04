using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialMessage
{
    public string Title;
    [TextArea(10, 20)]
    public string Message;

    public UnityEvent TutorialEvent = new UnityEvent();
}
