using UnityEngine;
public class HarpySound : MonoBehaviour
{
    [SerializeField] private AudioSource monsterAudio;
    [SerializeField] private AudioClip[] WingSound; 

    public void PlayWings()
    {
        if (monsterAudio && WingSound.Length > 0)
        {
            int randomIndex = Random.Range(0, WingSound.Length);
            monsterAudio.PlayOneShot(WingSound[randomIndex]);
        }
    }
}
