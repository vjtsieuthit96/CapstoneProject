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
    [SerializeField] protected BloodEffect4 bloodEffect;
    [Header("-----Components-----")]    
    [SerializeField] protected MonsterStats monsterStats;
    [SerializeField] protected Animator monsterAnimator;
    [SerializeField] protected NavMeshAgent monsterAgent;
    [SerializeField] protected SkillManager skillManager;
    [SerializeField] protected MonsterAudio monsterAudio;
    protected Node behaviorTree;

    private Vector3 _patrolCenter;  
    private bool hasRetreat = false;
    private bool isDead = false;
    private bool isHit = false;
    private bool isFreeze = false;
      protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        baseSpeed = monsterAgent.speed;
        _patrolCenter = transform.position;
        behaviorTree = CreateBehaviorTree();      
        InvokeRepeating("EvaluateBehaviorTree", 0f, 1.5f);
    }
    protected virtual void Update()
    {
        float Speed = monsterAgent.velocity.magnitude;
        SetAnimatorParameter(MonsterAnimatorHash.speedHash, Speed);    
        if (!isDead && monsterStats.GetCurrentHealth()<=0)
        {
            isDead = true;
            SetAnimatorParameter(MonsterAnimatorHash.isDeadHash, true);
            monsterAgent.isStopped = true;
        }
    }
    #region Behavior

    public void EvaluateBehaviorTree()
    {
        if (!isDead && !isFreeze)
            behaviorTree.Evaluate();       
    }       
   
    protected abstract Node CreateBehaviorTree();
    public void ApplyDamage(float amount)
    {
        monsterStats.TakeDamage(amount);        
        GetBehaviorNode<CheckPlayerInFOVNode>()?.OnAttacked();
    }

    public void FreezyEnemy(float duration)
    {
        if (!isFreeze)
        {
            isFreeze = true;
            monsterAnimator.speed = 0;
            monsterAgent.isStopped = true;
            frozenEffect.SetActive(true);
            Debug.Log("Enemy Freeze");
            Invoke(nameof(UnFreezeEnemy), duration);
        }
    }
    public void UnFreezeEnemy()
    {
        frozenEffect.SetActive(false);
        monsterAnimator.speed = 1;
        monsterAgent.isStopped = false;
        isFreeze = false;
        Debug.Log("enemy UnFreeze");
    }

    public void BleedEffect(Vector3 position)
    {
        PoolManager.Instance.GetObject<BloodEffect4>("BloodEF4",position,Quaternion.identity);
    }
    #endregion

    #region GET & SET
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
    public bool GetAnimatorParameter(int hash)
    {
        return monsterAnimator.GetBool(hash); // Lấy giá trị từ Animator
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
