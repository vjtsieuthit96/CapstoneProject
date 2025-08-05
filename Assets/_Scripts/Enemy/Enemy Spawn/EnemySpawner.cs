using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<EnemyData> enemyDataList;

    [Header("Spawn Points")]
    public List<SpawnPoint> spawnPoints;

    [Header("Level Settings")]
    public List<LevelConfig> levelConfigs;

    [Header("Runtime State")]
    public int currentLevel = 1;

    private List<EnemyInstance> activeEnemies = new List<EnemyInstance>();
    private int currentPoints = 0;

    private void Start()
    {
        foreach (var data in enemyDataList)
        {
            GameObjectPoolManager.Instance.CreatePool(data.id, data.prefab, data.initialPoolSize);
        }
        InvokeRepeating(nameof(SpawnCheck), 0f, 1f);
    }

    private void SpawnCheck()
    {
        var config = levelConfigs.FirstOrDefault(l => l.level == currentLevel);
        if (config == null) return;

        activeEnemies.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        int currentCount = activeEnemies.Count;
        int remainingCount = config.maxEnemyCount - currentCount;
        int remainingPoints = config.totalPoints - currentPoints;

        if (remainingCount <= 0 || remainingPoints <= 0) return;

        var validEnemies = enemyDataList
            .Where(e => e.point <= remainingPoints)
            .OrderBy(_ => Random.value)
            .ToList();

        foreach (var enemy in validEnemies)
        {
            if (remainingCount <= 0 || remainingPoints < enemy.point)
                break;

            if (SpawnEnemy(enemy))
            {
                remainingCount--;
                remainingPoints -= enemy.point;
            }
        }
    }

    private bool SpawnEnemy(EnemyData data)
    {
        var point = GetRandomActiveSpawnPoint();
        if (point == null) return false;

        GameObject enemyGO = GameObjectPoolManager.Instance.GetObject(data.id, point.transform.position, Quaternion.identity);
        if (enemyGO == null) return false;

        var instance = enemyGO.GetComponent<EnemyInstance>();
        if (instance == null)
            instance = enemyGO.AddComponent<EnemyInstance>();

        instance.poolKey = data.id;
        instance.onDeath = OnEnemyDeath;

        activeEnemies.Add(instance);
        currentPoints += data.point;

        return true;
    }

    private void OnEnemyDeath(EnemyInstance instance)
    {
        activeEnemies.Remove(instance);
        var data = enemyDataList.FirstOrDefault(e => e.id == instance.poolKey);
        if (data != null)
            currentPoints -= data.point;
    }

    private SpawnPoint GetRandomActiveSpawnPoint()
    {
        var active = spawnPoints.Where(p => p.IsActive).ToList();
        if (active.Count == 0) return null;
        return active[Random.Range(0, active.Count)];
    }
}
