using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SharpenerDamageUpgrades", menuName = "Progression/SharpenerDamageUpgrades")]
public class SharpenerDamageUpgrades : UpgradeData
{
    [SerializeField]
    int[] sharpenerDamageUpgrades;

    public SharpenerMagazineUpgrades sharpenerMagazine;

    public override bool IsUpgradeAvalible(int pointsAvailable)
    {
        if(sharpenerMagazine.GetUpgradeLevel() == -1) { return false; }
        return base.IsUpgradeAvalible(pointsAvailable);
    }
    public override void Upgrade()
    {
        InventoryManager.instance.SetDamage(2, sharpenerDamageUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }
    void OnEnable()
    {
        SetUpgradeLevel(0);
    }

}
