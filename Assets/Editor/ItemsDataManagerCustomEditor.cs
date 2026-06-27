using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemsDataManager))]
public class ItemsDataManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Weapons' Editor"))
        {
            WavesManagerEditorWindow.Open();
        }
        base.OnInspectorGUI();
    }
}
