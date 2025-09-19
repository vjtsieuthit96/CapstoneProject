using UnityEngine;

public class DimensionCutter : ESkillObjectMove
{
    [SerializeField] private DimensionHit hitPrefabs;   
    void Start()
    {      
        PoolManager.Instance.CreatePool("DMHit", hitPrefabs, 10);
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void ReturnObjectWhenHit()
    {
        return;
    }
    protected override void HitObj(RaycastHit hit)
    {
       PoolManager.Instance.GetObject<DimensionHit>("DMHit", hit.point, Quaternion.LookRotation(hit.normal));      
    }
    protected override void HitObjOtherTag(RaycastHit hit)
    {
        PoolManager.Instance.ReturnObject("DMCutter", this);
    }
    protected override void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DMCutter", this);
    }
    protected override void MovingStyle()
    {
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }
}
