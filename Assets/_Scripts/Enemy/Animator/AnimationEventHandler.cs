using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private MonsterAudio monsterAudio;    

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
        }
    }

    public void DWC() //Disable Weapon Collider
    {
        if (weaponCollider)
        {
            weaponCollider.enabled = false;
            
        }
    }

    #region SOUND 
    public void PlayFootstep()
    {
        monsterAudio.PlayFootStep();
    }
    public void PlayATK1()
    {
        monsterAudio.PlaySFX(monsterAudio.atkSound_1);
    }
    public void PlayATK2()
    {
        monsterAudio.PlaySFX(monsterAudio.atkSound_2);
    }
  
    #endregion
}