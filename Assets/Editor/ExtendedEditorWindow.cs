using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;

    protected SerializedProperty[] properties;

    protected int currentIndex = 0;

    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        if(prop == null) { return; }
        EditorGUILayout.LabelField(prop.displayName.Replace("Element","Wave"), EditorStyles.centeredGreyMiniLabel);
        string lastPropPath = string.Empty;
        foreach(SerializedProperty p in prop)
        {
            if(p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded,p.displayName);
                EditorGUILayout.EndHorizontal();

                if(p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p,drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { return; }
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }
    protected void DrawSidebar(SerializedProperty prop)
    {
        int count = 0;
        foreach(SerializedProperty p in prop)
        {
            count++;
            if(GUILayout.Button(p.displayName.Replace("Element","Wave")))
            {
                selectedPropertyPath = p.propertyPath;
                currentIndex = count-1;
            }
        }

        properties = new SerializedProperty[count];

        int i = 0;
        foreach(SerializedProperty p in prop)
        {
            properties[i] = p;
            i++;
        }

        if(!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        }
    }

    protected void SelectNextProperty()
    {
        if(EditorGUIUtility.editingTextField) { return; }
        currentIndex++;
        if(currentIndex == properties.Length) { currentIndex = 0; }
        selectedPropertyPath = properties[currentIndex].propertyPath;
    }

    protected void SelectPreviousProperty()
    {
        if(EditorGUIUtility.editingTextField) { return; }
        currentIndex--;
        if(currentIndex == -1) { currentIndex = properties.Length-1; }
        selectedPropertyPath = properties[currentIndex].propertyPath;
    }

    protected void RefreshCurrentProperty()
    {
        selectedPropertyPath = properties[currentIndex].propertyPath;
    }
}
