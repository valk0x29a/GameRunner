using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIButton : MonoBehaviour
{
    public bool isItem;

    public bool hasToBeBought;
    public string upgradeName;
    [SerializeField]
    int index;

    [SerializeField]
    bool dontAddOne;

    Button button;

    Text upgradeText;
    Text upgradeDetailsText;

    string defaultText;

    int uiLevelOffset;

    public int GetIndex() { return index; }
    void Awake()
    {
        button = GetComponent<Button>();
        GetTexts();
        if(hasToBeBought) { defaultText = upgradeText.text;}
    }

    void Update()
    {
        if(!isItem) { button.interactable = ProgressionManager.instance.IsUpgradeAvalible(index); }
        else {button.interactable = ProgressionManager.instance.IsItemAvalible(index); }
    }

    public void UpdateText(string detailsTextFormat, string maxedOutDetailsTextFormat)
    {
        if(isItem) { return; }
        uiLevelOffset = dontAddOne ? 0 : 1;
        ProgressionManager progression = ProgressionManager.instance;

        upgradeDetailsText.text = string.Format(detailsTextFormat, progression.GetUpgradeLevel(index)+uiLevelOffset,progression.GetUpgradeMaxLevel(index)+uiLevelOffset,progression.GetUpgradeCost(index));

        if(hasToBeBought)
        {
            if(progression.GetUpgradeLevel(index) == -uiLevelOffset)
            {
                string temp = upgradeName;

                if(index == 2) { temp = "Sharpener"; }
                if(index == 6) { temp = "Shotgun"; }

                upgradeText.text = "Buy " + temp;
                upgradeDetailsText.text = progression.GetUpgradeCost(index) + " P";
            }
            else { upgradeText.text = defaultText; }
        }

        if(progression.IsUpgradeMaxedOut(index))
        {
            upgradeText.text = upgradeName + " Maxed Out";
            upgradeDetailsText.text = string.Format(maxedOutDetailsTextFormat, progression.GetUpgradeMaxLevel(index)+uiLevelOffset);
        }
    }

    void GetTexts()
    {
        upgradeText = button.transform.GetChild(0).GetComponent<Text>();
        upgradeDetailsText = button.transform.GetChild(1).GetComponent<Text>();
    }
}
