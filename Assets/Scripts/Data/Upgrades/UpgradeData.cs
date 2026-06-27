using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Progression/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    int upgradeLevel;

    [SerializeField]
    int[] upgradeCosts;

    public virtual int GetUpgradeLevel() { return upgradeLevel; }
    public virtual int GetUpgradeMaxLevel() { return upgradeCosts.Length; } 
    public virtual bool IsUpgradeAvalible(int pointsAvailable)
    {
        if(upgradeLevel != upgradeCosts.Length) 
        { 
            return pointsAvailable >= upgradeCosts[upgradeLevel]; 
        }
        return false;
    }

    public virtual bool IsUpgradeMaxedOut() { return upgradeLevel == upgradeCosts.Length; }

    public virtual int GetUpgradeCost() 
    { 
        if(!IsUpgradeMaxedOut()) { return upgradeCosts[upgradeLevel]; }
        return -1;
    }

    public void SetUpgradeLevel(int level)
    {
        upgradeLevel = level;
    }

    public virtual void Upgrade() { }

}
