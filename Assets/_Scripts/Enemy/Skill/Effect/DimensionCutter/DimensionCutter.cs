using Invector.vCharacterController;
using UnityEngine;

public class DimensionCutter : ESkillObjectMove
{
    [SerializeField] private DimensionHit hitPrefabs;
    

    protected override void Start()
    {       
        base.Start();
        PoolManager.Instance.CreatePool("DMHit", hitPrefabs, 10);
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void HitObj(RaycastHit hit)
    {
       PoolManager.Instance.GetObject<DimensionHit>("DMHit", hit.point, Quaternion.LookRotation(hit.normal));      
    }

    protected override void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DMCutter", this);
    }
}
