using UnityEngine;
using System.Collections.Generic;

public class MonsterFactory : MonoBehaviour, IMonsterFactory
{
    private readonly Dictionary<string, MonsterPool> _pools = new();
    public static MonsterFactory Instance { get; private set; }

    public Transform monsterContainer;
    public EnemySpawner ES;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Init(List<EnemyData> enemyDataList)
    {
        foreach (var data in enemyDataList)
        {
            _pools[data.id] = new MonsterPool(data.prefab, data.initialPoolSize, monsterContainer);
        }
    }

    public GameObject SpawnEnemy(EnemyData data, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(data.id))
        {
            Debug.LogError($"Pool not found for enemy: {data.id}");
            return null;
        }
        Debug.Log("Đã spawn ra enemy mới!");
        return _pools[data.id].Get(position, rotation);
    }

    public void ReturnEnemy(EnemyData data, GameObject enemy)
    {
        if (!_pools.ContainsKey(data.id))
        {
            Debug.LogWarning($"Trying to return enemy that doesn't have a pool: {data.id}");
            Destroy(enemy);
            return;
        }
        ES.currentPoints -= data.point;
        Debug.Log("Enemy đã chết!");
        _pools[data.id].Return(enemy);
    }
}
