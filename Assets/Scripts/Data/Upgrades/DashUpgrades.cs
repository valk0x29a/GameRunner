using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashUpgrades", menuName = "Progression/DashUpgrades")]
public class DashUpgrades : UpgradeData
{
    [SerializeField]
    DashData[] dashUpgrades;
    
    PlayerMovement playerMovement;

    public override void Upgrade()
    {
        int playerDashlevel = GetUpgradeLevel();
        playerMovement.SetDashData(dashUpgrades[playerDashlevel]);
        SetUpgradeLevel(playerDashlevel+1); 
    }

    public void RegisterPlayer(PlayerMovement player)
    {
        playerMovement = player;
        playerMovement.SetDashData(dashUpgrades[GetUpgradeLevel()-1]);
    }
    void OnEnable() {  SetUpgradeLevel(1); }
}
