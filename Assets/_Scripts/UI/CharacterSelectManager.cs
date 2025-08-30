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

    [SerializeField] private int currentIndex = 0;

    void Start()
    {
        UpdateSelection();
        leftButton.onClick.AddListener(OnLeftButton);
        rightButton.onClick.AddListener(OnRightButton);
        Select.onClick.AddListener(OnSelectCharacter);
    }

    void Update()
    {
        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            points[currentIndex].position,
            Time.deltaTime * moveSpeed
        );
        Kai.SetBool("On", isKai);
        Ryo.SetBool("On", isRyo);
        Dane.SetBool("On", isDane);
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
