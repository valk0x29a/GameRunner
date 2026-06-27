//using Codice.CM.SEIDInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
//using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using System.ComponentModel;
//using static UnityEngine.Rendering.DebugUI;

public struct DebugVariableInfo
{
    public FieldInfo fieldInfo;
    public DebugVariable debugVariable;

    public DebugVariableInfo(FieldInfo field,DebugVariable variable)
    {
        fieldInfo = field;
        debugVariable = variable;
    } 
}

public class CompactDebug : EditorWindow
{
    List<DebugVariableInfo> debugVariables;
    object value;
    string fieldName;
    Vector2 scrollPos;
    // GUIStyle line;
    [MenuItem("Window/Compact Debug")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CompactDebug));
    }

    public void OnEnable()
    {
        // line = new GUIStyle();
        // line.normal.background = EditorGUIUtility.whiteTexture;
        // line.margin = new RectOffset( 0, 0, 4, 4 );
        // line.fixedHeight = 1f;
        value = "N/A";
        name = "N/A";
        UpdateDebugVariables();
    }
    public void OnGUI()
    {
        // double tempTime = EditorApplication.timeSinceStartup;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false,false);

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Debug Variables", EditorStyles.boldLabel);
       if (GUILayout.Button("Update Debug Variables", EditorStyles.toolbarButton)) { UpdateDebugVariables(); }
        GUILayout.EndHorizontal();

        // Color c = GUI.color;
        // GUI.color = Color.gray;
        // GUILayout.Box(GUIContent.none,line);
        // GUI.color = c;

      GUILayout.Space(2);

        foreach (DebugVariableInfo debugField in debugVariables)
        {
            DebugVariable variable = debugField.debugVariable;
            FieldInfo field = debugField.fieldInfo;
            Type fieldType = field.FieldType;

            if (FindAnyObjectByType(field.ReflectedType) != null)
            {

                value = field.GetValue(FindAnyObjectByType(field.ReflectedType));

                if (variable.displayName != "~")
                {
                    fieldName = variable.displayName;
                }
                else
                {
                    fieldName = field.Name;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(fieldName);
                
                string declaredType = fieldType.ToString();
                
                // double tempTimeS = EditorApplication.timeSinceStartup;
                if(declaredType == "System.String") { value = value.ToString(); }
                else if(declaredType == "UnityEngine.Vector3")
                {
                    string vector = value.ToString();
                     vector = vector.Substring(1, vector.Length - 2);

                     string[] coordinates = vector.Split(',');
                     value = new Vector3(
                        float.Parse(coordinates[0], CultureInfo.InvariantCulture),
                        float.Parse(coordinates[1], CultureInfo.InvariantCulture),
                        float.Parse(coordinates[2], CultureInfo.InvariantCulture)
                        );            
                }
                // Debug.Log((EditorApplication.timeSinceStartup - tempTimeS) * 1000);
               // Debug.Log(field.Name + " / " + declaredType + " / " + field.FieldType.BaseType);
                if (declaredType == "System.Boolean")
                {
                    bool temp = EditorGUILayout.Toggle((bool)value);
                    if (temp != (bool)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if (declaredType == "System.String")
                {
                    string temp = EditorGUILayout.TextField((string)value);
                    if (temp != (string)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if (declaredType == "System.Int32")
                {
                    int temp = EditorGUILayout.IntField((int)value);
                    if (temp != (int)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if (declaredType == "System.Single")
                {
                    float temp = EditorGUILayout.FloatField((float)value);
                    if (temp != (float)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if (declaredType == "System.Double")
                {
                    double temp = EditorGUILayout.DoubleField((double)value);
                    if (temp != (double)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if (declaredType == "UnityEngine.Vector3")
                {
                    Vector3 temp = EditorGUILayout.Vector3Field("", (Vector3)value);
                    if (temp != (Vector3)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if (declaredType == "UnityEngine.LayerMask")
                {
                    LayerMask layerMask = (LayerMask)value >> 1;
                    layerMask = EditorGUILayout.MaskField("", layerMask, InternalEditorUtility.layers) << 1;
                    if (layerMask != (LayerMask)value >> 1) field.SetValue(FindAnyObjectByType(field.ReflectedType), (LayerMask)layerMask);
                }
                else if (declaredType == "UnityEngine.AnimationCurve")
                {
                    AnimationCurve temp = EditorGUILayout.CurveField("", (AnimationCurve)value);
                    if (temp != (AnimationCurve)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                }
                else if(!fieldType.BaseType.ToString().StartsWith("System."))
                {
                    // GUI.enabled = false;
                    // double tempTimeS = EditorApplication.timeSinceStartup;
                    string assemblyName;
                    assemblyName = Assembly.GetAssembly(fieldType).FullName;
                    // Debug.Log((EditorApplication.timeSinceStartup - tempTimeS) * 1000);
                    // Debug.Log(field.Name + " / " + field.FieldType);         
                    UnityEngine.Object temp = EditorGUILayout.ObjectField((UnityEngine.Object)value, Type.GetType(declaredType + ", " + assemblyName), true);
                    if (temp != (UnityEngine.Object)value) field.SetValue(FindAnyObjectByType(field.ReflectedType), temp);
                    // GUI.enabled = true;
                }
                else
                {
                    EditorGUILayout.LabelField("Object Type Not Suported"); 
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        // EditorGUILayout.LabelField(((EditorApplication.timeSinceStartup-tempTime)*1000).ToString());
        EditorGUILayout.EndScrollView();
    }

    public void OnInspectorUpdate() {  Repaint(); }

    void UpdateDebugVariables()
    {
        // double temp = EditorApplication.timeSinceStartup;
        debugVariables = new List<DebugVariableInfo>();
        Assembly assembly = Assembly.Load("Assembly-CSharp");
       
        Type[] types = assembly.GetTypes();
        foreach (Type type in types)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo[] fields = type.GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                if(field.CustomAttributes.Count() > 0)
                {  
                    DebugVariable variable = field.GetCustomAttribute<DebugVariable>();
                    if (variable != null)
                    {
                        debugVariables.Add(new DebugVariableInfo (field, variable));
                    }
                }
            }
        }
        // Debug.Log((EditorApplication.timeSinceStartup - temp) * 1000);
    }

}


