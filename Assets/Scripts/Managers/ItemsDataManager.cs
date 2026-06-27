using UnityEngine;

public class ItemsDataManager : MonoBehaviour
{
    public static ItemsDataManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the scene");
        }
        else
        {
            instance = this;
        }
    }

    public WeaponItemData[] WeaponItemsInTheGame;
    public UtilityItemData[] UtilityItemsInTheGame;
    
    public WeaponItemData FindWeaponItemByName(string name)
    {
        foreach(WeaponItemData itemData in WeaponItemsInTheGame)
        {
            if (itemData.itemName == name) return itemData;
        }
        return WeaponItemsInTheGame[0];
    }
    public UtilityItemData FindUtilityItemByName(string name)
    {
        foreach (UtilityItemData itemData in UtilityItemsInTheGame)
        {
            if (itemData.itemName == name) return itemData;
        }
        return UtilityItemsInTheGame[0];
    }
}

