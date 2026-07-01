using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WavesManager))]
public class WavesManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Waves Editor"))
        {
            WavesManagerEditorWindow.Open();
        }
    }
}
