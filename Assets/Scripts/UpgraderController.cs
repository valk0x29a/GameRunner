using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgraderController : MonoBehaviour
{
    public float maxInteractionDistance;

    Transform Player;

    bool isAbleToInteract;

    UpgraderUIController upgraderUI;

    void Start()
    {
        InitializeInputAsset();
        Player = GameObject.Find("Player").transform;
        upgraderUI = GameObject.FindWithTag("PlayerHUD").GetComponent<UpgraderUIController>();
    }


    void Update()
    {
        isAbleToInteract = Vector3.Distance(Player.transform.position, transform.position) < maxInteractionDistance;        
    }

    void OpenUpgrader()
    {
        if(isAbleToInteract)
        {
            upgraderUI.OpenOrCloseUI();
        }
    }

    void InitializeInputAsset()
    {
        InputSystem.actions.FindActionMap("Interactions").FindAction("Open").performed += contex => OpenUpgrader();
    }

    private void OnDisable()
    {
        upgraderUI.CloseUI();
        isAbleToInteract = false;
    }

}
