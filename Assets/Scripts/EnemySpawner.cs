using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Tile
{
    [SerializeField] EnemyController cubeEnemyprefab;
    [SerializeField] EnemyController prismEnemyPrefab;
    [SerializeField] Transform enemiesHolderPrefab;
    Transform enemiesHolder;
    Vector2Int[] path;
    int currentLevel = 0;
    Vector3 spawnPoint;
    Vector3 orientation;
    float levelSpawnDelay;
    float timeLeftToSpawn = 10f;
    int noOfCubeEnemiesLeft = 0;
    int noOfPrismEnemiesLeft = 0;
    [HideInInspector] public float totalTime = 0;
    bool isSimulating = false;

    void Start()
    {
        LevelUp(0);
        isSimulating = true;
        spawnPoint = transform.position;
        spawnPoint.z  = (float)-0.5;
        if(!enemiesHolder)
            enemiesHolder = Instantiate(enemiesHolderPrefab, Vector3.zero, Quaternion.identity);
    } 
    
    void Update() 
    {
        if(!isSimulating)
            return;
        totalTime += Time.deltaTime;
        timeLeftToSpawn -= Time.deltaTime;
        GameManager.Instance.uIManager.gamePanel.UpdateTotalTime(Mathf.FloorToInt(totalTime));
        GameManager.Instance.uIManager.gamePanel.UpdateTimeTillNextEnemy(Mathf.FloorToInt(timeLeftToSpawn));
        if(timeLeftToSpawn <= 0)
        {
            timeLeftToSpawn = levelSpawnDelay;
            int choice = Random.Range(0, noOfPrismEnemiesLeft + noOfCubeEnemiesLeft);
            EnemyController enemy;
            if(choice < noOfPrismEnemiesLeft)
            {
                noOfPrismEnemiesLeft --;
                enemy = Instantiate(prismEnemyPrefab, spawnPoint, Quaternion.LookRotation(orientation, Vector3.back), enemiesHolder);
            }
            else
            {
                noOfCubeEnemiesLeft--;
                enemy = Instantiate(cubeEnemyprefab, spawnPoint, Quaternion.LookRotation(orientation, Vector3.back), enemiesHolder);
            }
            enemy.Setup(path);
        }

        if(noOfCubeEnemiesLeft + noOfPrismEnemiesLeft == 0)
            LevelUp(currentLevel+1);
    }

    public void Setup(Vector2Int[] path)
    {
        this.path = path;
        orientation = Utility.GetVector3(path[1] - path[0]);
    }

    void LevelUp(int nextLevel)
    {
        currentLevel = nextLevel;
        levelSpawnDelay = GetSpawnDelay(nextLevel);
        noOfCubeEnemiesLeft = GetNoOfCubeEnemies(nextLevel);
        noOfPrismEnemiesLeft = GetNoOfPrismEnemies(nextLevel);
    }

    float GetSpawnDelay(int level)
    {
        return 1+2*Mathf.Exp(-(float)0.1 * level);
    }

    int GetNoOfCubeEnemies(int level)
    {
        return Mathf.Max(0, Mathf.FloorToInt(-10 + level - Mathf.Exp(-(float)0.5 * level)));
    }

    int GetNoOfPrismEnemies(int level)
    {
        return Mathf.FloorToInt(3 + level - Mathf.Exp(-(float)0.5 * level));
    }

    public void StopSimulating()
    {
        isSimulating = false;
        foreach (var enemy in Enemy.AllEnemies)
        {
            Destroy(enemy.gameObject);
        }
        Enemy.AllEnemies.RemoveRange(0, Enemy.AllEnemies.Count);
    }
}
