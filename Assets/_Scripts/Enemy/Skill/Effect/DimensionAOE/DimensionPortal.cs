using UnityEngine;

public class DimensionPortal : ESkillObjectSphere
{  
    protected override void HitObj(RaycastHit hit)
    {
        PoolManager.Instance.GetObject<DimensionDrawHit>("DMDrawHit", hit.point, Quaternion.LookRotation(hit.normal));       
    }

    protected override void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DMPortal", this);
    }
}
