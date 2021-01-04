using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenBar : MonoBehaviour
{
    public RectTransform infoBox;

    public void MoveBox()
    {
        infoBox.transform.localPosition = new Vector2(2000, infoBox.transform.localPosition.y);
    }

    public void MoveBoxBack()
    {
        infoBox.transform.localPosition = new Vector2(0, infoBox.transform.localPosition.y);
    }
}
