using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;
using Invector;

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
        //if (currentController != null)
        //    currentController.onDead.RemoveListener(OnCharacterDead);

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

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(respawnDelay);
        OnCharacterDead(currentPlayer.gameObject);
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    //StartCoroutine(RespawnAfterSceneReady());
    //}

    private IEnumerator RespawnAfterSceneReady()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        // Xóa hoặc destroy player cũ
        if (oldPlayer != null)
        {
            if (destroyBodyAfterDead)
                Destroy(oldPlayer);
            else
                DestroyPlayerComponents(oldPlayer);

            oldPlayer = null;
        }

        // Reset các biến player
        currentPlayer = null;
        currentController = null;

        // Spawn player mới
        SpawnPlayer();

        // Hiển thị task nếu có
        if (QuestManager.Instance.currentMainTask != null)
            QuestUIManager.Instance.ShowTask(QuestManager.Instance.currentMainTask);
        if (QuestManager.Instance.currentSubTask != null)
            QuestUIManager.Instance.ShowTask(QuestManager.Instance.currentSubTask);

        isRespawning = false;
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

        //if (currentController != null)
        //{
        //    currentController.onDead.RemoveAllListeners();
        //    currentController.onDead.AddListener(OnCharacterDead);
        //}
        if (PathDrawer.Instance != null)
        {
            PathDrawer.Instance.CheckGame();
        }
        PlayerRealTimeData.Instance.isNewGame = false;
        PlayerRealTimeData.Instance.Scene1 = true;
    }

    private void DestroyPlayerComponents(GameObject target)
    {
        if (!target) return;

        foreach (var comp in target.GetComponentsInChildren<MonoBehaviour>())
            Destroy(comp);

        if (target.TryGetComponent(out Collider coll)) Destroy(coll);
        if (target.TryGetComponent(out Rigidbody rb)) Destroy(rb);
        if (target.TryGetComponent(out Animator anim)) Destroy(anim);
    }
}
