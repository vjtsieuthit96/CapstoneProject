﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolGeneric <T> where T : MonoBehaviour
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPoolGeneric(T prefab, int inittialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < inittialSize; i++)
        {
            CreateNewObject();
        }
    }
    //Tạo object
    private T CreateNewObject()
    {
        T newObj = Object.Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);
        pool.Enqueue(newObj);
        return newObj;
    }
   

    public T GetObject(Vector3 position,Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }
        T obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false );
        pool.Enqueue(obj);
    }
}
