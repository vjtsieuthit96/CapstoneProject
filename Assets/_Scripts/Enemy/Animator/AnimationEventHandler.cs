using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider; // Collider của vũ khí để bật/tắt khi tấn công
    [SerializeField] private AudioSource monsterAudio; // Âm thanh bước chân
    [SerializeField] private AudioClip[] footstepSounds; // Chứa các âm thanh bước chân theo địa hình

    private void Start()
    {    
        if (!weaponCollider) Debug.LogWarning("WeaponCollider chưa được gán!");
        else weaponCollider.enabled = false;     
    }

    public void EWC() // Enable Weapon Collider
    {
        if (weaponCollider)
        {
            weaponCollider.enabled = true;
            Debug.Log("Kích hoạt collider vũ khí!");
        }
    }

    public void DWC() //Disable Weapon Collider
    {
        if (weaponCollider)
        {
            weaponCollider.enabled = false;
            Debug.Log("Tắt collider vũ khí!");
        }
    }

    public void PlayFootstep()
    {
        if (monsterAudio && footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            monsterAudio.PlayOneShot(footstepSounds[randomIndex]);
            Debug.Log("Chạy âm thanh bước chân!");
        }
    }
    
}