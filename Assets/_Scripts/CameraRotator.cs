using UnityEngine;
using UnityEngine.UI;

public class CameraRotator : MonoBehaviour
{
    [Header("Tốc độ xoay (càng cao càng nhanh)")]
    public float rotateSpeed = 2f;

    private Quaternion targetRotation;
    private bool isRotating = false;

    [Header("Button")]
    public Button Btt_Setting;
    public Button Btt_Setting2Main;
    public Button Btt_Main2Quit;
    public Button Btt_No;

    private void Awake()
    {
        RotateTo(0f, 150f);
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

    public void RotateTo(float rotationX, float rotationY)
    {
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        isRotating = true;
    }
}
