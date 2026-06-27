using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager instance;

    public int normalPointsPerKill;

    int points;

    GameObject Player;
    GameObject PlayerHUD;
    PlayerHUDController playerHUD;

    [Header("Upgrades")]
    [SerializeField]
    DashUpgrades dashUpgrades;

    [SerializeField]
    MaxHealthUpgrades maxHealthUpgrades;

    [SerializeField]
    HandgunDamageUpgrades handgunDamageUpgrades;

    [SerializeField]
    SharpenerMagazineUpgrades sharpenerMagazineUpgrades;

    [SerializeField]
    SharpenerDamageUpgrades sharpenerDamageUpgrades;

    [SerializeField]
    ShotgunMagazineUpgrades shotgunMagazineUpgrades;

    [SerializeField]
    ShotgunCooldownUpgrades shotgunCooldownUpgrades;

    [SerializeField]
    ShotgunAdditionalPelletsUpgrades shotgunAdditionalPelletsUpgrades;

    [SerializeField]
    ExploderUpgrades exploderUpgrades;

    [SerializeField]
    int DecoyPrize;
    [SerializeField]
    int ExplosiveGrenadePrize;
    [SerializeField]
    int FreezingGrenadePrize;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the scene");
        }
        else
        {
            instance = this;
            InitalizeManager();
        }
    }

    void InitalizeManager()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerHUD = GameObject.FindWithTag("PlayerHUD");
        playerHUD = PlayerHUD.GetComponent<PlayerHUDController>();  
    }

    void Start() 
    { 
        playerHUD.UpdateTextVisibility();
    }

    public void Upgrade(int index)
    {
        points -= GetUpgradeData(index).GetUpgradeCost();
        GetUpgradeData(index).Upgrade();
        if(index == 3) { playerHUD.UpdateTextVisibility(); }
    }

    public void Buy(int buyIndex)
    {
        if (buyIndex == 0)
        {
            UtilityItemData decoy = ItemsDataManager.instance.FindUtilityItemByName("Decoy");
            InventoryManager.instance.ChangeOrAddUtility(decoy, 4, 1);
            points -= DecoyPrize;
        }
        if (buyIndex == 1)
        {
            UtilityItemData explosiveGrenade = ItemsDataManager.instance.FindUtilityItemByName("Explosive Grenade");
            InventoryManager.instance.ChangeOrAddUtility(explosiveGrenade, 5, 1);
            points -= ExplosiveGrenadePrize;
        }
        if (buyIndex == 2)
        {
            UtilityItemData freezingGrenade = ItemsDataManager.instance.FindUtilityItemByName("Freezing Grenade");
            InventoryManager.instance.ChangeOrAddUtility(freezingGrenade, 5, 1);
            points -= FreezingGrenadePrize;
        }
    }

    public void AddPoints(int pointsToAdd) => points += pointsToAdd; 

    public void DecreasePoints(int pointsToDecrease) => points -= pointsToDecrease; 

    public int GetPoints() => points; 

    public bool IsUpgradeAvalible(int index) => GetUpgradeData(index).IsUpgradeAvalible(points);

    public bool IsItemAvalible(int index)
    {
        return index switch
        {
            0 => points >= DecoyPrize,
            1 => points >= ExplosiveGrenadePrize,
            2 => points >= FreezingGrenadePrize,
            _ => false,
        };
    }

    public bool IsUpgradeMaxedOut(int index) => GetUpgradeData(index).IsUpgradeMaxedOut();
    
    public int GetUpgradeMaxLevel(int index) => GetUpgradeData(index).GetUpgradeMaxLevel();

    public int GetUpgradeLevel(int index) => GetUpgradeData(index).GetUpgradeLevel();

    public int GetUpgradeCost(int index) => GetUpgradeData(index).GetUpgradeCost();

    UpgradeData GetUpgradeData(int index)
    {
        return index switch
        {
            0 => dashUpgrades,
            1 => maxHealthUpgrades,
            2 => sharpenerMagazineUpgrades,
            3 => exploderUpgrades,
            4 => sharpenerDamageUpgrades,
            5 => handgunDamageUpgrades,
            6 => shotgunMagazineUpgrades,
            7 => shotgunCooldownUpgrades,
            8 => shotgunAdditionalPelletsUpgrades,
            _ => null,
        };
    }
}
