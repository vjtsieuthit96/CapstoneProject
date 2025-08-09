using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private int sceneIndex;    
    public bool isChangeMusic = false;
    public void LoadScene()
   {
        SceneManager.LoadScene(sceneIndex);
        if(isChangeMusic)
        {
            SoundMixerManager.Instance.MusicSource.Stop();
        }
    }
}
