using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Collider[] weaponColliders;
    [SerializeField] private MonsterAudio monsterAudio;
    [SerializeField] private MonsterAI monsterAI;

    private void Start()
    {
        if (weaponColliders == null)
        {
            Debug.LogWarning("Chưa gán WeaponColliders!");
        }
        else
        {
            foreach (var col in weaponColliders)
            {
                if (col) col.enabled = false;
            }
        }
    }
    #region Weapon Collider 

    public void EWC() // Enable default collider at index 0
    {
        EWC(0);
    }

    public void EWC(int index) // Enable specific collider
    {
        if (index >= 0 && index < weaponColliders.Length && weaponColliders[index])
        {
            weaponColliders[index].enabled = true;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy WeaponCollider ở index {index}");
        }
    }

    public void DWC() // Disable default collider at index 0
    {
        DWC(0);
    }

    public void DWC(int index) // Disable specific collider
    {
        if (index >= 0 && index < weaponColliders.Length && weaponColliders[index])
        {
            weaponColliders[index].enabled = false;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy WeaponCollider ở index {index}");
        }
    }
    #endregion

    #region Movement
    public void DisableMoving()
    {
        monsterAI.SetNavMeshStop(true);
    }

    public void EnableMoving()
    {
        monsterAI.SetNavMeshStop(false);
    }
    public void StartGetHit()
    {
        monsterAI.SetIsHit(true);
        monsterAI.SetNavMeshStop(true);
    }

    public void EndGetHit()
    {
        monsterAI.SetIsHit(true);
        monsterAI.SetNavMeshStop(false);
    }
    #endregion

    #region SOUND 
    public void PlayFootstep()
    {
        monsterAudio.PlayFootStep();
    }
    public void PlayATK1()
    {
        monsterAudio.PlaySFX(monsterAudio.atkSound[0]);
    }
    public void PlayATK2()
    {
        monsterAudio.PlaySFX(monsterAudio.atkSound[1]);
    }
  
    #endregion
}