using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WavesManager))]
public class WavesManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty interactions = serializedObject.FindProperty("interactionInputs");
        EditorGUILayout.PropertyField(interactions, false);
        if(GUILayout.Button("Open Waves Editor"))
        {
            WavesManagerEditorWindow.Open();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
