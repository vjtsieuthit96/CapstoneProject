using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransportGate : MonoBehaviour
{

    [SerializeField] private int sceneIndex;
    public Vector3 SpawnPointNextScene;
    public void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRealTimeData.Instance.isNewGame = true;
            PlayerRealTimeData.Instance.SetCheckpoint(SpawnPointNextScene,quaternion.identity);
            LoadScene(sceneIndex);
        }    
    }
}
