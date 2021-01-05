using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenBar : MonoBehaviour
{
    public RectTransform infoBox;

    /// <summary>
    /// Expand the box to show the extra oxygen info
    /// </summary>
    public void MoveBox()
    {
        infoBox.transform.localPosition = new Vector2(2000, infoBox.transform.localPosition.y);
    }

    /// <summary>
    /// Collide the box to hide extra oxygen info
    /// </summary>
    public void MoveBoxBack()
    {
        infoBox.transform.localPosition = new Vector2(0, infoBox.transform.localPosition.y);
    }
}
