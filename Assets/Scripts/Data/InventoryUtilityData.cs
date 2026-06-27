using System;
using UnityEngine;
[Serializable]
public class InventoryUtilityData
{
    public string itemName;
    public int quantity;

    public GameObject toInstance;
    public Vector3 modelPosition;

    public float cooldown;
    public float cooldownLeft;

    public bool isCooldownActive;

    public float range;

   /* public float upDownRecoil;
    public float notMovingUpDownRecoil;
    public float sprintUpDownRecoil;
    public float upDownRecoilLerpSpeed;
    public float upDownRecoilReturnLerpSpeed;

    public float leftRightRecoilRange;
    public float notMovingLeftRightRecoilRange;
    public float sprintLeftRightRecoilRange;
    public float leftRightRecoilLerpSpeed;
    public float leftRightRecoilReturnLerpSpeed;
   */
}
