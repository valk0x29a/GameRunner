using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UpgraderUIController : MonoBehaviour
{
    public Image UpgraderUI;

    CursorController cursor;
    CrosshairController crosshair;
    public Button sharpenerDamageUpgradeButton;
    public Button shotgunCooldownUpgradeButton;
    public Button shotgunAdditionalPelletsUpgradeButton;
    public string detailsTextFormat;
    public string maxedOutDetailsTextFormat;
    void Start()
    {
        InitializeInputAsset();
        cursor = GameObject.Find("Game Managers").GetComponent<CursorController>();
        crosshair = PlayerManager.instance.GetPlayer().GetComponent<CrosshairController>();
        CloseUI();
    }

    void Update()
    {
        if (UpgraderUI.gameObject.activeInHierarchy)
        {
            UpgradeUIButton[] buttons = FindObjectsByType<UpgradeUIButton>(FindObjectsSortMode.None);

            foreach(UpgradeUIButton button in buttons)
            {
                button.UpdateText(detailsTextFormat, maxedOutDetailsTextFormat);
            }

            UpdateButtonVisibility();
        }
    }

    void UpdateButtonVisibility()
    {
        sharpenerDamageUpgradeButton.gameObject.SetActive(ProgressionManager.instance.GetUpgradeLevel(2) != -1);
        shotgunCooldownUpgradeButton.gameObject.SetActive(ProgressionManager.instance.GetUpgradeLevel(6) != -1);
        shotgunAdditionalPelletsUpgradeButton.gameObject.SetActive(ProgressionManager.instance.GetUpgradeLevel(6) != -1);
    }

    public void OpenOrCloseUI()
    {
        bool active = UpgraderUI.gameObject.activeInHierarchy;
        UpgraderUI.gameObject.SetActive(!active);
        cursor.SetCursorActive(!active);
        crosshair.SetCrosshairVisibility(active);
    }

    public void OpenUI()
    {
        UpgraderUI.gameObject.SetActive(true);
        cursor.SetCursorActive(true);
        crosshair.SetCrosshairVisibility(false);
    }

    public void CloseUI()
    {
        if (UpgraderUI != null && UpgraderUI.gameObject.activeInHierarchy)
        {
            UpgraderUI.gameObject.SetActive(false);
            cursor.SetCursorActive(false);
            crosshair.SetCrosshairVisibility(true);
        }
    }

    public void Upgrade(int index)
    {
        if (ProgressionManager.instance.IsUpgradeAvalible(index))
        {
            ProgressionManager.instance.Upgrade(index);
        }
    }
    public void Buy(int index)
    {
        if(ProgressionManager.instance.IsItemAvalible(index))
        {
            ProgressionManager.instance.Buy(index);
        }
    }

    public bool IsUpgraderUIOpen()
    {
        return UpgraderUI.gameObject.activeInHierarchy;
    }

    void InitializeInputAsset()
    {
        InputSystem.actions.FindActionMap("Interactions").FindAction("Close").performed += contex => CloseUI();
    }
}
