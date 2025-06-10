using UnityEngine;

public class WeaponColliderManager : MonoBehaviour
{
    [SerializeField] private MonsterStats monsterStats;
    [SerializeField] private BloodEffect4 bloodPrefabs;   
    
    private void Start()
    {
        PoolManager.Instance.CreatePool("BloodEF4", bloodPrefabs, 50);
    }    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterConfigurator player = other.GetComponent<CharacterConfigurator>();
            if (player != null)
            {
                player.TakeDamage(monsterStats.GetCurrentDamage());
                PoolManager.Instance.GetObject<BloodEffect4>("BloodEF4", transform.position, Quaternion.identity);
            }
            
        }
    }
}
