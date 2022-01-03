using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Tile
{
    [SerializeField] EnemyController cubeEnemyprefab;
    [SerializeField] EnemyController prismEnemyPrefab;
    [SerializeField] Transform enemiesHolder;
    int lives = 3;
    int currentLevel = 0;
    Vector3 spawnPoint;
    Vector3 orientation;
    float levelSpawnDelay;
    float timeLeftToSpawn;
    int noOfCubeEnemiesLeft = 0;
    int noOfPrismEnemiesLeft = 0;
    bool isSimulating = true;

    void Start()=> LevelUp(0);
    
    void Update() 
    {
        if(!isSimulating)
            return;
        timeLeftToSpawn -= Time.deltaTime;
        if(timeLeftToSpawn <= 0)
        {
            timeLeftToSpawn = levelSpawnDelay;
            int choice = Random.Range(0, noOfPrismEnemiesLeft + noOfCubeEnemiesLeft);
            if(choice < noOfPrismEnemiesLeft)
            {
                noOfPrismEnemiesLeft --;
                Instantiate(prismEnemyPrefab, spawnPoint, Quaternion.LookRotation(orientation, Vector3.down), enemiesHolder);
            }
            else
            {
                noOfCubeEnemiesLeft--;
                Instantiate(cubeEnemyprefab, spawnPoint, Quaternion.LookRotation(orientation, Vector3.down), enemiesHolder);
            }
        }

        if(noOfCubeEnemiesLeft + noOfPrismEnemiesLeft == 0)
            LevelUp(currentLevel+1);
    }

    public void Setup(Vector3 spawnPoint, Vector2 orientation)
    {
        this.spawnPoint = spawnPoint;
        this.orientation = orientation;
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

    void LoseLife()
    {
        lives--;
        isSimulating = false;
    }
}
