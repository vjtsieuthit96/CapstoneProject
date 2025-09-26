using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public int sceneIndex;    
    public bool isChangeMusic = false;
    public virtual void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
        if(isChangeMusic)
        {
            SoundMixerManager.Instance.MusicSource.Stop();
        }
    }
}
