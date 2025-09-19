using UnityEngine;

public class DangerCloseCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    private MonsterStats monsterStats;
    private bool isDamaging = false;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && !isDamaging)
        {           
            isDamaging = true;
            CharacterConfigurator player = other.GetComponent<CharacterConfigurator>();
            if (player != null)
            {
                player.TakeDamage(monsterStats.GetCurrentDamage() * 0.02f);
                isDamaging = false;
            }
        }
    }
    public void SetStats(MonsterStats stats)
    {
        monsterStats = stats;
    }
}
