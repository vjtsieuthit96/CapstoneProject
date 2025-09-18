using Unity.VisualScripting;
using UnityEngine;

public class DragonBossEffectManager : MonoBehaviour
{
    [Header("-----FireBall-----")]
    [SerializeField] private FireBallMove fireBallPrefab;
    [SerializeField] private FireBallHit hitPrefabs;
    [SerializeField] private FireBallImpact impactPrefab;
    [SerializeField] private Transform fireBallSpawnPos;    
    [Header("-----Component-----")]
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private MonsterAI monsterAI;


    void Start()
    {
        PoolManager.Instance.CreatePool<FireBallMove>("FireBall", fireBallPrefab, 5);
        PoolManager.Instance.CreatePool<FireBallHit>("FireBallHit", hitPrefabs, 5);
        PoolManager.Instance.CreatePool<FireBallImpact>("FireBallImpact", impactPrefab, 5);
    }

    public void SpawnFireBall()
    {
        LookAtTarget();
        PoolManager.Instance.GetObject<FireBallImpact>("FireBallImpact", fireBallSpawnPos.position, Quaternion.identity);
        FireBallMove fireBall = PoolManager.Instance.GetObject<FireBallMove>("FireBall", fireBallSpawnPos.position,Quaternion.identity);
        fireBall.SetStats(monsterStats);
        fireBall.GetTarget(monsterAI.GetTarget());
    }

    private void LookAtTarget()
    {
        if (monsterAI.GetTarget() == null) return;
        Vector3 direction = (monsterAI.GetTarget().position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
