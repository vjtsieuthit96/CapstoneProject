using UnityEngine;

public class BossOgreEffectManager : MonoBehaviour
{
    [Header("-----Dimension Cutter-----")]
    [SerializeField] private DimensionCutter dimensionCutterPrefab;
    [SerializeField] private Transform cutterSpawnPoint;

    [Header("-----Component-----")]
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private MonsterAI monsterAI;

    void Start()
    {
        PoolManager.Instance.CreatePool<DimensionCutter>("DMCutter",dimensionCutterPrefab,5);
    }

    public void SpawnCutter()
    {
        DimensionCutter cutter = PoolManager.Instance.GetObject<DimensionCutter>("DMCutter", cutterSpawnPoint.position, Quaternion.identity);
        cutter.SetStatAndTarget(monsterStats,monsterAI);    
        cutter.SetTarget(monsterAI.GetTarget());
    }
    
}
