using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [Header("Spawner Control")]
    public bool spawnerEnabled = true;
    public Transform player;
    [Tooltip("Chỉ bật spawn point trong bán kính này quanh player (theo X,Z)")]
    public float spawnRadius = 39.2f;

    private List<EnemyInstance> activeEnemies = new List<EnemyInstance>();
    public int currentPoints = 0;

    private void Start()
    {
        MonsterFactory.Instance.Init(enemyDataList);
        foreach (var data in enemyDataList)
        {
            GameObjectPoolManager.Instance.CreatePool(data.id, data.prefab, data.initialPoolSize);
        }
        StartCoroutine(FindPlayerByTag("Player"));
        StartCoroutine(CheckSpawnRoutine(2f));
    }

    private void Update()
    {
        if (!spawnerEnabled || player == null) return;
        UpdateSpawnPointsByDistance(spawnRadius);
    }
    private IEnumerator FindPlayerByTag(string tag)
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(tag);
            if (playerObj != null)
            {
                player = playerObj.transform;
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public Transform GetPlayerTransform()
    {
        return player;
    }

    IEnumerator CheckSpawnRoutine(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            SpawnCheck();
        }
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

    private void UpdateSpawnPointsByDistance(float radius)
    {
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);

        foreach (var point in spawnPoints)
        {
            Vector2 pointPos = new Vector2(point.transform.position.x, point.transform.position.z);
            float dist = Vector2.Distance(playerPos, pointPos);

            point.gameObject.SetActive(dist <= radius);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
#endif
}
