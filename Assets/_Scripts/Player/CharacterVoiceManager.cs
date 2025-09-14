using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterVoiceManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip currentClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
    }

    public void PlayVoice(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (audioSource.isPlaying && currentClip == clip)
            return;

        audioSource.Stop();
        currentClip = clip;
        audioSource.clip = currentClip;
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.loop = false;
        audioSource.Play();
    }

    public void StopVoice()
    {
        audioSource.Stop();
        currentClip = null;
    }
}
