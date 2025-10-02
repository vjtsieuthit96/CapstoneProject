using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject Offense;
    public GameObject Defense;
    public GameObject Vietnergy;
    public Button btt_Offense;
    public Button btt_Defense;
    public Button btt_Vietnergy;

    private void Start()
    {
        btt_Defense.onClick.AddListener(() =>
        {
            OnButtonChoose(Defense, Offense, Vietnergy);
        });
        btt_Offense.onClick.AddListener(() =>
        {
            OnButtonChoose(Offense, Defense, Vietnergy);
        });
        btt_Vietnergy.onClick.AddListener(() =>
        {
            OnButtonChoose(Vietnergy, Offense, Defense);
        });
    }
    private void OnEnable()
    {
        SetallActive();
    }

    public void SetallActive()
    {
        Offense.SetActive(true);
        Defense.SetActive(true);
        Vietnergy.SetActive(true);
    }

    public void OnButtonChoose(GameObject g1, GameObject g2, GameObject g3)
    {
        g1.SetActive(true);
        g2.SetActive(false);
        g3.SetActive(false);
    }    
}
