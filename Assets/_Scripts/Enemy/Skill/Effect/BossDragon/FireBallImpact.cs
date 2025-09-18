using UnityEngine;

public class FireBallImpact : MonoBehaviour
{
    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnObject), 2f);
    }

    void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("FireBallImpact", this);
    }
}
