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
                Debug.Log("Hit player: " + col.name);
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
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.25f); // Màu xanh dương nhạt có alpha

        Vector3 explosionPos = transform.position + Vector3.up * positionOffset;
        Gizmos.DrawSphere(explosionPos, sphereRadius);
    }



    protected abstract void HitObj(RaycastHit hit);
    protected abstract void ReturnObject();

    public void SetStats(MonsterStats stats)
    {
        this.monsterStats = stats;
    }
}