using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;

[System.Serializable]
public class PlayerRespawnOption
{
    [Header("Prefab nhân vật để respawn")]
    public GameObject playerPrefab;

    [Tooltip("Index của scene cutscene trong Build Settings (nếu không có thì = -1)")]
    public int cutsceneDeathSceneIndex = -1;

    [Tooltip("Thời gian cutscene chạy (giây) trước khi quay lại gameplay")]
    public float cutsceneDuration = 3f;
}

public class RespawnPlayer : MonoBehaviour
{
    [Header("Cấu hình Respawn cho từng nhân vật (theo Index chọn ở SceneIndexManager)")]
    public PlayerRespawnOption[] playerOptions;

    [Header("Respawn Settings")]
    [Tooltip("Thời gian chờ trước khi load cutscene sau khi chết")]
    public float respawnDelay = 1f;
    [Tooltip("Xóa hẳn xác nhân vật sau khi chết (nếu false thì chỉ xóa các component)")]
    public bool destroyBodyAfterDead = true;

    [Header("Spawn Settings")]
    [Tooltip("Spawnpoint ban đầu (nếu chưa có checkpoint)")]
    [SerializeField] private Vector3 initialSpawnPos = Vector3.zero;
    [SerializeField] private Vector3 initialSpawnEuler = Vector3.zero;

    public static RespawnPlayer Instance;

    private GameObject currentPlayer;
    private vThirdPersonController currentController;
    private GameObject oldPlayer;

    private Vector3 checkpointPos = Vector3.zero;
    private Quaternion checkpointRot = Quaternion.identity;
    private bool hasCheckpoint = false;

    private int lastGameplaySceneIndex;
    private int pendingCutsceneIndex = -1;
    private float pendingCutsceneDuration = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnPlayerAtCheckpoint();
    }

    private void OnCharacterDead(GameObject deadObj)
    {
        oldPlayer = deadObj;
        lastGameplaySceneIndex = SceneManager.GetActiveScene().buildIndex;

        int index = Mathf.Clamp(PlayerRealTimeData.Instance.PlayerIndex, 0, playerOptions.Length - 1);
        pendingCutsceneIndex = playerOptions[index].cutsceneDeathSceneIndex;
        pendingCutsceneDuration = playerOptions[index].cutsceneDuration;

        StartCoroutine(DeathSequence());
    }


    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (pendingCutsceneIndex >= 0)
        {
            SceneManager.LoadScene(pendingCutsceneIndex);

            yield return new WaitForSeconds(pendingCutsceneDuration);

            SceneManager.LoadScene(lastGameplaySceneIndex);
        }
        else
        {
            Debug.LogWarning("Không có Cutscene Death cho nhân vật này! Respawn ngay trong scene.");
            StartCoroutine(RespawnAfterCutscene());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == lastGameplaySceneIndex)
        {
            StartCoroutine(RespawnAfterCutscene());
        }
    }

    private IEnumerator RespawnAfterCutscene()
    {
        yield return new WaitForEndOfFrame();

        if (oldPlayer != null)
        {
            if (destroyBodyAfterDead) Destroy(oldPlayer);
            else DestroyPlayerComponents(oldPlayer);
            oldPlayer = null;
        }

        SpawnPlayerAtCheckpoint();
    }


    private void SpawnPlayerAtCheckpoint()
    {
        int index = Mathf.Clamp(PlayerRealTimeData.Instance.PlayerIndex, 0, playerOptions.Length - 1);
        var option = playerOptions[index];

        if (option.playerPrefab == null)
        {
            Debug.LogError($"PlayerRespawnOption[{index}] chưa có playerPrefab!");
            return;
        }

        Vector3 spawnPos = hasCheckpoint ? checkpointPos : initialSpawnPos;
        Quaternion spawnRot = hasCheckpoint ? checkpointRot : Quaternion.Euler(initialSpawnEuler);

        currentPlayer = Instantiate(option.playerPrefab, spawnPos, spawnRot);
        currentController = currentPlayer.GetComponent<vThirdPersonController>();

        if (currentController != null)
        {
            currentController.onDead.RemoveListener(OnCharacterDead);
            currentController.onDead.AddListener(OnCharacterDead);
        }

        PlayerRealTimeData.Instance.isNewGame = false;
    }

    public void SetCheckpoint(Vector3 position, Quaternion rotation)
    {
        checkpointPos = position;
        checkpointRot = rotation;
        hasCheckpoint = true;
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
