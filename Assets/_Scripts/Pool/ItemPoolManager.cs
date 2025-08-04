using System.Collections.Generic;
using UnityEngine;

public class ItemPoolManager : MonoBehaviour
{
    public static ItemPoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size = 10;
    }

    public List<Pool> pools;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.prefab, objectPool);
        }
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab)) return null;

        var pool = poolDictionary[prefab];

        if (pool.Count == 0)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            return newObj;
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject instance)
    {
        if (!poolDictionary.ContainsKey(prefab)) return;

        instance.SetActive(false);
        poolDictionary[prefab].Enqueue(instance);
    }
}
