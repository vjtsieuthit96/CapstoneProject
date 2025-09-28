using UnityEngine;

public class PlayerMock : MonoBehaviour
{
    public static PlayerMock Instance { get; private set; }

    public Transform PlayerTransform { get; private set; }
    public GameObject PlayerPrefab { get; private set; }
    public GameObject _PlayerPrefab;
    public Transform _PlayerTransform;
    public DarkMagicTree Tree;
    public EnemySpawner enemySpawner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _PlayerPrefab = PlayerPrefab;
        _PlayerTransform = PlayerTransform;
    }

    public void SetPlayer(GameObject player)
    {
        PlayerPrefab = player;
        PlayerTransform = player.transform;
        ApplyPlayer();
    }
    public void ApplyPlayer()
    {
        enemySpawner.SetPlayer(PlayerTransform);
        Tree.target = PlayerTransform;
    }
}
