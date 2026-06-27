using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrosshairController : MonoBehaviour
{
    public PlayerWeaponRecoilController playerRecoil;
    public PlayerShooting playerShooting;

    public RectTransform crosshairTop;
    public RectTransform crosshairBottom;
    public RectTransform crosshairLeft;
    public RectTransform crosshairRight;

    [DebugVariable]
    bool isMoving;
    bool isSprinting;

    float crosshairDistanceDestination;
    float normalCrosshairDistance;

    public float crosshairRecoilMultiplier;

    public float distanceCrosshairCalibration;

    public float crosshairLerpSpeed;
   // public float crosshairReturnLerpSpeed;

    public float minCrosshairDistance;

    float distance;
    float distanceFromPlayerToSurface;

    public bool crosshairBasedCalculations;
    GameObject cursor3d;

  //  float lerpSpeed;
    void Start()
    {
        normalCrosshairDistance = crosshairTop.localPosition.y;
        UpdateCrosshairDistanceDestination();
        if(crosshairBasedCalculations) { cursor3d = GameObject.Find("CursorGameObject"); }
    }


    void Update()
    {
        if(crosshairBasedCalculations)
        {
            if(cursor3d == null) { cursor3d = GameObject.Find("CursorGameObject"); }
            distanceFromPlayerToSurface = Vector3.Distance(transform.position, cursor3d.transform.position);
        }
        if(crosshairTop.localPosition.y != crosshairDistanceDestination)
        {
            distance = Mathf.Lerp(crosshairTop.localPosition.y, crosshairDistanceDestination, crosshairLerpSpeed);

            crosshairTop.localPosition = new Vector2(0, distance);
            crosshairBottom.localPosition = new Vector2(0, -distance);
            crosshairLeft.localPosition = new Vector2( -distance,0);
            crosshairRight.localPosition = new Vector2(distance,0);
        }
    }

    void UpdateCrosshairDistanceDestination()
    {
        InventoryWeaponData equippedWeapon;
        equippedWeapon = InventoryManager.instance.equippedWeapon;

        float leftRightRecoil = 0;
        float upDownRecoil = 0;

            if (!isMoving)
            {
                leftRightRecoil = equippedWeapon.notMovingLeftRightRecoilRange;
                upDownRecoil = equippedWeapon.notMovingUpDownRecoil;
            }
            else if (!isSprinting)

            {
                leftRightRecoil = equippedWeapon.leftRightRecoilRange;
                upDownRecoil = equippedWeapon.upDownRecoil;
            }
            else
            {
                leftRightRecoil = equippedWeapon.sprintLeftRightRecoilRange;
                upDownRecoil = equippedWeapon.sprintUpDownRecoil;
            }

        float recoil = 0;

        recoil = leftRightRecoil >= upDownRecoil ? leftRightRecoil : upDownRecoil;

       
        crosshairDistanceDestination = calculateCrosshairDistanceDestination(recoil);
    }

    float calculateCrosshairDistanceDestination(float recoil)
    {
        float distanceMultiplayer = distanceFromPlayerToSurface / distanceCrosshairCalibration;
       //Debug.Log(recoil);
        float distance = (normalCrosshairDistance + (recoil * crosshairRecoilMultiplier)) * distanceMultiplayer;
        if(distance < minCrosshairDistance)
        {
            distance = minCrosshairDistance;
        }
        return distance;
    }

    public void SetDistanceFromPlayerToSurface(float distance)
    {
        if (!crosshairBasedCalculations) { distanceFromPlayerToSurface = distance; }
        UpdateCrosshairDistanceDestination();
    }

    public void StartMoving()
    {
        isMoving = true;
        UpdateCrosshairDistanceDestination();
    }
    public void StopMoving()
    {
        isMoving = false;
        UpdateCrosshairDistanceDestination();
    }

    public void Walk()
    {
        isSprinting = false;
        UpdateCrosshairDistanceDestination();
    }

    public void Sprint()
    {
        isSprinting = true;
        UpdateCrosshairDistanceDestination();
    }

    public void SetCrosshairVisibility(bool visibility)
    {
        crosshairTop.parent.gameObject.SetActive(visibility);
    }
}
