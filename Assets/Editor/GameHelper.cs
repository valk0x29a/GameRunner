using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using UnityEditor.UIElements;
using UnityEngine.Experimental.Rendering;

public class GameHelper : EditorWindow
{
    static int sharpenerPrize = 0;
    static SharpenerMagazineUpgrades sharpenerMagazine;

    GameObject Player;
    [MenuItem("Window/Game Helper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GameHelper));
    }

    void OnEnable()
    {
        sharpenerPrize = EditorPrefs.GetInt("Sharpener Prize");
        sharpenerMagazine = AssetDatabase.LoadAssetAtPath<SharpenerMagazineUpgrades>("Assets/Upgrades/SharpenerMagazineUpgrades.asset");
        Player = GameObject.Find("Player");
    }

    private void OnGUI()
    {
        UpdateSharpenerPrizeGUI();
        UpdateEnemyStateGUI();
        if (GUILayout.Button("Focus On Player", EditorStyles.miniButton))
        {
            Selection.activeGameObject = GetPlayer();
            SceneView.lastActiveSceneView.orthographic = false;
            SceneView.lastActiveSceneView.in2DMode = false;
            SceneView.FrameLastActiveSceneView();
        }
        if(GUILayout.Button("Focus On UI", EditorStyles.miniButton))
        {
            Selection.activeGameObject = FindAnyObjectByType<Canvas>().gameObject;
            SceneView.lastActiveSceneView.orthographic = true;
            SceneView.lastActiveSceneView.in2DMode = true;
            SceneView.FrameLastActiveSceneView();
        }
    }

    void UpdateSharpenerPrizeGUI()
    {
        if (GUILayout.Button("Sharpener for free", EditorStyles.miniButton))
        {
            int t = sharpenerMagazine.SetSharpenerForFreeEditorOnly();
            if (t != -1) { sharpenerPrize = t; }
            EditorPrefs.SetInt("Sharpener Prize", sharpenerPrize);
        };
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Restore Sharpener Prize", EditorStyles.miniButton))
        {
            sharpenerMagazine.RestoreSharpenerPrizeEditorOnly(sharpenerPrize);
            EditorPrefs.SetInt("Sharpener Prize", sharpenerPrize);
        }

        GUILayout.Label("Saved Prize:");
        GUILayout.TextField(sharpenerPrize.ToString());
        EditorGUILayout.EndHorizontal();
    }

    void UpdateEnemyStateGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Turn Off Enemies", EditorStyles.miniButton))
        {
            GameObject.Find("Game Managers").GetComponent<WavesManager>().enabled = false;
            GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enabled = false;
        };
        if (GUILayout.Button("Turn On Enemies", EditorStyles.miniButton))
        {
            GameObject.Find("Game Managers").GetComponent<WavesManager>().enabled = true;
            GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enabled = true;
        };
        EditorGUILayout.EndHorizontal();
    }

    GameObject GetPlayer()
    {
        if(Player == null) { return GameObject.Find("Player") ;}
        return Player;
    }
}
