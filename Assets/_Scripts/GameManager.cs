using Invector;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private vExplosive ExplosionPrefab;
    [SerializeField] private vExplosive ExplosionIcePrefab;
    [SerializeField] private vExplosive ExplosionElectricPrefab;
    [SerializeField] private vExplosive ExplosionPoisonPrefab;
    [SerializeField] private GameObject IcePlanePrefab;
    [SerializeField] private GameObject IceCube;
    [SerializeField] private int explosionPoolSize = 5;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PoolManager.Instance.CreatePool("Explosion", ExplosionPrefab, explosionPoolSize);
        PoolManager.Instance.CreatePool("IceExplosion", ExplosionIcePrefab, explosionPoolSize);
        PoolManager.Instance.CreatePool("ElectricExplosion", ExplosionElectricPrefab, explosionPoolSize);
        PoolManager.Instance.CreatePool("PoisonExplosion", ExplosionPoisonPrefab, explosionPoolSize);
        GameObjectPoolManager.Instance.CreatePool("IcePlane", IcePlanePrefab, explosionPoolSize * 2);
        GameObjectPoolManager.Instance.CreatePool("IceCube", IceCube, explosionPoolSize * 2);

    }
}
