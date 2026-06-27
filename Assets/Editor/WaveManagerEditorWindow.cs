using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WavesManagerEditorWindow : ExtendedEditorWindow
{
    static WavesManagerEditorWindow window;

    Vector2 scrollPos;
    [MenuItem("Window/Waves' Manager Editor")]
    public static void Open()
    {
        InitializeWindow();
    }

    static void InitializeWindow()
    {
        WavesManager waves = FindObjectOfType<WavesManager>();
        if(window == null) { window = GetWindow<WavesManagerEditorWindow>("Waves Editor"); }
        window.serializedObject = new SerializedObject(waves); 
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
    
        if(GUILayout.Button("Select Previous Wave"))
        {
            SelectPreviousProperty();
        }

        if(GUILayout.Button("Select Next Wave"))
        {
            SelectNextProperty();
        }

        EditorGUILayout.EndHorizontal();
      
        if(currentProperty == null) { InitializeWindow(); }
        currentProperty = window.serializedObject.FindProperty("WavesProperties");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150),GUILayout.ExpandHeight(true));
        DrawSidebar(currentProperty);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box",GUILayout.ExpandHeight(true));
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false,false);

        if(selectedProperty == null) { RefreshCurrentProperty(); }
        DrawProperties(selectedProperty,true); 
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        window.serializedObject.ApplyModifiedProperties();
    }
}
