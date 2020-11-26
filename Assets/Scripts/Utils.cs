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
    }
}
