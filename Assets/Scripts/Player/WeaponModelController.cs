using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelController : MonoBehaviour
{
    GameObject actualWeaponModel;
    Vector3 weaponModelPosition;

    WeaponVFX weaponVFX;

    PlayerAudio playerAudio;

    private void Start()
    {
        weaponVFX = GetComponent<WeaponVFX>();
        playerAudio = GetComponent<PlayerAudio>();
        UpdateWeaponModel();
    }

    public void UpdateWeaponModel()
    {
        if (actualWeaponModel != null)
        {
            Destroy(actualWeaponModel);
        }

        actualWeaponModel = Instantiate(InventoryManager.instance.equippedWeapon.model);
        actualWeaponModel.transform.SetParent(transform);
        weaponModelPosition = InventoryManager.instance.equippedWeapon.modelPosition;
        actualWeaponModel.transform.SetLocalPositionAndRotation(weaponModelPosition, Quaternion.identity);

        weaponVFX.SetWeaponFlashLightGO(actualWeaponModel.transform.Find("Explosion Light").gameObject);
        weaponVFX.SetBulletLineStartPositionGO(actualWeaponModel.transform.Find("Bullet Start Position").gameObject);
        playerAudio.SetPlayerGunAudioSource(actualWeaponModel.transform.Find("Weapon Audio Source").gameObject.GetComponent<AudioSource>());
    }

    public GameObject GetActualWeaponModel() => actualWeaponModel;
}
