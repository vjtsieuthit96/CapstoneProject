using System.Collections.Generic;
using UnityEngine;

public class ItemPoolManager : MonoBehaviour
{
    public static ItemPoolManager Instance;
    public Transform ObjectParent;

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size = 10;
    }

    [Header("Cấu hình pool")]
    public List<Pool> pools;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (ObjectParent == null)
        {
            GameObject containerGO = new GameObject("ItemPoolContainer");
            ObjectParent = containerGO.transform;
        }

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, ObjectParent);
                obj.SetActive(false);

                var autoReturn = obj.GetComponent<AutoReturnToPool>();
                if (autoReturn != null)
                {
                    autoReturn.prefabKey = pool.prefab;
                }

                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.prefab, objectPool);
        }
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Prefab {prefab.name} không tồn tại trong pool!");
            return null;
        }

        var pool = poolDictionary[prefab];
        GameObject obj;

        if (pool.Count == 0)
        {
            obj = Instantiate(prefab, ObjectParent);
            Debug.Log($"Pool rỗng, tạo mới: {prefab.name}");
        }
        else
        {
            obj = pool.Dequeue();
        }

        var autoReturn = obj.GetComponent<AutoReturnToPool>();
        if (autoReturn != null)
        {
            autoReturn.prefabKey = prefab;
        }

        obj.SetActive(true);
        Debug.Log($"Lấy {obj.name} từ pool {prefab.name}");
        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject instance)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Prefab {prefab.name} không có trong pool!");
            return;
        }

        instance.SetActive(false);
        poolDictionary[prefab].Enqueue(instance);
        Debug.Log($"{instance.name} được trả về pool {prefab.name}");
    }
}
