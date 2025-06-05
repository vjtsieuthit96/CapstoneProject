using UnityEngine;

public class WeaponColliderManager : MonoBehaviour
{
    [SerializeField] private MonsterStats monsterStats;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Take Hit");            
            CharacterConfigurator player = other.GetComponent<CharacterConfigurator>();
            player.TakeDamage(monsterStats.GetCurrentDamage());
        }
    }
}
