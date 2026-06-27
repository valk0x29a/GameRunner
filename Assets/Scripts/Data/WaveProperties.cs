using System;
using UnityEngine;

[Serializable]
public class WaveProperties
{
    [Header("Wave Zombie Settings")]
    public bool isBossWave;
    public int normalZombies;
    public int speedyZombies;
    public int dashyZombies;
    public int strongZombies;

    [Header("Other Settings")]
    public float timeToPrepare;
    public int rewardAmount;
}
