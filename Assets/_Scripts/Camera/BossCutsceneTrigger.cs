using UnityEngine;

public class BossCutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [SerializeField] private Camera cutsceneCamera;
    [SerializeField] private Transform rotatingObject;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private Collider Collider;

    private bool isCutsceneActive = false;
    private float currentRotation = 0f;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isCutsceneActive && other.CompareTag("Player"))
        {
            if (cutsceneCamera != null)
            {
                CutsceneFunc.Instance.setCurrentCamera(cutsceneCamera);
                CutsceneFunc.Instance.OnCutScene();
                Time.timeScale = 0f;
                isCutsceneActive = true;
                currentRotation = 0f;
                Collider.enabled = false;
            }
            else
            {
                Debug.LogWarning("Chưa gắn Camera cho BossCutsceneTrigger!");
            }
        }
    }

    private void Update()
    {
        if (!isCutsceneActive || rotatingObject == null) return;

        float deltaRotation = rotateSpeed * Time.unscaledDeltaTime;
        rotatingObject.Rotate(0f, 0f, deltaRotation);
        currentRotation += deltaRotation;

        if (currentRotation >= 360f)
        {
            isCutsceneActive = false;
            CutsceneFunc.Instance.OffCutScene();
            Time.timeScale = 1f;
            Collider.enabled = false;
        }
    }
}
