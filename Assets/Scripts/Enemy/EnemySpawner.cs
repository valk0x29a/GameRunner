using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    Transform player;

    public float xSpawnBounds;
    public float zSpawnBounds;

    public float minSpawnRange;
    public float maxSpawnRange;
    public float spawnY;

    public GameObject[] enemies;
    public GameObject[] bosses;

    public bool isEndlees;
    public float endlessModeTimer;

    float timerLeft;
    void InitializeSpawner()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if(isEndlees)
        {
            timerLeft = endlessModeTimer;
        }
    }

    void Start()
    {
        InitializeSpawner();
    }

    void Update()
    {
        if (isEndlees)
        {
            timerLeft -= Time.deltaTime;
            if(timerLeft <= 0)
            {
                timerLeft = endlessModeTimer;
                SpawnEnemy(Random.Range(0, enemies.Length));
            }
        }
    }

    public void SpawnWave(WaveProperties waveProperties)
    {
        if(waveProperties.isBossWave) { SpawnBoss(); return; }
        for(int i = 0; i < waveProperties.normalZombies;i++)
        {
            SpawnEnemy(0);
        }
        for (int i = 0; i < waveProperties.speedyZombies; i++)
        {
            SpawnEnemy(1);
        }
        for(int i = 0; i < waveProperties.dashyZombies; i++)
        {
            SpawnEnemy(2);
        }
        for (int i = 0; i < waveProperties.strongZombies; i++)
        {
            SpawnEnemy(3);
        }
    }

    void SpawnBoss()
    {
        int bossIndex = Random.Range(0,bosses.Length);
        GameObject boss = Instantiate(bosses[bossIndex]);
        boss.transform.position = new(0,spawnY,0);

        WavesManager.instance.RegisterEnemy();
    }

    void SpawnEnemy(int enemyIndex)
    {
        GameObject enemy = Instantiate(enemies[enemyIndex]);
        enemy.transform.position = CalculateSpawnPosition();

        enemy.transform.LookAt(player);
        WavesManager.instance.RegisterEnemy();
    }

    Vector3 CalculateSpawnPosition()
    { 
        float x = Random.Range(-xSpawnBounds,xSpawnBounds);;
        Vector3 localSpawnPosition = new(x,spawnY,randomSpawnRangeZ(x));
        if(localSpawnPosition.x == 0 && localSpawnPosition.z == 0) { Debug.Log("It happened"); }
        return localSpawnPosition;
    }

    float randomSpawnRangeZ(float x)
    {
        if(x < (player.transform.position.x + minSpawnRange) && x > (player.transform.position.x - minSpawnRange))
        {
            float enemyZP = Random.Range(zSpawnBounds, player.transform.position.z + minSpawnRange);
            float enemyZN = Random.Range(-zSpawnBounds, player.transform.position.z - minSpawnRange);
            if (enemyZP > zSpawnBounds) { enemyZP = enemyZN; }
            if (enemyZN < -zSpawnBounds) { enemyZN = enemyZP; }
            if (Random.Range(0, 100) > 50) { return enemyZP; } else { return enemyZN; }
        }
        return Random.Range(-zSpawnBounds,zSpawnBounds);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < 1000; i++)
            {
               // Gizmos.DrawSphere(CalculateSpawnPosition(), 1f);
            }
        }
    }
}
