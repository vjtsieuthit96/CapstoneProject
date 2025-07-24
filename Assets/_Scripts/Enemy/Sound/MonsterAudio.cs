using UnityEngine;
public class MonsterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource monsterAudio;
    [Header("Footstep Or Wings")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] wingsSounds;
    [Header("Roar")]
    [SerializeField] private AudioClip[] roarSounds;
    [Header("AttackSound")]
    public AudioClip[] atkSound;
    [Header("Get Hit")]
    [SerializeField] private AudioClip[] getHitSounds;

    [Header("Other Sounds")]
    [SerializeField] private AudioClip[] otherSounds;

    #region Foot&Wings Sounds
    public void PlayFootStep()
    {
        if (monsterAudio && footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            monsterAudio.PlayOneShot(footstepSounds[randomIndex]);
        }
    }
    public void PlayWingsSound()
    {
        if (wingsSounds.Length == 0) return;
        else 
        {
            int randomIndex = Random.Range(0, wingsSounds.Length);
            monsterAudio.PlayOneShot(wingsSounds[randomIndex]);
        }
    }
    public void PlayWingsSelected(int index)
    {
        if (wingsSounds.Length > index && index >= 0)
        {
            monsterAudio.PlayOneShot(wingsSounds[index]);
        }
    }
    #endregion
    #region Attack & Get Hit Sounds

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
    public void PlayRandomAttackSound()
    {
        if (monsterAudio && atkSound.Length > 0)
        {
            int randomIndex = Random.Range(0, atkSound.Length);
            monsterAudio.PlayOneShot(atkSound[randomIndex]);
        }
    }
    
    public void PlayHurt()
    {         
        if (monsterAudio && getHitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, getHitSounds.Length);
            monsterAudio.PlayOneShot(getHitSounds[randomIndex]);
        }
    }
    #endregion
    #region Other Sounds
    public void PlayOtherSound(int index)
    {
        if (otherSounds.Length > index && index >= 0)
        {
            monsterAudio.PlayOneShot(otherSounds[index]);
        }
    }
    public void PlayRandomOtherSound()
    {
        if (otherSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, otherSounds.Length);
            monsterAudio.PlayOneShot(otherSounds[randomIndex]);
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        monsterAudio.PlayOneShot(clip);
    }
    #endregion
}
