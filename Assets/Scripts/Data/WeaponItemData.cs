using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "Inventory/WeaponItemData")]
public class WeaponItemData  : ScriptableObject
{
    [Header("Item Variables")]
    public string itemName;
    public bool isWeapon;

    [Header("Model")]
    public GameObject model;
    public Vector3 modelPosition;

    [Header("Weapon Variables")]
    public int damage;
    public float shieldPenetrationRate;
    public float cooldown;
    public float range;

    [Header("Ammo")]
    public int magazineSize;
    public float reloadingTime;

    [Header("Up-Down Recoil")]
    public float upDownRecoil;
    public float notMovingUpDownRecoil;
    public float sprintUpDownRecoil;
    public float upDownRecoilLerpSpeed;
    public float upDownRecoilReturnLerpSpeed;

    [Header("Left-Right Recoil")]
    public float leftRightRecoilRange;
    public float notMovingLeftRightRecoilRange;
    public float sprintLeftRightRecoilRange;
    public float leftRightRecoilLerpSpeed;
    public float leftRightRecoilReturnLerpSpeed;

    
    public enum GunMode
    {
        Automatic,
        Burst,
        Single
    }

    [Header("Gun Mode")]
    public GunMode gunMode = GunMode.Automatic;
}
