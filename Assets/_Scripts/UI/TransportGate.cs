using Invector.vCamera;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class TransportGate : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    public Vector3 SpawnPointNextScene;
    public vThirdPersonCamera tpsCamera;

    IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(0.2f);
        if (tpsCamera == null)
        {
            tpsCamera = FindObjectOfType<vThirdPersonCamera>();
        }

    }
    private void OnEnable()
    {
        StartCoroutine(AfterStart());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRealTimeData.Instance.SetCheckpoint(SpawnPointNextScene, quaternion.identity);
            PlayerRealTimeData.Instance.SetDefaultPoint(SpawnPointNextScene, quaternion.identity);
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
