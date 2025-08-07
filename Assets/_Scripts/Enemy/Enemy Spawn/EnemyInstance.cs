using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    public string poolKey;
    public System.Action<EnemyInstance> onDeath;
    public MonsterAI monsterAI;
    public MonsterStats stats;

    private void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
        stats = GetComponent<MonsterStats>();
    }
    public void Die()
    {
        onDeath?.Invoke(this);
        GameObjectPoolManager.Instance.ReturnObject(poolKey, gameObject);
        monsterAI.isDead = true;
        stats.ResetStatsToInitial();
    }
}