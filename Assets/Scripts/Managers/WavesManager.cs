using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class WavesManager : MonoBehaviour
{
    public static WavesManager instance;

    int actualWave;

    public WaveProperties[] WavesProperties;

    EnemySpawner enemySpawner;

    float timer;
    bool isInterWaveTime;

    GameObject Upgrader;

    int numberOfEnemiesLeft;
    void Awake()
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
    void InitalizeManager()
    {
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        isInterWaveTime = true;
        timer = WavesProperties[actualWave].timeToPrepare;
        Upgrader = GameObject.FindWithTag("Upgrader");
        numberOfEnemiesLeft = 0; 
        InitializeInputAsset();
    }

    void Update()
    {
        if(isInterWaveTime)
        {
            if(timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                StartNextWave();
            }
        }
    }

    void SkipInterwave()
    {
        if (isInterWaveTime && instance.enabled) { StartNextWave(); }
    }

    void StartNextWave()
    {
        isInterWaveTime = false;
        Upgrader.SetActive(false);
        enemySpawner.SpawnWave(WavesProperties[actualWave]);
        actualWave++;
    }

    public bool IsInterwaveTime() => isInterWaveTime; 

    public float GetTimeLeftToNextWave() => timer; 

    public int GetActualWave() => actualWave; 

    public void EndWave()
    {
        ProgressionManager.instance.AddPoints(WavesProperties[actualWave-1].rewardAmount);
        isInterWaveTime = true;
        Upgrader.SetActive(true);
        timer = WavesProperties[actualWave].timeToPrepare;
    }

    public void RegisterEnemy() => numberOfEnemiesLeft++;

    public void UnRegisterEnemy()
    {
        numberOfEnemiesLeft--;
        if(numberOfEnemiesLeft <= 0)
        {
            EndWave();
        }
    }

    public int GetNumberOfEnemiesLeft() => numberOfEnemiesLeft;

    void InitializeInputAsset()
    {
        InputSystem.actions.FindActionMap("Interactions").FindAction("Skip Interwave").performed += contex => SkipInterwave();
    }
}
