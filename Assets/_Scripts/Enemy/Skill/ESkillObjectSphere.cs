using UnityEngine;

public abstract class ESkillObjectSphere : MonoBehaviour
{
    [SerializeField] protected bool AbleHit;
    [SerializeField] protected float DestroyTime;
    [SerializeField] private float damageMultiplier = 1.0f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float sphereRadius = 5.0f;
    [SerializeField] private float positionOffset = -3f;

    protected MonsterStats monsterStats;
    private bool hasHit = false;

    private void OnEnable()
    {
        hasHit = false;
        
        Invoke(nameof(ReturnObject), DestroyTime); // Gọi nổ sau khoảng thời gian tồn tại
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

                CharacterConfigurator player = col.GetComponent<CharacterConfigurator>();
                if (player != null)
                {
                    float damage = monsterStats.GetCurrentDamage() * damageMultiplier;
                    player.TakeDamage(damage);

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
   
    protected abstract void HitObj(RaycastHit hit);
    protected abstract void ReturnObject();

    public void SetStats(MonsterStats stats)
    {
        this.monsterStats = stats;
    }
}