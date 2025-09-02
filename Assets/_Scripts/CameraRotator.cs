using UnityEngine;
using UnityEngine.UI;

public class CameraRotator : MonoBehaviour
{
    [Header("Tốc độ xoay (càng cao càng nhanh)")]
    public float rotateSpeed = 2f;

    private Quaternion targetRotation;
    private bool isRotating = false;

    [Header("Button 3D")]
    public Button Btt_Setting;
    public Button Btt_Setting2Main;
    public Button Btt_Main2Quit;
    public Button Btt_No;
    public Button Btt_Yes;

    [Header("Button 2D")]
    public Button Btt_2dSetting;
    public Button Btt_2dSetting2Main;
    public Button Btt_2dMain2Quit;
    public Button Btt_2dNo;
    public Button Btt_2dYes;

    [Header("Mode")]
    public GameObject Mode3d;
    public Button Btt_To2dMode;
    public GameObject Mode2d;
    public Button Btt_To3dMode;

    [Header("2D Panel")]
    public GameObject Main2d;
    public GameObject Setting2d;
    public GameObject Confirm2d;


    private void Awake()
    {
        RotateTo(0f, 150f);
        SwitchType(Mode2d, Mode3d);
    }
    private void Start()
    {
        Btt_Setting.onClick.AddListener(() => 
        {
            RotateTo(0f, 100f);
        });
        Btt_Setting2Main.onClick.AddListener(() =>
        {
            RotateTo(0f, 150f);
        });
        Btt_Main2Quit.onClick.AddListener(() =>
        {
            RotateTo(-50f, 150f);
        });
        Btt_No.onClick.AddListener(() =>
        {
            RotateTo(0f, 150f);
        });
        Btt_Yes.onClick.AddListener(Exit);
        Btt_2dYes.onClick.AddListener(Exit);
        Btt_To2dMode.onClick.AddListener(() =>
        {
            SwitchType(Mode3d, Mode2d);
            SwitchForm2D(Setting2d, Confirm2d, Main2d);
        });
        Btt_To3dMode.onClick.AddListener(() =>
        {
            SwitchType(Mode2d, Mode3d);
        });
        Btt_2dSetting.onClick.AddListener(() => 
        {
            SwitchForm2D(Main2d,Confirm2d,Setting2d);
        });
        Btt_2dSetting2Main.onClick.AddListener(() =>
        {
            SwitchForm2D(Setting2d, Confirm2d, Main2d);
        });
        Btt_2dMain2Quit.onClick.AddListener(() =>
        {
            SwitchForm2D(Main2d, Setting2d, Confirm2d);
        });
        Btt_2dNo.onClick.AddListener(() =>
        {
            SwitchForm2D(Confirm2d, Setting2d, Main2d);
        });
    }
    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotateSpeed
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SwitchType(GameObject TypeModeBase, GameObject TypeSwitch)
    {
        TypeModeBase.SetActive(false);
        TypeSwitch.SetActive(true);
    }    

    public void RotateTo(float rotationX, float rotationY)
    {
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        isRotating = true;
    }

    public void SwitchForm2D(GameObject NowForm,GameObject UnescessaryForm, GameObject FormSwitch)
    {
        NowForm.SetActive(false);
        UnescessaryForm.SetActive(false);
        FormSwitch.SetActive(true);
    }    
}
