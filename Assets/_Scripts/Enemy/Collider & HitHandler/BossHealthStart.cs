using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private Canvas bossCanvnas;
    [SerializeField] private BossOrgeAI bossOrgeAI;
    [SerializeField] private MonsterStats monsterStats;
    private bool isActive = false;

    private float checkTimer = 0f;
    private float checkInterval = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (!bossOrgeAI.isDead)
        {
            if (other.CompareTag("Player"))
            {
                bossCanvnas.enabled = true;
                bossHealthBar.HealthBarOn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossCanvnas.enabled = false;
        }
    }

    private void Update()
    {
        checkTimer += Time.unscaledDeltaTime;

        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            if (monsterStats.GetCurrentHealth() < monsterStats.GetMaxHealth() && !isActive)
            {
                isActive = true;
                bossCanvnas.enabled = true;
                bossHealthBar.HealthBarOn();
            }
        }
    }
}
