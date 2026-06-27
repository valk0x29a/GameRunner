using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVFX : MonoBehaviour
{
    public GameObject bulletExplosionPrefab;
    public GameObject bulletLinePrefab;
    public GameObject bulletDecalPivot;

    GameObject bulletLineStartPositionGO;
    GameObject weaponFlashLightGO;

    float flashTimer;
    bool isFlashActive;

    Light weaponLight;

    void Update()
    {
        if (isFlashActive)
        {
            flashTimer -= Time.deltaTime;
            if(flashTimer <= 0)
            {
                weaponLight.intensity = 0f;
            }
        }
    }
    public void CreateWeaponFlash()
    {
        isFlashActive = false;
        weaponLight = weaponFlashLightGO.GetComponent<Light>();
        weaponLight.intensity = 2f;

        flashTimer = 0.05f;
        isFlashActive = true;
    }

    public void CreateBulletExplosion(Vector3 position, Vector3 forward)
    {
        GameObject bulletExplosion = Instantiate(bulletExplosionPrefab, position, Quaternion.identity);
        bulletExplosion.transform.forward = forward;
        Destroy(bulletExplosion.transform.Find("BulletExplosionLight").gameObject, 0.1f);


        bulletExplosion.GetComponent<ParticleSystem>().Play();
        Destroy(bulletExplosion, 1f);
    }

    public void CreateBulletLine( Vector3 forward)
    {
        GameObject bulletLine = Instantiate(bulletLinePrefab, bulletLineStartPositionGO.transform.position, Quaternion.identity);
        bulletLine.transform.forward = forward;

        Destroy(bulletLine, 0.05f);
    }
    public void CreateBulletDecal(Vector3 position, Vector3 hitNormal, Transform parent)
    {
        GameObject bulletDecalGO = Instantiate(bulletDecalPivot, position, Quaternion.FromToRotation(Vector3.up, hitNormal));

        if (hitNormal.z != 0)
        {
            Vector3 bulletDecalEulerAngels = bulletDecalGO.transform.localEulerAngles;
            bulletDecalGO.transform.localEulerAngles = new Vector3(bulletDecalEulerAngels.x - 90f, bulletDecalEulerAngels.y, bulletDecalEulerAngels.z);
        }
        else if (hitNormal.x == 0)
        {
            Vector3 bulletDecalEulerAngels = bulletDecalGO.transform.localEulerAngles;
            bulletDecalGO.transform.localEulerAngles = new Vector3(bulletDecalEulerAngels.x + 90f, bulletDecalEulerAngels.y, bulletDecalEulerAngels.z);
        }
        else
        {
            Vector3 bulletDecalEulerAngels = bulletDecalGO.transform.localEulerAngles;
            bulletDecalGO.transform.localEulerAngles = new Vector3(bulletDecalEulerAngels.x, bulletDecalEulerAngels.y + 90f, bulletDecalEulerAngels.z);
        }
        bulletDecalGO.transform.SetParent(parent);
        Destroy(bulletDecalGO, 20f);
    }

    public void SetBulletLineStartPositionGO(GameObject GO)
    {
        bulletLineStartPositionGO = GO;
    }
    public void SetWeaponFlashLightGO(GameObject GO)
    {
        weaponFlashLightGO = GO;
    }
    public void SwitchWeaponVFX()
    {
        isFlashActive = false;
        flashTimer = 0.0f;
    }
}
