using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExploderUpgrades", menuName = "Progression/ExploderUpgrades")]
public class ExploderUpgrades : UpgradeData
{
    [SerializeField]
    ExploderData[] exploderUpgrades;

    GameObject Player;

    public override void Upgrade()
    {
        if(GetUpgradeLevel() == 0) { Player.GetComponent<Exploder>().enabled = true; }
        Player.GetComponent<Exploder>().SetExploderData(exploderUpgrades[GetUpgradeLevel()]);
        SetUpgradeLevel(GetUpgradeLevel()+1);
    }

    void OnEnable() 
    {  
        SetUpgradeLevel(0); 
        Player = GameObject.FindWithTag("Player");
    }
}
