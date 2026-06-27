using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityItemData", menuName = "Inventory/UtilityItemData")]
public class UtilityItemData : ScriptableObject
{
    [Header("Item Variables")]
    public string itemName;

    [Header("Model")]
    public GameObject toInstance;
    public Vector3 modelPosition;

    [Header("Utility Variables")]
    public float cooldown;
    public float range;

  /*  [Header("Up-Down Recoil")]
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
  */

}
