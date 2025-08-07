using UnityEngine;
using System.Collections.Generic;

public class MonsterPool
{
    private readonly GameObject _prefab;
    private readonly Stack<GameObject> _pool;
    private readonly Transform _parent;

    public MonsterPool(GameObject prefab, int initialSize, Transform parent)
    {
        _prefab = prefab;
        _pool = new Stack<GameObject>(initialSize);
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Object.Instantiate(_prefab, _parent);
            obj.SetActive(false);
            _pool.Push(obj);
        }
    }

    public GameObject Get(Vector3 pos, Quaternion rot)
    {
        GameObject obj = _pool.Count > 0 ? _pool.Pop() : Object.Instantiate(_prefab, _parent);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Push(obj);
    }
}
