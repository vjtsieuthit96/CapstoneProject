using System.Collections;
using UnityEngine;

public class DimensionSpawnEffect : MonoBehaviour    
{
    [SerializeField] private DimensionPortal dimensionPortalPrefab;
    [SerializeField] private float minRadius = 5f;
    [SerializeField] private float maxRadius = 20f;
    [SerializeField] private float delay = 0.5f;    
    [SerializeField] private int effectCount = 10;
    private MonsterStats monsterStats;
    private Coroutine spawnRoutine;

    private void Start()
    {
        PoolManager.Instance.CreatePool<DimensionPortal>("DMPortal",dimensionPortalPrefab,10);
    }    
    private void OnEnable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        spawnRoutine = StartCoroutine(SpawnDimension(transform.position, effectCount, delay));
        Invoke(nameof(ReturnOjbect), effectCount * delay + 3f); 
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }
    IEnumerator SpawnDimension(Vector3 center,int count, float delay)
    {
       
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(delay);
            Vector2 rand = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
            Vector3 spawnPosition = center + new Vector3(rand.x, 0, rand.y);
            DimensionPortal portal = PoolManager.Instance.GetObject<DimensionPortal>("DMPortal", spawnPosition, Quaternion.identity);
            portal.SetStats(monsterStats);
        }
        
    }
    public void SetStats(MonsterStats stats)
    {
        this.monsterStats = stats;
    }
    public void ReturnOjbect()
    {
        PoolManager.Instance.ReturnObject("DMAOE", this);
    }
}
