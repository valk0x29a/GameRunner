using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SharpenerMagazineUpgrades", menuName = "Progression/SharpenerMagazineUpgrades")]
public class SharpenerMagazineUpgrades : UpgradeData
{
    [SerializeField]
    int[] sharpenerMagazineUpgrades;

    [SerializeField]
    int sharpenerPrize;
    public override int GetUpgradeCost()
    {
        if(GetUpgradeLevel() == -1) { return sharpenerPrize;}
        return base.GetUpgradeCost();
    }

    public override bool IsUpgradeAvalible(int pointsAvailable)
    {
        if(GetUpgradeLevel() == -1) {return pointsAvailable >= sharpenerPrize; }
        return base.IsUpgradeAvalible(pointsAvailable);
    }
    public override void Upgrade()
    {
        if(GetUpgradeLevel() == -1)
        {
            WeaponItemData hitscanAutomaticWeapon = ItemsDataManager.instance.FindWeaponItemByName("Sharpener");
            InventoryManager.instance.ChangeOrAddWeapon(hitscanAutomaticWeapon, 2);
            InventoryManager.instance.EquipWeapon(2);
            SetUpgradeLevel(GetUpgradeLevel()+1);
            return;
        }
        InventoryManager.instance.SetMagazineSize(2, sharpenerMagazineUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }

    void OnEnable()
    {
        SetUpgradeLevel(-1);
    }

    public int SetSharpenerForFreeEditorOnly()
    {
        int t = sharpenerPrize;
        sharpenerPrize = 0;
        if (t == 0) { return -1; }
        return t;
    }

    public void RestoreSharpenerPrizeEditorOnly(int prize)
    {
        sharpenerPrize = prize;
    }
}
