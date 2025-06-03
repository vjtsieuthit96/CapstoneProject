using System.Collections.Generic;
using UnityEngine;

public class ObjectPool <T> where T : MonoBehaviour
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform Parent;

    public ObjectPool(T prefab, int size, Transform parent = null)
    {
        this.prefab = prefab;
        this.Parent = parent;

        for (int i = 0; i < size; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }    
    }

    private void CreateNewObject()
    {
        T obj = GameObject.Instantiate (prefab, Parent);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public T GetObject()
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }
        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false );
        pool.Enqueue(obj);
    }
}
