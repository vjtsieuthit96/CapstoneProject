using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ScaleAndEnableCollider : MonoBehaviour
{
    [Header("Kích thước cuối cùng của object")]
    public Vector3 targetScale = Vector3.one;

    [Header("Thời gian để phóng to (giây)")]
    public float scaleDuration = 1f;

    private BoxCollider boxCollider;
    private Vector3 initialScale;
    private float timer;
    private bool finished;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        initialScale = Vector3.zero;
        transform.localScale = initialScale;
    }

    void Update()
    {
        if (finished) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / scaleDuration);

        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        if (t >= 1f)
        {
            boxCollider.enabled = true;
            finished = true;
        }
    }
}
