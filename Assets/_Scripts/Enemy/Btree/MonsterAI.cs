using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public abstract class MonsterAI : MonoBehaviour
{
    [Header("-----Target-----")]
    [SerializeField] protected Transform target;
    [Header("-----Speed Multiplier-----")]
    [SerializeField] float speedMultiplier = 1.75f;
    [Header("-----FOV-----")]
    [SerializeField] protected float viewRadius = 15f; // Tầm nhìn tối đa
    [SerializeField] protected float viewAngle = 105f; // Góc nhìn của quái vật
    [SerializeField] protected float alertRadius = 10f;
    [Header("-----Attack & Patrol-----")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float patrolRadius;
    [SerializeField] protected float baseSpeed;
    [Header("-----Negative Effects-----")]
    [SerializeField] protected GameObject frozenEffect;
    [SerializeField] protected GameObject ElectricEffect;
    [SerializeField] protected BloodEffect5 bloodEffect;
    [Header("-----Components-----")]
    [SerializeField] protected MonsterStats monsterStats;
    [SerializeField] protected Animator monsterAnimator;
    [SerializeField] protected NavMeshAgent monsterAgent;
    [SerializeField] protected SkillManager skillManager;
    [SerializeField] protected MonsterAudio monsterAudio;
    [Header("-----Player Kill Log-----")]
    private GameObject lastAttacker = null;
    private Dictionary<GameObject, float> damageLog = new Dictionary<GameObject, float>();
    [SerializeField] private string enemyType = "Orc";
    public string GetEnemyType() => enemyType;

    protected Node behaviorTree;
    private Vector3 _patrolCenter;
    private ItemDropper itemDropper;

    private bool hasRetreat = false;
    [SerializeField] public bool isDead = false;
    private bool isHit = false;
    private bool isFreeze = false;
    private bool isSlowDown = false;
    private bool isShocked = false;
    private bool isInCombat;

    [SerializeField] private float returnToPoolDelay = 6f;

    public EnemyData enemyData;
    protected virtual void Start()
    {
        enemyType = monsterStats.enemyType;
        PoolManager.Instance.CreatePool<BloodEffect5>("BloodEF5", bloodEffect, 50);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        baseSpeed = monsterAgent.speed;
        _patrolCenter = transform.position;
        behaviorTree = CreateBehaviorTree();
        itemDropper = GetComponent<ItemDropper>();
        enemyData = GetComponent<EnemyData>();

    }
    protected virtual void Update()
    {
        GroundLocomotion();
        Die();
    }
    private void OnEnable()
    {
        isDead = false;
        isHit = false;
        isFreeze = false;
        isSlowDown = false;
        isShocked = false;
        monsterAgent.isStopped = false;
        monsterAnimator.speed = 1f;
        damageLog.Clear();
        lastAttacker = null;
        isInCombat = false;
        hasRetreat = false;
    }

    public void Die()
    {
        if (!isDead && monsterStats.GetCurrentHealth() <= 0)
        {
            isDead = true;
            monsterAgent.isStopped = true;
            itemDropper.TryDropItem();
            SetAnimatorParameter(MonsterAnimatorHash.isDeadHash, true);
            Debug.Log("<color=red>--- Enemy Damage Report ---</color>");
            float totalDamage = 0f;
            foreach (var entry in damageLog)
            {
                totalDamage += entry.Value;
            }

            foreach (var entry in damageLog)
            {
                float percent = totalDamage > 0 ? (entry.Value / totalDamage) * 100f : 0f;
                Debug.Log($"{entry.Key.name} dealt {entry.Value:F1} damage ({percent:F1}%)");
            }

            if (lastAttacker != null)
            {
                Debug.Log($"<color=green>Final blow by: {lastAttacker.name}</color>");
                PlayerPlayRecords playerPlayRecords = lastAttacker.GetComponent<PlayerPlayRecords>();
                string enemyType = GetEnemyType();
                playerPlayRecords.RegisterKill(enemyType);
            }
            else
            {
                Debug.Log("Enemy died with unknown killer.");
            }
            StartCoroutine(ReturnToPoolAfterDelay());
        }
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(returnToPoolDelay);
        MonsterFactory.Instance.ReturnEnemy(enemyData, gameObject);
    }

    public void RegisterDamage(GameObject attacker, float damage)
    {
        if (attacker == null || isDead) return;

        lastAttacker = attacker;

        if (!damageLog.ContainsKey(attacker))
            damageLog[attacker] = 0f;

        damageLog[attacker] += damage;
    }
    #region BEHAVIOR
    public void EvaluateBehaviorTree()
    {
        if (!isDead && !isFreeze)
            behaviorTree.Evaluate();
    }
    public void RepeatEvaluateBehaviorTree(float time, float repeatRate)
    {
        InvokeRepeating("EvaluateBehaviorTree", time, repeatRate);
    }

    protected abstract Node CreateBehaviorTree();
    private void GroundLocomotion()
    {
        float Speed = monsterAgent.velocity.magnitude;
        SetAnimatorParameter(MonsterAnimatorHash.speedHash, Speed);
       
        float normalizedSpeed = Speed / monsterAgent.speed; 
        normalizedSpeed = Mathf.Clamp(normalizedSpeed, 0f, 1f); // Giới hạn từ 0 -> 1
      
        float locomotionValue = Vector3.Dot(monsterAgent.velocity.normalized, transform.forward) * normalizedSpeed;

        //Điều chỉnh giá trị về khoảng -1 -> 1
        locomotionValue = Mathf.Lerp(-1f, 1f, Mathf.Clamp01((locomotionValue + 1) / 2));
       
        SetAnimatorParameter(MonsterAnimatorHash.locomotionHash, locomotionValue);
    }
    public void ApplyDamage(float amount)
    {
        monsterStats.TakeDamage(amount);
        GetBehaviorNode<CheckPlayerInFOVNode>()?.OnAttacked();
    }
    public void FreezyEnemy(float duration)
    {
        if (!isFreeze && !isDead)
        {
            isFreeze = true;
            monsterAgent.isStopped = true;
            monsterAnimator.speed = 0;            
            frozenEffect.SetActive(true);
            Invoke(nameof(UnFreezeEnemy), duration);
        }
    }
    public void UnFreezeEnemy()
    {
        frozenEffect.SetActive(false);
        monsterAnimator.speed = 1;
        monsterAgent.isStopped = false;
        isFreeze = false;
    }
    public void SlowDown(float percent, float duration)
    {
        if(!isDead && !isSlowDown)
        {
            isSlowDown = true;
            monsterAnimator.speed -= percent;
            Invoke(nameof(DisableSlowDown), duration);
        }
    }
    public void DisableSlowDown()
    {
        monsterAnimator.speed = 1;
        isSlowDown = false;
    }
    public void BleedEffect(Vector3 position)
    {
        PoolManager.Instance.GetObject<BloodEffect5>("BloodEF5",position,Quaternion.identity);
    }
    public void ShockEffect(float duration)
    {
        if (!isShocked && !isDead)
        {
            isShocked = true;
            //monsterAnimator.SetFloat(name: "TakeShock", duration * 5);
            ElectricEffect.SetActive(true);
            ParticleSystem electricParticle = ElectricEffect.GetComponent<ParticleSystem>();
            electricParticle.Play();
            ParticleSystem.MainModule mainModule = electricParticle.main;
            mainModule.duration = duration;
            StartCoroutine(ShockLoop(duration));            
        }
    }
    private IEnumerator ShockLoop(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            SetAnimatorParameter(MonsterAnimatorHash.takeHitHash, null);
            //Debug.Log(message: "Enemy bị giật nè");
            yield return new WaitForSeconds(0.75f);
            timer += 0.75f;
        }
        isShocked = false;
        ElectricEffect.SetActive(false);
        //monsterAnimator.SetFloat("TakeShock", 1);
        //Debug.Log("Enemy bị giật xong rồi");
    }       
    #endregion

    #region GET & SET
    public bool IsInCombat() => isInCombat;
    public void SetInCombat(bool value)
    {
        isInCombat = value;       
    }
    public float GetViewRadius() => viewRadius;
    public float GetViewAngle() => viewAngle;
    public float GetAlertRadius()=> alertRadius;    
    public T GetBehaviorNode<T>() where T : Node
    {
        return behaviorTree.FindNode<T>(); 
    }

    public Transform GetTarget() { return target; }
    public float GetAttackRange() => attackRange;
    public float GetStoppingDistance() => monsterAgent.stoppingDistance;
    public Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // Random vị trí trong bán kính tuần tra
        randomDirection += _patrolCenter; // Giữ AI di chuyển quanh khu vực trung tâm

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position; // Nếu vị trí hợp lệ, sử dụng nó
        }

        return _patrolCenter; // Nếu không tìm thấy vị trí hợp lệ, quay lại trung tâm tuần tra
    }
    public float GetPatrolRadius() => patrolRadius;
    public Vector3 GetPatrolCenter() => _patrolCenter;
    public void SetNewPatrolCenter(Vector3 position)
    {
        _patrolCenter = position;
    }
    public void SetNavMeshStop(bool value)
    {
        monsterAgent.isStopped = value;
    }
    public bool GetIsHit() => isHit;
    public void SetIsHit (bool value)
    {
        isHit = value;
    }
    public float GetBaseSpeed() => baseSpeed;
    public float GetSpeedMultiplier()=>speedMultiplier;
    public bool GetHasRetreat() => hasRetreat;
    public void SetHasRetreat(bool value)
    {
        hasRetreat = value;
    }
    public void SetAnimatorParameter(int hash, object value)
    {
        if (monsterAnimator == null)
        {
            Debug.Log("Missing Monster Animator");
            return;
        }
        switch (value)
        {
            case bool boolValue:
                monsterAnimator.SetBool(hash, boolValue); 
                break;
            case float floatValue:
                monsterAnimator.SetFloat(hash, floatValue);
                break;
            case null:
                monsterAnimator.SetTrigger(hash);
                break;
            default:
                break;
        }
    }
    public bool GetBoolAnimatorParameter(int hash)
    {
        return monsterAnimator.GetBool(hash); // Lấy giá trị từ Animator
    }
    public float GetFloatAnimatorParameter(int hash)
    {
        return monsterAnimator.GetFloat(hash);
    }
    #endregion    

    #region DRAW FOV
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;

        // Vẽ cung tròn thể hiện góc nhìn từ hai điểm cuối
        Handles.DrawWireArc(transform.position, Vector3.up,
                            DirectionFromAngle(transform.eulerAngles.y, -viewAngle / 2),
                            viewAngle, viewRadius);

        // Vẽ hai đường chỉ hướng góc nhìn
        Vector3 viewAngleA = DirectionFromAngle(transform.eulerAngles.y, -viewAngle / 2);
        Vector3 viewAngleB = DirectionFromAngle(transform.eulerAngles.y, viewAngle / 2);

        Handles.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        float rad = (eulerY + angleInDegrees) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }
    #endregion
}
