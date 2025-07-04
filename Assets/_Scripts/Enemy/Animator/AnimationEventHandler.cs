using System.Linq;
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

    public void EWC() // Enable Weapon Colliders
    {
        foreach (var col in weaponColliders)
        {
            if (col) col.enabled = true;
        }
    }

    public void DWC() // Disable Weapon Colliders
    {
        foreach (var col in weaponColliders)
        {
            if (col) col.enabled = false;
        }
    }

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