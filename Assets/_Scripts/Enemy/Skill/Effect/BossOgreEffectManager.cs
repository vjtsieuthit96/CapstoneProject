using Unity.VisualScripting;
using UnityEngine;

public class BossOgreEffectManager : MonoBehaviour
{
    [Header("-----Dimension Cutter-----")]
    [SerializeField] private DimensionCutter dimensionCutterPrefab;
    [SerializeField] private Transform cutterSpawnPoint;   
    [Header("-----Dimension Draw-----")]
    [SerializeField] private DimensionDraw dimensionDrawPrefab;
    [SerializeField] private Transform DrawSpawnPoint;
    [SerializeField] private DimensionDrawHit hitPrefabs;
    [Header("-----Dimension AOE-----")]
    [SerializeField] private DimensionSpawnEffect dimensionAOEPrefab;  
    [SerializeField] private GameObject aoePlane;
    [Header("-----Rage-----")]
    [SerializeField] private GameObject rageEffect;
    [Header("-----Component-----")]
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private MonsterAI monsterAI;
    

    void Start()
    {        
        PoolManager.Instance.CreatePool<DimensionCutter>("DMCutter",dimensionCutterPrefab,5);
        PoolManager.Instance.CreatePool<DimensionDraw>("DMDraw", dimensionDrawPrefab, 5);
        PoolManager.Instance.CreatePool<DimensionSpawnEffect>("DMAOE", dimensionAOEPrefab, 2);        
        PoolManager.Instance.CreatePool("DMDrawHit", hitPrefabs, 10);        
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

    public void SpawnDimensionAOE()
    {
        Vector3 spawmPos = monsterAI.GetTarget().position;
        spawmPos.y = 0; 
        DimensionSpawnEffect aoe = PoolManager.Instance.GetObject<DimensionSpawnEffect>("DMAOE", spawmPos, Quaternion.identity);
        aoe.SetStats(monsterStats);
        aoePlane.SetActive(true);
    }

    public void EnableRageRoar()
    {
        rageEffect.SetActive(true);
    }
    public void DisableRageRoar()
    {
        rageEffect.SetActive(false);
    }
}
