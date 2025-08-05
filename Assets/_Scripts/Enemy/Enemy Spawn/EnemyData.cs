using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public int point;
    public int initialPoolSize;
}
