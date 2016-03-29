using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(SomeScript))]
public class SomeScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
}
