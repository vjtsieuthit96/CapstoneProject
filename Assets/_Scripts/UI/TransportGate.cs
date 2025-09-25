using Invector.vCamera;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransportGate : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    public Vector3 SpawnPointNextScene;
    public vThirdPersonCamera tpsCamera;
    private void Start()
    {
        StartCoroutine(AfterStart());

    }

    IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(0.2f);
        if (tpsCamera == null)
        {
            tpsCamera = FindObjectOfType<vThirdPersonCamera>();
        }

    }
private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRealTimeData.Instance.SetCheckpoint(SpawnPointNextScene, quaternion.identity);
            DontDestroyOnLoad(other.gameObject);
            DontDestroyOnLoad(tpsCamera.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayerRealTimeData.Instance.SetCheckpoint(SpawnPointNextScene, quaternion.identity);

            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = PlayerRealTimeData.Instance.spawnPos;
            player.transform.rotation = PlayerRealTimeData.Instance.spawnRot;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
