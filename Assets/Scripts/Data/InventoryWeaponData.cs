using System;
using UnityEngine;
[Serializable]
public class InventoryWeaponData
{
    public string itemName;
    public bool isWeapon;

    public int damage;
    public float shieldPenetrationRate;

    public GameObject model;
    public Vector3 modelPosition;

    public float cooldown;
    public float cooldownLeft;

    public bool isCooldownActive;

    public float range;

    public float reloadingTime;
    public float reloadingTimeLeft;

    public int maxAmountOfBulletsInMagazine;
    public int ammoLeftInMagazine;

    public bool isReloading;

    public float upDownRecoil;
    public float notMovingUpDownRecoil;
    public float sprintUpDownRecoil;
    public float upDownRecoilLerpSpeed;
    public float upDownRecoilReturnLerpSpeed;

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
    public GunMode gunMode = GunMode.Automatic;

    public InventoryWeaponData(WeaponItemData item)
    {
        if(!item.isWeapon) { return; }
        itemName = item.itemName;
        isWeapon = true;
        damage = item.damage;
        shieldPenetrationRate = item.shieldPenetrationRate;

        cooldown = item.cooldown;
        cooldownLeft = item.cooldown;
        isCooldownActive = false;

        model = item.model;
        modelPosition = item.modelPosition;

        range = item.range;

        reloadingTime = item.reloadingTime;
        reloadingTimeLeft = item.reloadingTime;
        isReloading = false;
        maxAmountOfBulletsInMagazine = item.magazineSize;
        ammoLeftInMagazine = item.magazineSize;

        upDownRecoil = item.upDownRecoil;
        notMovingUpDownRecoil = item.notMovingUpDownRecoil;
        sprintUpDownRecoil = item.sprintUpDownRecoil;
        upDownRecoilLerpSpeed = item.upDownRecoilLerpSpeed;
        upDownRecoilReturnLerpSpeed = item.upDownRecoilReturnLerpSpeed;

        leftRightRecoilRange = item.leftRightRecoilRange;
        notMovingLeftRightRecoilRange = item.notMovingLeftRightRecoilRange;
        sprintLeftRightRecoilRange = item.sprintLeftRightRecoilRange;
        leftRightRecoilLerpSpeed = item.leftRightRecoilLerpSpeed;
        leftRightRecoilReturnLerpSpeed = item.leftRightRecoilReturnLerpSpeed;

        if (item.gunMode == WeaponItemData.GunMode.Automatic)
        {
            gunMode = GunMode.Automatic;
        }
        else if(item.gunMode == WeaponItemData.GunMode.Burst)
        {
            gunMode = GunMode.Burst;
        }
        else if (item.gunMode == WeaponItemData.GunMode.Single)
        {
            gunMode = GunMode.Single;
        }
    }
}
