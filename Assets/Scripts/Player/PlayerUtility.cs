using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

public class PlayerUtility : MonoBehaviour
{
    public Vector3 InstancingStartingPoint;
    bool utilityRequested;
    private InventoryUtilityData equippedUtility;
    [SaveDuringPlay]
    public AnimationCurve ThrowForceCurve;
    public float throwForce;

    void Start()
    {
        InitializeInputAsset();
    }

    void Update()
    {
        equippedUtility = InventoryManager.instance.equippedUtility;

        CalculateUtilityCooldown();

        if(utilityRequested && !equippedUtility.isCooldownActive)
        {
            UseUtility();
        }
    }

    void CalculateUtilityCooldown()
    {
        if (equippedUtility != null && equippedUtility.isCooldownActive == true)
        {
            equippedUtility.cooldownLeft -= Time.deltaTime;
            if (equippedUtility.cooldownLeft <= 0)
            {
                equippedUtility.cooldownLeft = equippedUtility.cooldown;
                equippedUtility.isCooldownActive = false;
            }
        }
    }

    void UseUtility()
    {
        equippedUtility.isCooldownActive = true;
        equippedUtility.quantity -= 1;

        // Vector3 forward = GameObject.Find("CursorGameObject").transform.position - transform.TransformPoint(InstancingStartingPoint);
        GameObject utility = Instantiate(equippedUtility.toInstance, transform.TransformPoint(InstancingStartingPoint), Quaternion.identity);//Quaternion.LookRotation(forward.normalized,Vector3.up));
        utility.GetComponent<Rigidbody>().AddExplosionForce(utility.GetComponent<Rigidbody>().mass * ThrowForceCurve.Evaluate(Vector3.Distance(transform.position, GameObject.Find("CursorGameObject").transform.position)/50) * throwForce,transform.TransformPoint(new Vector3(InstancingStartingPoint.x,InstancingStartingPoint.y,InstancingStartingPoint.z -0.5f)), 10f,0.5f,ForceMode.Impulse);
        Debug.Log(Vector3.Distance(transform.position, GameObject.Find("CursorGameObject").transform.position) / 50);
      //  Mathf.Sqrt(Vector3.Distance(transform.position, GameObject.Find("CursorGameObject").transform.position))
        InventoryManager.instance.UseUtility();
        utilityRequested = false;
    }

    void UtilityRequest()
    {
        if (equippedUtility.quantity > 0)
        {
            utilityRequested = true;
        }
    }

    void CancelUtilityRequest()
    {
        utilityRequested = false;
    }

    void EquipDecoy()
    {
        InventoryManager.instance.EquipUtility(4);
    }

    void EquipGrenade()
    {
        InventoryManager.instance.EquipUtility(5);
    }

   // public void SetCrosshairPosition(Vector3 hitPosition)
   // {
 //       crosshairPosition = hitPosition;
 //   }

    void InitializeInputAsset()
    {
        InputSystem.actions.FindActionMap("Shooting").FindAction("Utility Request").performed += contex => UtilityRequest();
        InputSystem.actions.FindActionMap("Shooting").FindAction("Utility Request").canceled += contex => CancelUtilityRequest();

        InputSystem.actions.FindActionMap("Shooting").FindAction("Equip Decoy").performed += context => EquipDecoy();
        InputSystem.actions.FindActionMap("Shooting").FindAction("Equip Grenade").performed += contex => EquipGrenade();
    }
}
