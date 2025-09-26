using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromDeath : ChangeScene
{
    public int SceneMap2 = 12;

    public override void LoadScene()
    {
        if(PlayerRealTimeData.Instance.Scene1 && !PlayerRealTimeData.Instance.Scene2)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else if(PlayerRealTimeData.Instance.Scene1 && PlayerRealTimeData.Instance.Scene2)
        {
            SceneManager.LoadScene(SceneMap2);

        }
    }
}
