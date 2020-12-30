#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTileScript))]
public class BaseTileScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tile = (BaseTileScript) target;

        if(GUILayout.Button("Make Max Nature (10)"))
        {
            tile.SetNaturePollutedDegree(10);
        }

        if (GUILayout.Button("Make Max Toxic (-10)"))
        {
            tile.SetNaturePollutedDegree(-10);
        }
    }
}

#endif
