using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance;
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
    void InitalizeManager() { numberOfEnemiesLeft = 0; }

    public void RegisterEnemy()
    {
        numberOfEnemiesLeft++;
    }

    public void UnregisterEnemy()
    {
        numberOfEnemiesLeft--;
        if(numberOfEnemiesLeft <= 0)
        {
            WavesManager.instance.EndWave();
        }
    }

    public int GetNumberOfEnemiesLeft()
    {
        return numberOfEnemiesLeft;
    }
}
