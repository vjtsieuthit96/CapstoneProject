using UnityEngine;

public interface IMonsterFactory
{
    GameObject SpawnEnemy(EnemyData data, Vector3 position, Quaternion rotation);
    void ReturnEnemy(EnemyData data, GameObject enemy);
}
