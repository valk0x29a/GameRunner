using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.EditorTools;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;
using System.Drawing;

[Overlay(typeof(SceneView), "Game Helper Overlay", true)]
public class GameHelperOverlay : Overlay
{
    GameObject Player;
    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement() { name = "GameHelper Overlay" };
        root.Add(new Button(TurnOffEnemies) { text = "Turn Off Enemies" });
        root.Add(new Button(TurnOnEnemies) { text = "Turn On Enemies" });
        root.Add(new Button(FocusOnPlayer) { text = "Focus On Player" });
        root.Add(new Button(FocusOnUI) { text = "Focus On UI" });
        Player = GameObject.Find("Player");
        return root;
    }

    void TurnOffEnemies()
    {
        GameObject.Find("Game Managers").GetComponent<WavesManager>().enabled = false;
        GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enabled = false;
    }

    void TurnOnEnemies()
    {
        GameObject.Find("Game Managers").GetComponent<WavesManager>().enabled = true;
        GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enabled = true;
    }

    void FocusOnPlayer()
    {
        Selection.activeGameObject = GetPlayer();
        SceneView.lastActiveSceneView.orthographic = false;
        SceneView.lastActiveSceneView.in2DMode = false;
        SceneView.FrameLastActiveSceneView();
    }

    void FocusOnUI()
    {
        Selection.activeGameObject = GameObject.FindAnyObjectByType<Canvas>().gameObject;
        SceneView.lastActiveSceneView.orthographic = true;
        SceneView.lastActiveSceneView.in2DMode = true;
        SceneView.FrameLastActiveSceneView();  
    }

    GameObject GetPlayer()
    {
        if(Player == null)
        {
            Player = GameObject.Find("Player");
        }
        return Player;
    }

}


