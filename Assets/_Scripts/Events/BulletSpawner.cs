using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public List<SpawnPoint> spawnPoints;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform bulletParent;

    [Header("Spawner Control")]
    public Transform player;
    public bool spawnerEnabled = true;
    public float spawnInterval = 20f;
    public int bulletPerSpawn = 1;

    [Header("Task Prefabs")]
    public List<GameObject> taskPrefabs;
    public Transform taskParent;

    [Header("Spawner Control")]
    public bool TaskspawnerEnabled = true;
    public float TaskspawnInterval = 1f;

    private void Start()
    {
        if (player == null)
        {
            StartCoroutine(FindPlayerByTag("Player"));
        }

        StartCoroutine(SpawnRoutine());
        StartCoroutine(TaskSpawnRoutine());

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

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (!spawnerEnabled || player == null) continue;

            var point = GetNearestSpawnPoint();
            if (point != null)
            {
                for (int i = 0; i < bulletPerSpawn; i++)
                {
                    SpawnBullet(point.transform.position);
                }
            }
        }
    }

    private void SpawnBullet(Vector3 position)
    {
        if (bulletPrefab == null) return;
        GameObject bullet = ItemPoolManager.Instance.GetFromPool(bulletPrefab);
        if (bullet == null) return;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
    }

    private IEnumerator TaskSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(TaskspawnInterval);

            if (!TaskspawnerEnabled || player == null) continue;

            if (QuestManager.Instance.currentSubTask != null) continue;
            if (QuestManager.Instance.currentSubTask == null)
            {
                var point = GetNearestSpawnPoint();
                if (point != null)
                {
                    SpawnTask(point.transform.position);
                }
            }

        }
    }

    private void SpawnTask(Vector3 position)
    {
        if (taskPrefabs == null || taskPrefabs.Count == 0) return;

        GameObject prefab = taskPrefabs[Random.Range(0, taskPrefabs.Count)];
        if (prefab == null) return;

        GameObject task = ItemPoolManager.Instance.GetFromPool(prefab);
        if (task == null) return;

        if (taskParent != null)
            task.transform.SetParent(taskParent);
        else
            task.transform.SetParent(null); 

        task.transform.position = position;
        task.transform.rotation = Quaternion.identity;

        SubQuestTrigger trigger = task.GetComponent<SubQuestTrigger>();
        QuestData data = trigger.questData;
        data.isCompleted = false;
    }



    private SpawnPoint GetNearestSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) return null;
        if (player == null) return null;

        return spawnPoints
            .OrderBy(p => Vector3.Distance(player.position, p.transform.position))
            .FirstOrDefault();
    }
}
