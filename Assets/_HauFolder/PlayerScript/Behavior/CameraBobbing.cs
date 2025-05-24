using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    private float amplitude = 0f;
    private float frequency = 0f;
    private float bobTimer = 0f;
    private Vector3 initialLocalPos;

    private void Awake()
    {
        initialLocalPos = transform.localPosition;
    }

    public void SetBobbing(float amplitude, float frequency)
    {
        this.amplitude = Mathf.Clamp(amplitude, 0f, 0.05f);
        this.frequency = Mathf.Clamp(frequency, 0f, 6f);
    }

    private void Update()
    {
        if (frequency > 0f && amplitude > 0f)
        {
            bobTimer += Time.deltaTime * frequency;
            float bobOffsetY = Mathf.Sin(bobTimer) * amplitude;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * (amplitude * 0.5f);
            transform.localPosition = initialLocalPos + new Vector3(bobOffsetX, bobOffsetY, 0f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 10f);
        }
    }
}
