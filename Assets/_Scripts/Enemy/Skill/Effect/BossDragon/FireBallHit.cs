
using UnityEngine;

public class FireBallHit : ESkillObjectSphere
{
    protected override void HitObj(RaycastHit hit)
    {
        return;        
    }   

    protected override void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("FireBallHit", this);
    } 
}
