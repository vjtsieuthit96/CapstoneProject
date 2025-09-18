using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;

[System.Serializable]
public class PlayerRespawnOption
{
    public GameObject playerPrefab;

    [Tooltip("Index của scene cutscene trong Build Settings")]
    public int cutsceneDeathSceneIndex;

    [Tooltip("Thời gian cutscene chạy (giây) trước khi quay lại gameplay")]
    public float cutsceneDuration = 3f;
}

public class RespawnPlayer : MonoBehaviour
{
    [Header("Cấu hình Respawn cho từng nhân vật (theo Index chọn ở SceneIndexManager)")]
    public PlayerRespawnOption[] playerOptions;

    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public bool destroyBodyAfterDead = true;

    [Header("Spawn Settings")]
    [Tooltip("Spawnpoint ban đầu (nếu chưa có checkpoint)")]
    [SerializeField] private Vector3 initialSpawnPos = Vector3.zero;
    [SerializeField] private Vector3 initialSpawnEuler = Vector3.zero;

    public static RespawnPlayer Instance;

    private GameObject currentPlayer;
    private vThirdPersonController currentController;
    private GameObject oldPlayer;

    [SerializeField] private Vector3 checkpointPos = Vector3.zero;
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
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SpawnPlayerAtCheckpoint();
    }

    private void OnCharacterDead(GameObject deadObj)
    {
        oldPlayer = deadObj;
        lastGameplaySceneIndex = SceneManager.GetActiveScene().buildIndex;

        int index = Mathf.Clamp(SceneIndexManager.Instance.selectedIndex, 0, playerOptions.Length - 1);
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
            Debug.LogWarning("Không có Cutscene Death cho nhân vật này!");
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
        }

        SpawnPlayerAtCheckpoint();
    }

    private void SpawnPlayerAtCheckpoint()
    {
        
        int index = Mathf.Clamp(SceneIndexManager.Instance.selectedIndex, 0, playerOptions.Length - 1);
        var option = playerOptions[index];

        Vector3 spawnPos;
        Quaternion spawnRot;

        if (hasCheckpoint)
        {
            spawnPos = checkpointPos;
            spawnRot = checkpointRot;
        }
        else
        {
            spawnPos = initialSpawnPos;
            spawnRot = Quaternion.Euler(initialSpawnEuler);
        }

        currentPlayer = Instantiate(option.playerPrefab, spawnPos, spawnRot);
        currentController = currentPlayer.GetComponent<vThirdPersonController>();

        if (currentController != null)
        {
            currentController.onDead.AddListener(OnCharacterDead);
        }
        SceneIndexManager.Instance.isNewGame = false;
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

        var comps = target.GetComponentsInChildren<MonoBehaviour>();
        foreach (var comp in comps)
        {
            Destroy(comp);
        }

        var coll = target.GetComponent<Collider>();
        if (coll) Destroy(coll);

        var rb = target.GetComponent<Rigidbody>();
        if (rb) Destroy(rb);

        var anim = target.GetComponent<Animator>();
        if (anim) Destroy(anim);
    }
}
