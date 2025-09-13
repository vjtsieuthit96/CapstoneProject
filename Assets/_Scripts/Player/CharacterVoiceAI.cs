using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum VoiceState
{
    Normal,
    Excited,
    Frustrated
}

[RequireComponent(typeof(AudioSource))]
public class CharacterVoiceAI : MonoBehaviour
{
    [Header("Voice Clips by State")]
    public List<AudioClip> normalClips;
    public List<AudioClip> excitedClips;
    public List<AudioClip> frustratedClips;

    [Header("Settings")]
    [Tooltip("Khoảng delay tối thiểu giữa 2 voice (giây)")]
    public float minDelay = 60f;

    [Tooltip("Khoảng delay tối đa giữa 2 voice (giây)")]
    public float maxDelay = 120f;

    private AudioSource audioSource;

    [SerializeField] private VoiceState currentState = VoiceState.Normal;
    private Coroutine playRoutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // tắt để không tự phát
        audioSource.loop = false;        // tắt loop vì dùng PlayOneShot
    }

    void Start()
    {
        // Luôn khởi động routine ngay khi bắt đầu
        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayVoiceRoutine());
    }

    public void SetState(VoiceState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("Voice state changed to: " + currentState);

        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayVoiceRoutine());
    }

    private IEnumerator PlayVoiceRoutine()
    {
        Debug.Log("VoiceRoutine started for state: " + currentState);

        // Lần đầu chờ ngắn để test (2–5 giây)
        float firstDelay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(firstDelay);

        while (true)
        {
            AudioClip clip = GetRandomClipForState(currentState);
            if (clip != null)
            {
                Debug.Log("Playing voice: " + clip.name);
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning("No clips assigned for state: " + currentState);
            }

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private AudioClip GetRandomClipForState(VoiceState state)
    {
        List<AudioClip> clips = null;

        switch (state)
        {
            case VoiceState.Normal:
                clips = normalClips;
                break;
            case VoiceState.Excited:
                clips = excitedClips;
                break;
            case VoiceState.Frustrated:
                clips = frustratedClips;
                break;
        }

        if (clips == null || clips.Count == 0) return null;

        int index = Random.Range(0, clips.Count);
        return clips[index];
    }
}
