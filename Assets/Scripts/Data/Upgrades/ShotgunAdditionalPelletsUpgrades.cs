using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunAdditionalPelletsUpgrades", menuName = "Progression/ShotgunAdditionalPelletsUpgrades")]
public class ShotgunAdditionalPelletsUpgrades : UpgradeData
{
    [SerializeField]
    int[] shotgunAdditionalPelletsUpgrades;

    public override void Upgrade()
    {
        InventoryManager.instance.SetShotgunAdditonalPellets(shotgunAdditionalPelletsUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }

    void OnEnable()
    {
        SetUpgradeLevel(0);
    }
}
