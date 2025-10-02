using UnityEngine;

public class OpenningCutScene : MonoBehaviour
{
    public GameObject Text1;
   public void PlayerCutScene()
   {
        CutsceneFunc.Instance.OnCutScene();
   }   
    
   public void FinishCutScene()
   {
        Text1.SetActive(false);
        CutsceneFunc.Instance.OffCutScene();
   }

    private void Awake()
    {
        PlayerCutScene();
    }

}
