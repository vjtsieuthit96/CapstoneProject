using UnityEngine;

public class EmotionSystem : MonoBehaviour
{
    [Header("Emotion Settings")]
    [Range(1f, 10f)] public float emotion = 5f;
    public float decayRate = 0.1f;
    public float minValue = 1f;
    public float maxValue = 10f;

    private bool lowHealthPenaltyApplied = false;
    private bool lowStaminaPenaltyApplied = false;


    private CharacterVoiceAI voiceAI;

    void Awake()
    {
        voiceAI = GetComponent<CharacterVoiceAI>();
    }

    void Update()
    {
        if (emotion > 5f)
            emotion = Mathf.Max(5f, emotion - decayRate * Time.deltaTime);
        else if (emotion < 5f)
            emotion = Mathf.Min(5f, emotion + decayRate * Time.deltaTime);

        emotion = Mathf.Clamp(emotion, minValue, maxValue);

        UpdateVoiceState();
    }

    private void UpdateVoiceState()
    {
        VoiceState newState;

        if (emotion <= 4f)
            newState = VoiceState.Frustrated;
        else if (emotion >= 7f)
            newState = VoiceState.Excited;
        else
            newState = VoiceState.Normal;

        if (voiceAI != null)
        {
            voiceAI.SetState(newState);
        }
    }

    public void OnKillEnemy(float point) => AddEmotionPoint(point);
    public void OnCollectItem(float point) => AddEmotionPoint(point);
    public void OnFinishMission(float point) => AddEmotionPoint(point);
    public void OnHitEnemy(float point) => AddEmotionPoint(point);

    public void OnHitByEnemy(float point) => SubtractEmotionPoint(1f);
    public void OnDecreaseHealth(float amount) => SubtractEmotionPoint(amount * 0.1f);
    public void OnOutOfEndurance(float point) => SubtractEmotionPoint(1.5f);
    public void OnLowHealthOnce(float point)
    {
        if (!lowHealthPenaltyApplied)
        {
            SubtractEmotionPoint(point);
            lowHealthPenaltyApplied = true;
        }
    }
    public void OnHealthRecovery()
    {
        lowHealthPenaltyApplied = false;
    }
    public void OnLowStaminaOnce(float point)
    {
        if (!lowStaminaPenaltyApplied)
        {
            SubtractEmotionPoint(point);
            lowStaminaPenaltyApplied = true;
        }
    }
    public void OnStaminaRecovery()
    {
        lowStaminaPenaltyApplied = false;
    }

    public void AddEmotionPoint(float value)
    {
        Debug.Log("Emotion Point Increase: " + value);
        emotion = Mathf.Clamp(emotion + value, minValue, maxValue);
    }

    public void SubtractEmotionPoint(float value)
    {
        Debug.Log("Emotion Point Decrease: " + value);

        emotion = Mathf.Clamp(emotion - value, minValue, maxValue);
    }
}
