using Unity.VisualScripting;
using UnityEngine;

public class FireBallMove : MonoBehaviour    
{
    [SerializeField] private FireBallHit hitPrefabs;
    [SerializeField] private float MoveSpeed = 10;
    private Vector3 playerTarget;
    private MonsterStats monsterStats;
    [SerializeField] private float damageMultiplier = 1.0f;
    [SerializeField] private float DestroyTime = 30f;
    
    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnObject), DestroyTime);       
    }
    private void Update()
    {
        MovingStyle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterConfigurator player = other.GetComponent<CharacterConfigurator>();
            if (player != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                HitObj(hitPoint);
                //float damage = monsterStats.GetCurrentDamage() * damageMultiplier;
                //player.TakeDamage(damage);
                PoolManager.Instance.GetObject<BloodEffect4>("BloodEF4", hitPoint, Quaternion.identity);
                ReturnObject();
            }
        }
        if (other.CompareTag("Ground"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            HitObj(hitPoint);
            ReturnObject();
        }
    }
   

    private void HitObj(Vector3 hit)
    {        
        FireBallHit fireBallHit = PoolManager.Instance.GetObject<FireBallHit>("FireBallHit", hit, Quaternion.LookRotation(hit));     
        fireBallHit.SetStats(monsterStats);
    }   
    private void ReturnObject()
    {
        PoolManager.Instance.ReturnObject("FireBall", this);
    }

    private void MovingStyle()
    {
        Vector3 direction = (playerTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);        
        float tiltAngle = 45f; // Độ nghiêng mong muốn
        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);
        transform.rotation = tiltRotation;
        transform.position = Vector3.MoveTowards(transform.position, playerTarget, MoveSpeed * Time.deltaTime);
       
    }
    public void GetTarget(Transform target)
    {
        playerTarget = target.position;
    }
    public void SetStats(MonsterStats stats)
    {
        this.monsterStats = stats;
    }
}
