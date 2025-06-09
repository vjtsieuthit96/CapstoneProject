using Invector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private vExplosive ExplosionPrefab;
    [SerializeField] private vExplosive ExplosionIcePrefab;
    [SerializeField] private int explosionPoolSize = 5;

    void Start()
    {
        PoolManager.Instance.CreatePool("Explosion", ExplosionPrefab, explosionPoolSize);
        PoolManager.Instance.CreatePool("IceExplosion", ExplosionIcePrefab, explosionPoolSize);
    }
}
