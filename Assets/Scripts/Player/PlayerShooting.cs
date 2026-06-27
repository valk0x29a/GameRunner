using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Transform raycastPivot;

    [DebugVariable]
    bool isShootRequested;
    private InventoryWeaponData equippedWeapon;
    private WeaponVFX weaponVFX;
    private PlayerAudio playerAudio;
    private WeaponModelController weaponModelController;
    private PlayerWeaponRecoilController playerRecoil;
    private CrosshairController crosshair;
    private UpgraderUIController upgraderUI;

    public LayerMask crosshairRaycastLayer;
    public LayerMask shootingRaycastExludedLayer;
    void Start()
    {
        InitializeInputAsset();
        weaponVFX = GetComponent<WeaponVFX>();
        playerAudio = GetComponent<PlayerAudio>();
        weaponModelController = GetComponent<WeaponModelController>();
        playerRecoil = GetComponent<PlayerWeaponRecoilController>();
        crosshair = GetComponent<CrosshairController>();
        upgraderUI = GameObject.Find("PlayerHUD").GetComponent<UpgraderUIController>();
    }


    void Update()
    {
        equippedWeapon = InventoryManager.instance.equippedWeapon;

        CalculateWeaponCooldown();
        CalculateDistanceForCrosshair();

        CheckForAutoReload();

        if (equippedWeapon.isReloading)
        {
            CalculateReloading();
        }

        if (isShootRequested && !equippedWeapon.isCooldownActive && !equippedWeapon.isReloading)
        {
            ShootWeapon();
        }
    }
    void CheckForAutoReload()
    {
        if (equippedWeapon.ammoLeftInMagazine <= 0)
        {
            Reload();
        }
    }


    void CalculateWeaponCooldown()
    {
        if (equippedWeapon.isCooldownActive == true)
        {
            equippedWeapon.cooldownLeft -= Time.deltaTime;
            if (equippedWeapon.cooldownLeft <= 0)
            {
                equippedWeapon.cooldownLeft = equippedWeapon.cooldown;
                equippedWeapon.isCooldownActive = false;
            }
        }
    }
    void ShootWeapon()
    {
        playerRecoil.FireWeapon();
        equippedWeapon.isCooldownActive = true;
        equippedWeapon.ammoLeftInMagazine -= 1;

        weaponVFX.CreateWeaponFlash();

        playerAudio.PlayGunSound(equippedWeapon.itemName);

        if(equippedWeapon.itemName != "Shotgun")
        {
            weaponVFX.CreateBulletLine(transform.forward);
            if (Physics.Raycast(raycastPivot.position, raycastPivot.forward, out RaycastHit hit, equippedWeapon.range, ~shootingRaycastExludedLayer))
            {
                weaponVFX.CreateBulletExplosion(hit.point, hit.normal);
                weaponVFX.CreateBulletDecal(hit.point, hit.normal, hit.transform);
                
                if(hit.transform.gameObject.GetComponentInParent<EnemyHealth>() != null && !hit.collider.CompareTag("EnemyShield"))
                {
                    bool specialHit = hit.collider.transform.CompareTag("SpecialHit");
                    hit.transform.gameObject.GetComponentInParent<EnemyHealth>().Damage(equippedWeapon.damage, specialHit);
                }

                if(hit.collider.CompareTag("EnemyShield"))
                {
                    hit.transform.GetComponentInParent<EnemyHealth>().Damage((int)((float)equippedWeapon.damage * equippedWeapon.shieldPenetrationRate), false);
                }
            }
        }
        else
        {
            int pellets = 7+(int)InventoryManager.instance.equippedWeapon.shieldPenetrationRate;
            for(int i = 0; i < pellets; i++)
            {
                Vector3 orgRotation = raycastPivot.localEulerAngles;
                float upDownRecoil = InventoryManager.instance.equippedWeapon.upDownRecoil;
                float leftRightRecoilRange = InventoryManager.instance.equippedWeapon.leftRightRecoilRange;
                raycastPivot.localEulerAngles = new(Random.Range(-upDownRecoil,upDownRecoil),Random.Range(-leftRightRecoilRange, leftRightRecoilRange),raycastPivot.localEulerAngles.z);
                
                // Vector3 bulletForward = new(raycastPivot.forward.x + Random.Range(-0.1f,0.1f),raycastPivot.forward.y + Random.Range(-0.1f,0.1f),raycastPivot.forward.z);
                weaponVFX.CreateBulletLine(raycastPivot.forward);
                if (Physics.Raycast(raycastPivot.position, raycastPivot.forward, out RaycastHit hit, equippedWeapon.range, ~shootingRaycastExludedLayer))
                {
                    weaponVFX.CreateBulletExplosion(hit.point, hit.normal);
                    weaponVFX.CreateBulletDecal(hit.point, hit.normal, hit.transform);
                    if(hit.transform.gameObject.GetComponentInParent<EnemyHealth>() != null && !hit.collider.CompareTag("EnemyShield"))
                    {
                        bool specialHit = hit.collider.transform.CompareTag("SpecialHit");
                        hit.transform.gameObject.GetComponentInParent<EnemyHealth>().Damage(equippedWeapon.damage, specialHit);
                    }

                    if(hit.collider.CompareTag("EnemyShield"))
                    {
                        hit.transform.GetComponentInParent<EnemyHealth>().Damage((int)((float)equippedWeapon.damage * equippedWeapon.shieldPenetrationRate), false);
                    }
                }
                raycastPivot.localEulerAngles = orgRotation;
            }
        }

        if (equippedWeapon.gunMode == InventoryWeaponData.GunMode.Single)
        {
            isShootRequested = false;
        }
        else { isShootRequested = !upgraderUI.IsUpgraderUIOpen(); }
    }

    void Reload()
    {
        if (!equippedWeapon.isReloading && equippedWeapon.ammoLeftInMagazine < equippedWeapon.maxAmountOfBulletsInMagazine)
        {
            equippedWeapon.isReloading = true;
            isShootRequested = false;
        }
    }

    void ReloadInputAction()  { Reload(); }

    void CalculateReloading()
    {
        equippedWeapon.reloadingTimeLeft -= Time.deltaTime;

        if (equippedWeapon.reloadingTimeLeft <= 0)
        {
            equippedWeapon.reloadingTimeLeft = equippedWeapon.reloadingTime;

            int needdedAmountOfBullets = equippedWeapon.maxAmountOfBulletsInMagazine - equippedWeapon.ammoLeftInMagazine;
            equippedWeapon.ammoLeftInMagazine += needdedAmountOfBullets;

            equippedWeapon.isReloading = false;
        }
    }

    void CalculateDistanceForCrosshair()
    {
        if (Physics.Raycast(raycastPivot.position, raycastPivot.forward, out RaycastHit hit, float.PositiveInfinity, crosshairRaycastLayer))
        {
            Vector3 hitPosition = new(hit.point.x, 1.5f, hit.point.z);
            crosshair.SetDistanceFromPlayerToSurface(Vector3.Distance(transform.position, hitPosition));
            //  Debug.Log(Vector3.Distance(transform.position, hitPosition));
            // Debug.Log(hit.transform.gameObject.name);
            // GetComponent<PlayerUtility>().SetCrosshairPosition(hitPosition);
        }
    }
    void ShootRequest()
    {
        /*
        if (equippedWeapon.gunMode == InventoryWeaponData.GunMode.Automatic)
        {
            if (InventoryManager.instance.equippedWeapon.isCooldownActive == false)
            {
                isShootRequested = true;
            }

        }
        else if (equippedWeapon.gunMode == InventoryWeaponData.GunMode.Burst)
        {
            if (InventoryManager.instance.equippedWeapon.isCooldownActive == false)
            {
                isShootRequested = true;
            }
        }
        else if (equippedWeapon.gunMode == InventoryWeaponData.GunMode.Single)
        {
            if (InventoryManager.instance.equippedWeapon.isCooldownActive == false)
            {
                isShootRequested = true;
            }
        }
        */
        isShootRequested = !upgraderUI.IsUpgraderUIOpen();
    }

    void CancelShootRequest()  { isShootRequested = false; }

    void EquipFirstWeapon()
    {
        InventoryManager.instance.EquipWeapon(1);
        weaponModelController.UpdateWeaponModel();
        weaponVFX.SwitchWeaponVFX();
    }

    void EquipSecondWeapon()
    {
        InventoryManager.instance.EquipWeapon(2);
        weaponModelController.UpdateWeaponModel();
        weaponVFX.SwitchWeaponVFX();
    }

    void EquipThirdWeapon()
    {
        InventoryManager.instance.EquipWeapon(3);
        weaponModelController.UpdateWeaponModel();
        weaponVFX.SwitchWeaponVFX();
    }

    void InitializeInputAsset()
    {
        InputSystem.actions.FindActionMap("Shooting").FindAction("Equip First Gun").performed += context => EquipFirstWeapon();
        InputSystem.actions.FindActionMap("Shooting").FindAction("Equip Second Gun").performed += contex => EquipSecondWeapon();
        InputSystem.actions.FindActionMap("Shooting").FindAction("Equip Third Gun").performed += contex => EquipThirdWeapon();

        InputSystem.actions.FindActionMap("Shooting").FindAction("Reload").performed += contex => ReloadInputAction();

        InputSystem.actions.FindActionMap("Shooting").FindAction("Shoot Request").performed += contex => ShootRequest();
        InputSystem.actions.FindActionMap("Shooting").FindAction("Shoot Request").canceled += contex => CancelShootRequest();
    }

    public bool IsShootRequested() { return isShootRequested; }
}
