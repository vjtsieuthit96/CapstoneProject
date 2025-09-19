using Unity.VisualScripting;
using UnityEngine;

public class DragonBossEffectManager : MonoBehaviour
{
    [Header("-----FireBall-----")]
    [SerializeField] private FireBallMove fireBallPrefab;
    [SerializeField] private FireBallHit hitPrefabs;
    [SerializeField] private FireBallImpact impactPrefab;
    [SerializeField] private Transform mountSpawnPos;
    [Header("-----FlameBreath-----")]
    [SerializeField] private FlameBreathManager flameBreath;
    [Header("-----GroundScatter-----")]
    [SerializeField] private GroundScatter groundScatter;
    [SerializeField] private Transform[] handSpawnPos;
    [Header("-----Component-----")]
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private MonsterAI monsterAI;


    void Start()
    {
        PoolManager.Instance.CreatePool<FireBallMove>("FireBall", fireBallPrefab, 5);
        PoolManager.Instance.CreatePool<FireBallHit>("FireBallHit", hitPrefabs, 5);
        PoolManager.Instance.CreatePool<FireBallImpact>("FireBallImpact", impactPrefab, 5);
        PoolManager.Instance.CreatePool<FlameBreathManager>("FlameBreath", flameBreath, 2);
        PoolManager.Instance.CreatePool<GroundScatter>("GroundScatter", groundScatter, 2);
    }

    public void SpawnFireBall()
    {
        LookAtTarget();
        PoolManager.Instance.GetObject<FireBallImpact>("FireBallImpact", mountSpawnPos.position, Quaternion.identity);
        FireBallMove fireBall = PoolManager.Instance.GetObject<FireBallMove>("FireBall", mountSpawnPos.position,Quaternion.identity);
        fireBall.SetStats(monsterStats);
        fireBall.GetTarget(monsterAI.GetTarget());        
    }
    public void SpawnFlameBreath()
    {
        Debug.Log("FlameBreath");
        LookAtTarget();
        Vector3 direction = (monsterAI.GetTarget().position - mountSpawnPos.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        FlameBreathManager flame = PoolManager.Instance.GetObject<FlameBreathManager>("FlameBreath", mountSpawnPos.position, lookRotation);
        FlameBreathCollision flameBreathCollision = flame.GetComponentInChildren<FlameBreathCollision>();
        flameBreathCollision.SetStats(monsterStats);
    }

    public void SpawnGroundScatter()
    {       
        GroundScatter scatter = PoolManager.Instance.GetObject<GroundScatter>("GroundScatter",monsterAI.GetTarget().position, Quaternion.identity);
        scatter.SetStats(monsterStats);
    }
    public void SpawnHandEffect()
    {
        foreach (Transform hand in handSpawnPos)
        {
            PoolManager.Instance.GetObject<FireBallImpact>("FireBallImpact", mountSpawnPos.position, Quaternion.identity);
        }
    }

    private void LookAtTarget()
    {
        if (monsterAI.GetTarget() == null) return;
        Vector3 direction = (monsterAI.GetTarget().position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
