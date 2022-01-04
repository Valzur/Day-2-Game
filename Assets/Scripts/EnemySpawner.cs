using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Tile
{
    [SerializeField] EnemyController cubeEnemyprefab;
    [SerializeField] EnemyController prismEnemyPrefab;
    [SerializeField] Transform enemiesHolderPrefab;
    List<Enemy> enemies = new List<Enemy>();
    Transform enemiesHolder;
    Vector2Int[] path;
    int currentLevel = 0;
    Vector3 spawnPoint;
    Vector3 orientation;
    float levelSpawnDelay;
    float timeLeftToSpawn;
    int noOfCubeEnemiesLeft = 0;
    int noOfPrismEnemiesLeft = 0;
    bool isSimulating = true;

    void Start()
    {
        LevelUp(0);
        spawnPoint = transform.position;
        spawnPoint.z  = (float)-0.5;
        enemiesHolder = Instantiate(enemiesHolderPrefab, Vector3.zero, Quaternion.identity);
    } 
    
    void Update() 
    {
        if(!isSimulating)
            return;
        timeLeftToSpawn -= Time.deltaTime;
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
            enemies.Add(enemy);
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
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.RemoveRange(0, enemies.Count);
    }

    public void RemoveMe(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
