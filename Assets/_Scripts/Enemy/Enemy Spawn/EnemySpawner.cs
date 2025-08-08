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

    [Header("Parent Object")]
    public Transform enemyParent;

    private List<EnemyInstance> activeEnemies = new List<EnemyInstance>();
    private int currentPoints = 0;

    private void Start()
    {
        MonsterFactory.Instance.Init(enemyDataList);
        foreach (var data in enemyDataList)
        {
            GameObjectPoolManager.Instance.CreatePool(data.id, data.prefab, data.initialPoolSize);
        }

        
    }

    private void Update()
    {
        SpawnCheck();
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

        var spawnPlan = EnemySpawnOption.GetOptimalCombination(enemyDataList, remainingCount, remainingPoints);
        foreach (var plan in spawnPlan)
        {
            for (int i = 0; i < plan.count; i++)
            {
                SpawnEnemy(plan.data);
            }
        }
    }

    private bool SpawnEnemy(EnemyData data)
    {
        var point = GetRandomActiveSpawnPoint();
        if (point == null) return false;

        GameObject enemyGO = MonsterFactory.Instance.SpawnEnemy(data, point.transform.position, Quaternion.identity);
        if (enemyGO == null) return false;

        enemyGO.transform.SetParent(enemyParent);
        var instance = enemyGO.GetComponent<EnemyInstance>();
        if (instance == null)
            instance = enemyGO.AddComponent<EnemyInstance>();

        instance.poolKey = data.id;
        instance.onDeath = OnEnemyDeath;

        var ai = enemyGO.GetComponent<MonsterAI>();
        if (ai != null)
        {
            ai.enemyData = data;
            ai.isDead = false;
        }
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


