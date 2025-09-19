using UnityEngine;

public class FlameBreathCollision : MonoBehaviour
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
            NegativeEffect negativeEffect = other.GetComponent<NegativeEffect>();
            if (player != null)
            {
                player.TakeDamage(monsterStats.GetCurrentDamage()*0.01f); 
                negativeEffect.ApplyBurn(player.PlayerMaxHealth*0.01f, 5f);
                isDamaging = false;
            }
        }
    }    
    public void SetStats(MonsterStats stats)
    {
        monsterStats = stats;
    }
}



