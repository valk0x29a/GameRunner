using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunCooldownUpgrades", menuName = "Progression/ShotgunCooldownUpgrades")]
public class ShotgunCooldownUpgrades : UpgradeData
{
    [SerializeField]
    float[] shotgunCooldownUpgrades;

    public override void Upgrade()
    {
        InventoryManager.instance.SetCooldown(3,shotgunCooldownUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }
    void OnEnable()
    {
        SetUpgradeLevel(0);
    }
}
