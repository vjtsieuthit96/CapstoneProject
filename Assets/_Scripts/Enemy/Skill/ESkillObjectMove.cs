using UnityEngine;

public abstract class ESkillObjectMove : MonoBehaviour
{
    [SerializeField] protected float MoveSpeed = 10;
    [SerializeField] protected bool AbleHit;
    [SerializeField] protected float MaxLength;
    [SerializeField] protected float DestroyTime;
    [SerializeField] private float damageMultiplier = 1.0f;
    [SerializeField] private LayerMask playerLayer;

    protected MonsterStats monsterStats;
    protected MonsterAI monsterAI;
    private bool hasHit = false;

    protected virtual void Start()
    {
      
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnObject), DestroyTime);
        hasHit = false; // Reset trạng thái để có thể trúng lần sau
    }

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

        if (hasHit) return; // Nếu đã trúng, không cần kiểm tra nữa

        if (AbleHit)
        {
            float spreadAngle = 120f; // Mở rộng góc bắn
            int rayCount = 2; // Số tia cần bắn
            RaycastHit hit;

            for (int i = 0; i < rayCount; i++)
            {
                float angleOffset = spreadAngle * ((i / (float)(rayCount - 1)) - 0.5f);
                Vector3 direction = Quaternion.Euler(0, angleOffset, 0) * transform.forward;

                // Vẽ tia trong Scene để kiểm tra
                Debug.DrawRay(transform.position, direction * MaxLength, Color.red, 0.1f);

                if (Physics.Raycast(transform.position, direction, out hit, MaxLength, playerLayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        hasHit = true; // Đánh dấu đã trúng, ngừng kiểm tra tiếp

                        CharacterConfigurator player = hit.collider.GetComponent<CharacterConfigurator>();
                        if (player != null)
                        {
                            HitObj(hit);
                            float damage = monsterStats.GetCurrentDamage() * damageMultiplier;
                            Debug.Log($"Monster {monsterAI.name} hit Player {player.name} with damage: {damage}");
                            player.TakeDamage(damage);
                            PoolManager.Instance.GetObject<BloodEffect4>("BloodEF4", hit.point, Quaternion.identity);
                            break; // Ngừng kiểm tra tiếp nếu đã trúng
                        }
                    }
                }
            }
        }
    }

    protected abstract void HitObj(RaycastHit hit);
    protected abstract void ReturnObject();

    public void SetStatAndTarget(MonsterStats stats, MonsterAI monsterAI)
    {
        this.monsterStats = stats;
        this.monsterAI = monsterAI;
    }

    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}