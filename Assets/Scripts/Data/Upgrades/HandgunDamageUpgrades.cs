using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandgunDamageUpgrades", menuName = "Progression/HandgunDamageUpgrades")]
public class HandgunDamageUpgrades : UpgradeData
{
    [SerializeField]
    int[] handgunDamageUpgrades;

    public override void Upgrade()
    {
        InventoryManager.instance.SetDamage(1, handgunDamageUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }

    void OnEnable() 
    {  
        SetUpgradeLevel(0);
    }
}
