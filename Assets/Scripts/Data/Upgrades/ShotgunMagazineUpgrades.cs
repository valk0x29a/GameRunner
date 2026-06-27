using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunMagazineUpgrades", menuName = "Progression/ShotgunMagazineUpgrades")]
public class ShotgunMagazineUpgrades : UpgradeData
{
    [SerializeField]
    int[] shotgunMagazineUpgrades;

    [SerializeField]
    int shotgunPrize;

    public override int GetUpgradeCost()
    {
        if(GetUpgradeLevel() == -1) { return shotgunPrize; }
        return base.GetUpgradeCost();
    }

    public override bool IsUpgradeAvalible(int pointsAvailable)
    {
        if(GetUpgradeLevel() == -1) { return pointsAvailable >= shotgunPrize; }
        return base.IsUpgradeAvalible(pointsAvailable);
    }

    public override void Upgrade()
    {
        if(GetUpgradeLevel() == -1)
        {
            WeaponItemData shotgun = ItemsDataManager.instance.FindWeaponItemByName("Shotgun");
            InventoryManager.instance.ChangeOrAddWeapon(shotgun, 3);
            InventoryManager.instance.EquipWeapon(3);
            SetUpgradeLevel(GetUpgradeLevel()+1);
            return;
        }
        InventoryManager.instance.SetMagazineSize(3, shotgunMagazineUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }
    void OnEnable()
    {
        SetUpgradeLevel(-1);
    }
}
