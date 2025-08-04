using UnityEngine;
using UnityEngine.UI;

public class SkillTreePanel : MonoBehaviour
{
    public GameObject PanelOffence;
    public GameObject PanelDeffence;
    public GameObject PanelVietnergy;
    public GameObject PanelButton;

    public CanvasGroup Offence;
    public CanvasGroup Deffence;
    public CanvasGroup Vietnergy;
    public CanvasGroup Buttonpanel;

    public Button bttOffence;
    public Button bttDeffence;
    public Button bttVietnergy;
    public Button bttBack;

    private void Awake()
    {
        Offence = PanelOffence.GetComponent<CanvasGroup>();
        Deffence = PanelDeffence.GetComponent<CanvasGroup>();
        Vietnergy = PanelVietnergy.GetComponent<CanvasGroup>();
        Buttonpanel = PanelButton.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        bttDeffence.onClick.AddListener(OpenDeffence);
        bttOffence.onClick.AddListener(OpenOffence);
        bttVietnergy.onClick.AddListener(OpenVietnergy);
        bttBack.onClick.AddListener(OpenButton);
    }

    public void OpenOffence()
    {
        Offence.alpha = 1.0f;
        Offence.interactable = true;
        Offence.blocksRaycasts = true;

        Deffence.alpha = 0f;
        Deffence.interactable = false;
        Deffence.blocksRaycasts = false;

        Vietnergy.alpha = 0f;
        Vietnergy.interactable = false;
        Vietnergy.blocksRaycasts = false;

        Buttonpanel.alpha = 0f;
        Buttonpanel.interactable = false;
        Buttonpanel.blocksRaycasts = false;
    }

    public void OpenDeffence()
    {
        Offence.alpha = 0f;
        Offence.interactable = false;
        Offence.blocksRaycasts = false;

        Deffence.alpha = 0f;
        Deffence.interactable = true;
        Deffence.blocksRaycasts = true;

        Vietnergy.alpha = 0f;
        Vietnergy.interactable = false;
        Vietnergy.blocksRaycasts = false;

        Buttonpanel.alpha = 0f;
        Buttonpanel.interactable = false;
        Buttonpanel.blocksRaycasts = false;
    }

    public void OpenVietnergy()
    {
        Offence.alpha = 0f;
        Offence.interactable = false;
        Offence.blocksRaycasts = false;

        Deffence.alpha = 0f;
        Deffence.interactable = false;
        Deffence.blocksRaycasts = false;

        Vietnergy.alpha = 1.0f;
        Vietnergy.interactable = true;
        Vietnergy.blocksRaycasts = true;

        Buttonpanel.alpha = 0f;
        Buttonpanel.interactable = false;
        Buttonpanel.blocksRaycasts = false;
    }

    public void OpenButton()
    {
        Offence.alpha = 0f;
        Offence.interactable = false;
        Offence.blocksRaycasts = false;

        Deffence.alpha = 0f;
        Deffence.interactable = false;
        Deffence.blocksRaycasts = false;

        Vietnergy.alpha = 0f;
        Vietnergy.interactable = false;
        Vietnergy.blocksRaycasts = false;

        Buttonpanel.alpha = 1.0f;
        Buttonpanel.interactable = true;
        Buttonpanel.blocksRaycasts = true;
    }
}
