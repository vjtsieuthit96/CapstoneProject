using UnityEngine;

public class DimensionHit : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(ReturnObject), 1.5f); 
    }

    void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DMHit", this);
    }
}
