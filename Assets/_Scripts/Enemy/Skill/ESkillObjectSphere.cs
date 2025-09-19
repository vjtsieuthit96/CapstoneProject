using UnityEngine;

public abstract class ESkillObjectSphere : MonoBehaviour
{
    [SerializeField] protected bool AbleHit;
    [SerializeField] protected float DestroyTime;
    [SerializeField] private float damageMultiplier = 1.0f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float sphereRadius = 5.0f;
    [SerializeField] private float positionOffset = -3f;
    [SerializeField] private int negativeEffectRate;

    protected MonsterStats monsterStats;
    private bool hasHit = false;

    private void OnEnable()
    {
        CancelInvoke();
        hasHit = false;        
        Invoke(nameof(ReturnObject), DestroyTime); // trả về pool sau khoảng thời gian tồn tại
    }
    void Update()
    {
        if (!AbleHit || hasHit) return;

        Vector3 explosionPos = transform.position + Vector3.up * positionOffset;
        Collider[] hits = Physics.OverlapSphere(explosionPos, sphereRadius, playerLayer);

        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                hasHit = true;
                Debug.Log("Hit player: " + col.name);
                CharacterConfigurator player = col.GetComponent<CharacterConfigurator>();
                NegativeEffect negativeEffect = col.GetComponent<NegativeEffect>();
                if (player != null)
                {
                    float damage = monsterStats.GetCurrentDamage() * damageMultiplier;
                    Debug.Log("Damage dealt: " + damage);
                    player.TakeDamage(damage);
                    int rate = Random.Range(0, 100);
                    if (rate < negativeEffectRate)
                    {
                        negativeEffect.ApplyBurn(player.PlayerMaxHealth * 0.01f, 5f);
                    }
                    // Gọi HitObj với thông tin va chạm tối thiểu
                    RaycastHit fakeHit = new RaycastHit
                    {
                        point = col.ClosestPoint(explosionPos)
                    };
                    HitObj(fakeHit);
                }
            }
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0f, 0.5f, 1f, 0.25f);
    //    Vector3 explosionPos = transform.position + Vector3.up * positionOffset;
    //    Gizmos.DrawSphere(explosionPos, sphereRadius);
    //}

    protected abstract void HitObj(RaycastHit hit);
    protected abstract void ReturnObject();
    
    public void SetStats(MonsterStats stats)
    {
        this.monsterStats = stats;
    }
}