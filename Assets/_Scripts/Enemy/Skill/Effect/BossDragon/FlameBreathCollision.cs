using UnityEngine;

public class FlameBreathCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    private MonsterStats monsterStats;
    
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Flame Breath hit Player");
            CharacterConfigurator player = other.GetComponent<CharacterConfigurator>();
            if (player != null)
            {
                player.TakeDamage(monsterStats.GetCurrentDamage()*0.01f); // Adjust damage value as needed
            }
        }
    }    
    public void SetStats(MonsterStats stats)
    {
        monsterStats = stats;
    }
}



