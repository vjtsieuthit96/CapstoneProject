using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Button Btt_Setting;
    public Button Btt_Back2Main;

    public GameObject MainPanel;
    public GameObject SettingPanel;

    private void Start()
    {
        Btt_Setting.onClick.AddListener(() => 
        {
            ButtonClick(MainPanel,SettingPanel);
        });
        Btt_Back2Main.onClick.AddListener(() =>
        {
            ButtonClick(SettingPanel,MainPanel);
        });
    }
    public void ButtonClick(GameObject CurrentPanel = null, GameObject NextPanel = null)
   {
        CurrentPanel.SetActive(false);
        NextPanel.SetActive(true);
   }    
}
