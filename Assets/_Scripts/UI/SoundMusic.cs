using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SoundMusic : MonoBehaviour
{
    [SerializeField] AudioClip musicAudio;

    private void Start()
    {
        SoundMixerManager.Instance.PlaytMusicAudio(musicAudio);

    }

   
}
