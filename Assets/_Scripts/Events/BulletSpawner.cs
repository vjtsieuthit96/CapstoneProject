using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // prefab đạn
    public Transform bulletParent;  // nơi chứa các đạn spawn ra

    [Header("Spawn Points")]
    public List<SpawnPoint> spawnPoints; // dùng chung kiểu spawnpoint như EnemySpawner

    [Header("Spawner Control")]
    public Transform player;
    public bool spawnerEnabled = true;
    public float spawnInterval = 20f; // 20 giây/lần
    public int bulletPerSpawn = 2;    // số lượng đạn mỗi lần

    private void Start()
    {
        if (player == null)
        {
            StartCoroutine(FindPlayerByTag("Player"));
        }

        StartCoroutine(SpawnRoutine());
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

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        if (bulletParent != null)
            bullet.transform.SetParent(bulletParent);
    }

    private SpawnPoint GetNearestSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) return null;
        if (player == null) return null;

        return spawnPoints
            .OrderBy(p => Vector3.Distance(player.position, p.transform.position))
            .FirstOrDefault();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, 5f); // chỉ để debug
        }
    }
#endif
}
