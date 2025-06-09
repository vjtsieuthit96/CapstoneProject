using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField] private string poolKey = "IcePlane";
    [SerializeField] private float returnDelay = 5f;

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPoolNow), returnDelay);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPoolNow));
    }

    private void ReturnToPoolNow()
    {
        if (GameObjectPoolManager.Instance != null)
        {
            GameObjectPoolManager.Instance.ReturnObject(poolKey, gameObject);
        }
    }
}
