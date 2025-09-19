using UnityEngine;

public class GroundScatter : ESkillObjectSphere
{
    protected override void HitObj(RaycastHit hit)
    {
        return;
    }

    protected override void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("GroundScatter", this);
    }
}
