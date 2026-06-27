using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthUpgrades", menuName = "Progression/MaxHealthUpgrades")]
public class MaxHealthUpgrades : UpgradeData
{
    [SerializeField]
    int[] maxHealthUpgrades;

    public override void Upgrade()
    {
        PlayerManager.instance.SetMaxHealth(maxHealthUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }

    void OnEnable() {  SetUpgradeLevel(0); }
}
