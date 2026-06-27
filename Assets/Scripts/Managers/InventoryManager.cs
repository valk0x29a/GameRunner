using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the scene");
        }
        else
        {
            instance = this;
            InitalizeManager();
        }
    }

    [Header("Edited By Other Managers")]
    public InventoryWeaponData FirstWeapon;
    public InventoryWeaponData SecondWeapon;
    public InventoryWeaponData ThirdWeapon;

    public InventoryWeaponData equippedWeapon { get { return GetEquippedWeapon(); }}

    public InventoryUtilityData Decoy;
    public InventoryUtilityData[] Grenades;

    public InventoryUtilityData equippedUtility;

    public int quantityOfGrenadeTypes;

    public int equippedWeaponIndex;

    public int equippedUtilityIndex;

    public int equippedGrenade;

    GameObject PlayerHUD;

    PlayerHUDController playerHUD;

    InventoryWeaponData GetEquippedWeapon() => GetWeaponSlot(equippedWeaponIndex, false);

    public void ChangeOrAddWeapon(WeaponItemData weapon, int slot)
    {
        if (!weapon.isWeapon) { return; }
        SetWeaponData(weapon, GetWeaponSlot(slot, true));
    }

    public void ChangeOrAddUtility(UtilityItemData utility, int slot, int quantity)
    {
        if(slot == 4)
        {
            if (Decoy.itemName == "")
            {
                SetUtilityData(utility, ref Decoy, quantity);
            }
            else
            {
                Decoy.quantity += quantity;
            }
        }
        if(slot == 5)
        {
            for(int i = 0; i < Grenades.Length; i++)
            {
                if(Grenades[i].itemName == utility.itemName)
                {
                    Grenades[i].quantity += quantity;
                    return;
                }
            }
           // Grenades[quantityOfGrenadeTypes] = new InventoryUtilityData();
            SetUtilityData(utility, ref Grenades[quantityOfGrenadeTypes], quantity);
            quantityOfGrenadeTypes++;
        }
        playerHUD.UpdateTextVisibility();
    }
    public void EquipWeapon(int slot)
    {
        if (GetWeaponSlot(slot, false) == null || GetWeaponSlot(slot, false).itemName == "") { return; }

        equippedWeaponIndex = slot;
    }

    public void EquipUtility(int slot)
    {
        if(slot == 4 && Decoy.itemName == "Decoy" && Decoy.quantity > 0)
        { 
            equippedUtility = Decoy;
            equippedUtilityIndex = 4;
        }
        else if(slot == 5 && quantityOfGrenadeTypes > 0)
        {
            if (equippedUtilityIndex == 5)
            { 
                equippedGrenade++;
                if (equippedGrenade > quantityOfGrenadeTypes - 1) { equippedGrenade = 0; }
            }
            equippedUtility = Grenades[equippedGrenade];
            equippedUtilityIndex = 5;
        }
        playerHUD.UpdateTextVisibility();
    }

    public void UseUtility()
    {
        if(equippedUtility.quantity <= 0)
        {
            if(equippedUtilityIndex == 4)
            {
                Decoy.itemName = "";
                equippedUtility = null;
            }
            else if(equippedUtilityIndex == 5)
            {
                Grenades[equippedGrenade].itemName = "";
                equippedUtility = new InventoryUtilityData();
                Grenades[equippedGrenade] = new InventoryUtilityData();
                
                for(int i = equippedGrenade; i < quantityOfGrenadeTypes-1;i++)
                {
                    Grenades[i] = Grenades[i + 1];
                }
                Grenades[quantityOfGrenadeTypes - 1] = new InventoryUtilityData();
                quantityOfGrenadeTypes--;
                if(equippedGrenade == quantityOfGrenadeTypes && equippedGrenade != 0) { equippedGrenade--; }
                if (quantityOfGrenadeTypes > 0) { equippedUtility = Grenades[equippedGrenade]; }
            }
        }
        playerHUD.UpdateUtilityText();
    }
    void SetWeaponData(WeaponItemData item, InventoryWeaponData weapon)
    {
        if (item.isWeapon)
        {
            weapon.itemName = item.itemName;
            weapon.isWeapon = true;
            weapon.damage = item.damage;
            weapon.shieldPenetrationRate = item.shieldPenetrationRate;

            weapon.cooldown = item.cooldown;
            weapon.cooldownLeft = item.cooldown;
            weapon.isCooldownActive = false;

            weapon.model = item.model;
            weapon.modelPosition = item.modelPosition;

            weapon.range = item.range;

            weapon.reloadingTime = item.reloadingTime;
            weapon.reloadingTimeLeft = item.reloadingTime;
            weapon.isReloading = false;
            weapon.maxAmountOfBulletsInMagazine = item.magazineSize;
            weapon.ammoLeftInMagazine = item.magazineSize;

            weapon.upDownRecoil = item.upDownRecoil;
            weapon.notMovingUpDownRecoil = item.notMovingUpDownRecoil;
            weapon.sprintUpDownRecoil = item.sprintUpDownRecoil;
            weapon.upDownRecoilLerpSpeed = item.upDownRecoilLerpSpeed;
            weapon.upDownRecoilReturnLerpSpeed = item.upDownRecoilReturnLerpSpeed;

            weapon.leftRightRecoilRange = item.leftRightRecoilRange;
            weapon.notMovingLeftRightRecoilRange = item.notMovingLeftRightRecoilRange;
            weapon.sprintLeftRightRecoilRange = item.sprintLeftRightRecoilRange;
            weapon.leftRightRecoilLerpSpeed = item.leftRightRecoilLerpSpeed;
            weapon.leftRightRecoilReturnLerpSpeed = item.leftRightRecoilReturnLerpSpeed;

            if (item.gunMode == WeaponItemData.GunMode.Automatic)
            {
                weapon.gunMode = InventoryWeaponData.GunMode.Automatic;
            }
            else if (item.gunMode == WeaponItemData.GunMode.Burst)
            {
                weapon.gunMode = InventoryWeaponData.GunMode.Burst;
            }
            else if (item.gunMode == WeaponItemData.GunMode.Single)
            {
                weapon.gunMode = InventoryWeaponData.GunMode.Single;
            }
        }
    }

    void SetUtilityData(UtilityItemData item, ref InventoryUtilityData utility, int quantity)
    {
        utility.itemName = item.itemName;
        utility.quantity = quantity;

        utility.toInstance = item.toInstance;
        utility.modelPosition = item.modelPosition;

        utility.cooldown = item.cooldown;
        utility.cooldownLeft = item.cooldown;
        utility.isCooldownActive = false;

        utility.range = item.range;

     /*   utility.upDownRecoil = item.upDownRecoil;
        utility.notMovingUpDownRecoil = item.notMovingUpDownRecoil;
        utility.sprintUpDownRecoil = item.sprintUpDownRecoil;
        utility.upDownRecoilLerpSpeed = item.upDownRecoilLerpSpeed;
        utility.upDownRecoilReturnLerpSpeed = item.upDownRecoilReturnLerpSpeed;

        utility.leftRightRecoilRange = item.leftRightRecoilRange;
        utility.notMovingLeftRightRecoilRange = item.notMovingLeftRightRecoilRange;
        utility.sprintLeftRightRecoilRange = item.sprintLeftRightRecoilRange;
        utility.leftRightRecoilLerpSpeed = item.leftRightRecoilLerpSpeed;
        utility.leftRightRecoilReturnLerpSpeed = item.leftRightRecoilReturnLerpSpeed;
     */
    }

    public void SetMagazineSize(int weaponIndex, int magazineSize)
    {
        GetWeaponSlot(weaponIndex, true).maxAmountOfBulletsInMagazine = magazineSize;
    }

    public void SetDamage(int weaponIndex, int damage)
    {
        GetWeaponSlot(weaponIndex, true).damage = damage;
    }

    public void SetCooldown(int weaponIndex, float cooldown)
    {
        InventoryWeaponData weaponData = GetWeaponSlot(weaponIndex, true);
        weaponData.cooldown = cooldown;
        if(weaponData.cooldown < weaponData.cooldownLeft)
        { 
            weaponData.cooldownLeft = cooldown;
        }
    }

    public void SetShotgunAdditonalPellets(int additionalPellets)
    {
        ThirdWeapon.shieldPenetrationRate = additionalPellets;
        if(equippedWeaponIndex == 3) GetWeaponSlot(3, true).shieldPenetrationRate = additionalPellets;
    }

    InventoryWeaponData GetWeaponSlot(int index, bool clamp)
    {
        if(clamp) { index = Mathf.Clamp(index, 1, 3); }
        return index switch
        {
            1 => FirstWeapon,
            2 => SecondWeapon,
            3 => ThirdWeapon,
            _ => null
        };
    }

    void InitalizeManager()
    {
        PlayerHUD = GameObject.FindWithTag("PlayerHUD");
        playerHUD = PlayerHUD.GetComponent<PlayerHUDController>();
        quantityOfGrenadeTypes = 0;
        equippedGrenade = 0;
        WeaponItemData handgun = ItemsDataManager.instance.FindWeaponItemByName("Handgun");
       // WeaponItemData hitscanAutomaticWeapon = ItemsDataManager.instance.FindWeaponItemByName("Sharpener");
        WeaponItemData hitscanSingleWeapon = ItemsDataManager.instance.FindWeaponItemByName("Sniper Gun");
       // WeaponItemData shotgun = ItemsDataManager.instance.FindWeaponItemByName("Shotgun");
        ChangeOrAddWeapon(handgun, 1);
        ChangeOrAddWeapon(hitscanSingleWeapon, 2);
       // ChangeOrAddWeapon(shotgun, 3);
        EquipWeapon(1);
    }
    
}
