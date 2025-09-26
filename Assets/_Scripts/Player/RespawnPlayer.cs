using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;

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
    private vThirdPersonController currentController;
    private GameObject oldPlayer;

    private bool isRespawning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnPlayer();
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
            return;
        }

        StartCoroutine(DeathSequence(targetSceneIndex));
    }

    private IEnumerator DeathSequence(int targetSceneIndex)
    {
        yield return new WaitForSeconds(respawnDelay);
        SceneManager.LoadScene(targetSceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RespawnAfterSceneReady());
    }

    private IEnumerator RespawnAfterSceneReady()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        if (oldPlayer != null)
        {
            if (destroyBodyAfterDead) Destroy(oldPlayer);
            else DestroyPlayerComponents(oldPlayer);
            oldPlayer = null;
        }
        if (currentPlayer == null)
            SpawnPlayer();

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
        {
            spawnPos = hit.position;
        }

        currentPlayer = Instantiate(option.playerPrefab, spawnPos, spawnRot);
        currentController = currentPlayer.GetComponent<vThirdPersonController>();

        if (currentController != null)
        {
            currentController.onDead.RemoveListener(OnCharacterDead);
            currentController.onDead.AddListener(OnCharacterDead);
        }

        PlayerRealTimeData.Instance.isNewGame = false;
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
