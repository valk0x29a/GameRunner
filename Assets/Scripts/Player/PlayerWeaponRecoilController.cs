using UnityEngine;

public class PlayerWeaponRecoilController : MonoBehaviour
{

    [Header("Assign Components")]
    public Transform playerRecoilCameraPivot;

    [Header("Recoil System Debug")]
    bool isFiring;

    [Header("Recoil Debug")]
    float upDownRecoil;

    
    float upDownRecoilLerpSpeed;
    float upDownRecoilReturnLerpSpeed;

    float leftRightRecoilRange;

    float leftRightRecoilLerpSpeed;
    float leftRightRecoilReturnLerpSpeed;

    float outputCameraX;
    float outputCameraY;

    float actualLerpX;
    float actualLerpY;

    //float defaultX;

    InventoryWeaponData equippedWeapon;

    bool isSprinting;
    bool isMoving;

    void Start()
    {
      //  defaultX = GetCameraRecoilPivotX();
    }
    void LateUpdate()
    {
        if (isFiring)
        {
            float cameraX = GetCameraRecoilPivotX();
            float cameraY = playerRecoilCameraPivot.localEulerAngles.y;
            
            float x = Mathf.LerpAngle(cameraX, outputCameraX, upDownRecoilLerpSpeed * Time.deltaTime);
            float y = Mathf.LerpAngle(cameraY, outputCameraY, leftRightRecoilLerpSpeed * Time.deltaTime);
            SetCameraRecoilPivotX(x);
            SetCameraRecoilPivotY(y);
      
            actualLerpX += upDownRecoilLerpSpeed * Time.deltaTime;
            actualLerpY += leftRightRecoilLerpSpeed * Time.deltaTime;

            if((int)actualLerpX >= 1 && (int)actualLerpY >= 1)
            {
                isFiring = false;
                actualLerpX = 0;
                actualLerpY = 0;
            }
    
        }
        else if (!isFiring)
        {
            float cameraX = playerRecoilCameraPivot.localEulerAngles.x;
            float cameraY = playerRecoilCameraPivot.localEulerAngles.y;
            if (cameraX != 0)
            {
                float x = Mathf.LerpAngle(cameraX, 0, upDownRecoilReturnLerpSpeed * Time.deltaTime);
                Vector3 cameraRotation = playerRecoilCameraPivot.localEulerAngles;
                playerRecoilCameraPivot.localEulerAngles = new Vector3(x, cameraRotation.y, cameraRotation.z);

            }
            if (cameraY != 0)
            {
                float y = Mathf.LerpAngle(cameraY, 0, leftRightRecoilReturnLerpSpeed * Time.deltaTime);
                Vector3 cameraRotation = playerRecoilCameraPivot.localEulerAngles;
                playerRecoilCameraPivot.localEulerAngles = new Vector3(cameraRotation.x, y, cameraRotation.z);
            }
        }
    }

    public void FireWeapon()
    {
        equippedWeapon = InventoryManager.instance.equippedWeapon;
        if (isMoving)
        {
            upDownRecoil = isSprinting ? equippedWeapon.sprintUpDownRecoil : equippedWeapon.upDownRecoil;
            leftRightRecoilRange = isSprinting ? equippedWeapon.sprintLeftRightRecoilRange : equippedWeapon.leftRightRecoilRange;
        }
        else
        {
            upDownRecoil = equippedWeapon.notMovingUpDownRecoil;
            leftRightRecoilRange = equippedWeapon.notMovingLeftRightRecoilRange;
        }

        upDownRecoilLerpSpeed = equippedWeapon.upDownRecoilLerpSpeed;
        upDownRecoilReturnLerpSpeed = equippedWeapon.upDownRecoilReturnLerpSpeed;

        leftRightRecoilLerpSpeed = equippedWeapon.leftRightRecoilLerpSpeed;
        leftRightRecoilReturnLerpSpeed = equippedWeapon.leftRightRecoilReturnLerpSpeed;

        isFiring = true;

        float randomYRecoil = Random.Range(-leftRightRecoilRange, leftRightRecoilRange);
        outputCameraX = Random.Range(-upDownRecoil,upDownRecoil);
        outputCameraY = randomYRecoil;
    }


    void SetCameraRecoilPivotX(float x)
    {
        Vector3 cameraRotation = playerRecoilCameraPivot.localEulerAngles;
        playerRecoilCameraPivot.localEulerAngles = new Vector3(-x, cameraRotation.y, cameraRotation.z);
    }

    float GetCameraRecoilPivotX() => -playerRecoilCameraPivot.localEulerAngles.x;

    void SetCameraRecoilPivotY(float y)
    {
        Vector3 cameraRotation = playerRecoilCameraPivot.localEulerAngles;
        playerRecoilCameraPivot.localEulerAngles = new Vector3(cameraRotation.x, y, cameraRotation.z);
    }

    public float GetOutputCameraX() { return outputCameraX; }
    public float GetOutputCameraY() { return outputCameraY; }

    public void Walk()
    {
        isSprinting = false;
    }
    public void Sprint()
    {
        isSprinting = true;
    }
    public void StopMoving()
    {
        isMoving = false;
    }

    public bool IsFiring() { return isFiring; }

    public void StartMoving()
    {
        isMoving = true;
    }
}
