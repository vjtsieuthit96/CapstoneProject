using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;

    [SerializeField] private AudioSource musicSource;
    public AudioSource MusicSource
    {
        get { return musicSource; }
    }
    [SerializeField] private AudioSource sfxSource;
    public AudioSource SFXSource
    {
        get { return sfxSource; }
    }
    [SerializeField] private AudioSource voiceSource;
    public AudioSource VoiceSource
    {
        get { return voiceSource; }
    }
    public static SoundMixerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }
    public void SetVoiceVolume(float level)
    {
        audioMixer.SetFloat("voiceVolume", Mathf.Log10(level) * 20f);
    }

    public void PlaytMusicAudio(AudioClip audio)
    {
        musicSource.clip = audio;
        musicSource.Play();
    }

    public void PlaySFXAudio(AudioClip audio)
    {
        sfxSource.PlayOneShot(audio);
    }

    public void PlayVoiceAudio(AudioClip audio)
    {
        voiceSource.clip = audio;
        voiceSource.Play();
    }
}
