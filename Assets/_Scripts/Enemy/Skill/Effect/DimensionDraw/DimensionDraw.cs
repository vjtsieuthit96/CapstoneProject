using UnityEngine;

public class DimensionDraw : ESkillObjectSphere
{
    [SerializeField] private DimensionDrawHit hitPrefabs;
    void Start()
    {
        PoolManager.Instance.CreatePool("DMDrawHit", hitPrefabs, 10);
    }
    protected override void HitObj(RaycastHit hit)
    {
        PoolManager.Instance.GetObject<DimensionDrawHit>("DMDrawHit", hit.point, Quaternion.LookRotation(hit.normal));
    }

    protected override void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DMDraw", this);
    }

}
