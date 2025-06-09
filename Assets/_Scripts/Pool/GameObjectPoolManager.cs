using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPoolManager : MonoBehaviour
{
    public static GameObjectPoolManager Instance;

    private Dictionary<string, ObjectPool<GameObject>> poolDictionary = new Dictionary<string, ObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CreatePool(string key, GameObject prefab, int initialSize)
    {
        if (poolDictionary.ContainsKey(key)) return;

        ObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: (go) => go.SetActive(true),
            actionOnRelease: (go) => go.SetActive(false),
            actionOnDestroy: (go) => Destroy(go),
            collectionCheck: false,
            defaultCapacity: initialSize,
            maxSize: 100
        );

        // Pre-warm the pool
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = objectPool.Get();
            obj.SetActive(false);
            objectPool.Release(obj);
        }

        poolDictionary.Add(key, objectPool);
    }

    public GameObject GetObject(string key, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.TryGetValue(key, out var objectPool))
        {
            GameObject obj = objectPool.Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
        return null;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        if (!poolDictionary.ContainsKey(key))
        {
            Destroy(obj);
            return;
        }

        poolDictionary[key].Release(obj);
    }
}
