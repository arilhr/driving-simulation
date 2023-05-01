using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutomaticGenerateRoad))]
public class AutomaticGenerateRoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AutomaticGenerateRoad automaticGenerateRoad = (AutomaticGenerateRoad)target;

        if (GUILayout.Button("Generate Road"))
        {
            automaticGenerateRoad.GenerateRoad();
        }

        if (GUILayout.Button("Clear Road"))
        {
            automaticGenerateRoad.ResetData();
        }
    }
}
