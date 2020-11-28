using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    /// <summary>
    /// Generic Utilities
    /// </summary>
    static class Utils
    {
        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        /// <summary>
        /// Check of the cursor is over an ui element.
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}
