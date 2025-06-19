using UnityEngine;

public class BossOgreEffectManager : MonoBehaviour
{
    [Header("-----Dimension Cutter-----")]
    [SerializeField] private DimensionCutter dimensionCutterPrefab;
    [SerializeField] private Transform cutterSpawnPoint;   
    [Header("-----Dimension Draw-----")]
    [SerializeField] private DimensionDraw dimensionDrawPrefab;
    [SerializeField] private Transform DrawSpawnPoint;

    [Header("-----Component-----")]
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private MonsterAI monsterAI;
    

    void Start()
    {        
        PoolManager.Instance.CreatePool<DimensionCutter>("DMCutter",dimensionCutterPrefab,5);
        PoolManager.Instance.CreatePool<DimensionDraw>("DMDraw", dimensionDrawPrefab, 5);
    }

    public void SpawnCutter()
    {
        DimensionCutter cutter = PoolManager.Instance.GetObject<DimensionCutter>("DMCutter", cutterSpawnPoint.position, Quaternion.identity);
        cutter.SetStats(monsterStats);    
        cutter.SetTarget(monsterAI.GetTarget());
    }

    public void SpawnDrawWithRotation(float zAngle)
    {
        Quaternion rotationZ = Quaternion.Euler(0, 0, zAngle);
        DimensionDraw draw = PoolManager.Instance.GetObject<DimensionDraw>("DMDraw", DrawSpawnPoint.position, rotationZ);
        draw.SetStats(monsterStats);
    }


}
