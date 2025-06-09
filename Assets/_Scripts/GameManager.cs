using Invector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private vExplosive ExplosionPrefab;
    [SerializeField] private int explosionPoolSize = 10;

    void Start()
    {
        PoolManager.Instance.CreatePool("Explosion", ExplosionPrefab, explosionPoolSize);
    }
}
