using System.Collections.Generic;
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
    public AIDirector AI;

    public List<QuestData> datas;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        foreach (QuestData data in datas)
        {
            data.isCompleted = false;
        }
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
        if (Tree == null)
        {
            Tree = FindAnyObjectByType<DarkMagicTree>();
        }
        if(enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }
        ApplyPlayer();
    }
    public void ApplyPlayer()
    {
        enemySpawner.SetPlayer(PlayerTransform);
        PathDrawer.Instance.setplayer(PlayerTransform);
        Tree.target = PlayerTransform;
        AI.SetPlayer(PlayerTransform);
    }
}
