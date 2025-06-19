using UnityEngine;

public class DimensionDrawHit : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(ReturnObject), 1.5f);
    }

    void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DMDrawHit", this);
    }
}
