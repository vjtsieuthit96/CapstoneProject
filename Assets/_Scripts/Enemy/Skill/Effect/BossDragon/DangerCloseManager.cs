using UnityEngine;

public class DangerCloseManager : MonoBehaviour
{
    [SerializeField] private float DestroyTime;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnObject), DestroyTime);
    }
    private void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("DangerClose", this);
    }
}
