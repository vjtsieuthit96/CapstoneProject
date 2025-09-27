using UnityEngine;

public class AutoReturnToPool : MonoBehaviour
{
    [Tooltip("Prefab gốc trong ItemPoolManager (dùng để trả về pool)")]
    [HideInInspector] public GameObject prefabKey;

    [Tooltip("Thời gian tồn tại trước khi trả về pool")]
    public float lifetime = 5f;

    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        if (prefabKey != null && ItemPoolManager.Instance != null)
        {
            ItemPoolManager.Instance.ReturnToPool(prefabKey, gameObject);
        }
        else
        {
            Debug.LogWarning($"{name} không có prefabKey → chỉ SetActive(false).");
            gameObject.SetActive(false);
        }
    }
}
