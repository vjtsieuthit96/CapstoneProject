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
            return null;
        }
        return _pools[data.id].Get(position, rotation);
    }

    public void ReturnEnemy(EnemyData data, GameObject enemy)
    {
        if (!_pools.ContainsKey(data.id))
        {
            Destroy(enemy);
            return;
        }
        ES.currentPoints -= data.point;
        _pools[data.id].Return(enemy);
    }
}
