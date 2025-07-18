﻿using UnityEngine;
public class MonsterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource monsterAudio;
    [Header("Footstep")]
    [SerializeField] private AudioClip[] footstepSounds;
    [Header("Roar")]
    [SerializeField] private AudioClip[] roarSounds;
    [Header("AttackSound")]
    public AudioClip[] atkSound;   
    
    public void PlayFootStep ()
    {
        if (monsterAudio && footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            monsterAudio.PlayOneShot(footstepSounds[randomIndex]);
        }
    }
    public void PlayRoar()
    {
        if (monsterAudio && roarSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, roarSounds.Length);
            monsterAudio.PlayOneShot(roarSounds[randomIndex]);
        }
    }
    
    public void PlayRoarSelected(int index)
    {
        if (monsterAudio && roarSounds.Length > index && index >= 0)
        {
            monsterAudio.PlayOneShot(roarSounds[index]);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        monsterAudio.PlayOneShot(clip);
    }
}
