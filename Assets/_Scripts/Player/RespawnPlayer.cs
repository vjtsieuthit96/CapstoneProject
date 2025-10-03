using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;
using Invector;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PlayerRespawnOption
{
    [Header("Prefab nhân vật để respawn")]
    public GameObject playerPrefab;

    [Tooltip("Scene index sẽ load khi nhân vật này chết")]
    public int respawnSceneIndex = -1;
}

public class RespawnPlayer : MonoBehaviour
{
    public static RespawnPlayer Instance;
    [Header("Cấu hình Respawn cho từng nhân vật")]
    public PlayerRespawnOption[] playerOptions;

    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public bool destroyBodyAfterDead = true;

    private GameObject currentPlayer;
    [SerializeField] private vThirdPersonController currentController;
    private GameObject oldPlayer;
    [SerializeField] private bool isRespawning = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SpawnPlayer();
        PlayerMock.Instance.SetPlayer(currentPlayer);
       
    }
    private void Update()
    {
        if (isRespawning || currentPlayer == null) return;

        var health = currentPlayer.GetComponent<vHealthController>();
        if (health != null && health.currentHealth <= 0 && currentPlayer.IsDead())
        {
            StartCoroutine(DeathSequence());
        }
    }



    private void OnCharacterDead(GameObject deadObj)
    {

        if (isRespawning) return;
        isRespawning = true;

        oldPlayer = deadObj;

        int index = Mathf.Clamp(PlayerRealTimeData.Instance.PlayerIndex, 0, playerOptions.Length - 1);
        int targetSceneIndex = playerOptions[index].respawnSceneIndex;
        
        if (targetSceneIndex < 0)
        {
            Debug.LogError($"PlayerRespawnOption[{index}] chưa có respawnSceneIndex hợp lệ!");
            isRespawning = false;
            return;
        }
        SceneManager.LoadScene(targetSceneIndex);
    }
    public void SetThirdPersonController(vThirdPersonController vThird)
    {
        if(currentController == null)
        this.currentController = vThird;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SpawnPlayerDelayed());
    }

    private IEnumerator SpawnPlayerDelayed()
    {
        yield return new WaitForSeconds(1f);
        SpawnPlayer();
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(respawnDelay);
        OnCharacterDead(currentPlayer.gameObject);
    }
    private void SpawnPlayer()
    {
        int index = Mathf.Clamp(PlayerRealTimeData.Instance.PlayerIndex, 0, playerOptions.Length - 1);
        var option = playerOptions[index];

        if (option.playerPrefab == null)
        {
            Debug.LogError($"PlayerRespawnOption[{index}] chưa có playerPrefab!");
            return;
        }

        Vector3 spawnPos;
        Quaternion spawnRot;

        if (PlayerRealTimeData.Instance.isNewGame)
        {
            spawnPos = PlayerRealTimeData.Instance.defaultSpawnPos;
            spawnRot = Quaternion.Euler(PlayerRealTimeData.Instance.defaultSpawnEuler);
            PlayerRealTimeData.Instance.SetCheckpoint(spawnPos, spawnRot);
        }
        else
        {
            spawnPos = PlayerRealTimeData.Instance.spawnPos;
            spawnRot = PlayerRealTimeData.Instance.spawnRot;
        }

        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out var hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
            spawnPos = hit.position;

        currentPlayer = Instantiate(option.playerPrefab, spawnPos, spawnRot);
        PlayerMock.Instance.SetPlayer(currentPlayer);
        currentController = currentPlayer.GetComponent<vThirdPersonController>();
        if (PathDrawer.Instance != null)
        {
            PathDrawer.Instance.CheckGame();
        }
        PlayerRealTimeData.Instance.isNewGame = false;
        PlayerRealTimeData.Instance.Scene1 = true;
    }
}
