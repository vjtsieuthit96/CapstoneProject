using Unity.VisualScripting;
using UnityEngine;

public class DragonBossEffectManager : MonoBehaviour
{
    [Header("-----FireBall-----")]
    [SerializeField] private FireBallMove fireBallPrefab;
    [SerializeField] private FireBallHit hitPrefabs;
    [SerializeField] private Transform fireBallSpawnPos;    
    [Header("-----Component-----")]
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private MonsterAI monsterAI;


    void Start()
    {
        PoolManager.Instance.CreatePool<FireBallMove>("FireBall", fireBallPrefab, 5);
        PoolManager.Instance.CreatePool("FireBallHit", hitPrefabs, 10);
    }

    public void SpawnFireBall()
    {       
        FireBallMove fireBall = PoolManager.Instance.GetObject<FireBallMove>("FireBall", fireBallSpawnPos.position,Quaternion.identity);
        fireBall.SetStats(monsterStats);
        fireBall.GetTarget(monsterAI.GetTarget());
    }
   
}
