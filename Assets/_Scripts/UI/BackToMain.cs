using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : ChangeScene
{
    public override void LoadScene()
    {
        SaveManager.Instance.SaveGame();
        PlayerRealTimeData.Instance.ResetRuntimeData();
        SceneManager.LoadScene(sceneIndex);
    }
}
