using UnityEditor;
using UnityEngine;

namespace DrivingSimulation
{
    [CustomEditor(typeof(RepeatedObject))]
    public class RepeatedObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RepeatedObject repeatedObject = (RepeatedObject)target;
            if (GUILayout.Button("Build"))
            {
                repeatedObject.Build();
            }
        }
    }
}
