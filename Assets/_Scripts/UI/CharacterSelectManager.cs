using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTransform;
    public float moveSpeed = 5f;

    [Header("Points")]
    public Transform[] points;

    [Header("Buttons")]
    public Button leftButton;
    public Button rightButton;
    public Button Select;

    [Header("Character Bools")]
    public bool isKai;
    public bool isRyo;
    public bool isDane;

    [Header("Character Animator")]
    public Animator Kai;
    public Animator Ryo;
    public Animator Dane;

    [Header("Character Info")]
    public GameObject Kai_Info;
    public GameObject Ryo_Info;
    public GameObject Dane_Info;

    [Header("Character Stats")]
    public GameObject Kai_Stats;
    public GameObject Ryo_Stats;
    public GameObject Dane_Stats;

    [Header("Panels")]
    public GameObject InfoPanel;
    public GameObject StatsPanel;

    public GameObject SettingPanel2D;
    public GameObject MainPanel;
    private bool isOn = false;

    [SerializeField] private int currentIndex = 0;

    void Start()
    {
        isOn = false;
        MainPanel.SetActive(true);
        UpdateSelection();
        leftButton.onClick.AddListener(OnLeftButton);
        rightButton.onClick.AddListener(OnRightButton);
        Select.onClick.AddListener(OnSelectCharacter);
        InfoPanel.SetActive(true);
        StatsPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOn = !isOn;
        }
        SettingPanel2D.SetActive(isOn);
        
        Select.gameObject.SetActive(!isOn);
        StatsPanel.SetActive(!isOn);
        InfoPanel.SetActive(!isOn);
        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            points[currentIndex].position,
            Time.deltaTime * moveSpeed
        );
        Kai.SetBool("On", isKai);
        Ryo.SetBool("On", isRyo);
        Dane.SetBool("On", isDane);
        //Kai
        Kai_Info.SetActive(isKai);
        Kai_Stats.SetActive(isKai);
        //Ryo
        Ryo_Info.SetActive(isRyo);
        Ryo_Stats.SetActive(isRyo);
        //Dane
        Dane_Info.SetActive(isDane);
        Dane_Stats.SetActive(isDane);
    }

    public void OnSelectCharacter()
    {
        if (isKai && !isRyo && !isDane)
        {
            Kai.SetBool("Select", isKai);
        }
        else if (!isKai && isRyo && !isDane)
        {
            Ryo.SetBool("Select", isRyo);
        }   
        else if (!isKai && !isRyo && isDane)
        {
            Dane.SetBool("Select", isDane);
        }
        leftButton.interactable = false;
        rightButton.interactable = false;
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        InfoPanel.SetActive(false);
        StatsPanel.SetActive(false);
        Select.gameObject.SetActive(false);
    }    

    public void OnLeftButton()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSelection();
        }
    }

    public void OnRightButton()
    {
        if (currentIndex < points.Length - 1)
        {
            currentIndex++;
            UpdateSelection();
        }
    }

    private void UpdateSelection()
    {
        leftButton.interactable = currentIndex > 0;
        rightButton.interactable = currentIndex < points.Length - 1;

        isRyo = (currentIndex == 0);
        isKai = (currentIndex == 1);
        isDane = (currentIndex == 2);
    }
}
