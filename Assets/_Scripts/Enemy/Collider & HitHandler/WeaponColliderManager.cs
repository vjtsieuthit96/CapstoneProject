using UnityEngine;

public class WeaponColliderManager : MonoBehaviour
{
    [SerializeField] private MonsterStats monsterStats;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Take Hit");            
            //monsterStats.GetCurrentDamage();
        }
    }
}
