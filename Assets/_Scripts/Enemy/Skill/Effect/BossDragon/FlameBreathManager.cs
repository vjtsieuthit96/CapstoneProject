using UnityEngine;

public class FlameBreathManager : MonoBehaviour
{
    [SerializeField]private float DestroyTime;
   
    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnObject), DestroyTime);
    }  
    private void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("FlameBreath", this);
    }
}
