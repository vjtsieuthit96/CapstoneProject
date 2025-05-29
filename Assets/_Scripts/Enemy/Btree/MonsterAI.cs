using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterAI : MonoBehaviour
{
    protected Node behaviorTree;
    [Header("-----Scripts Component-----")]
    [SerializeField] protected MonsterStats monsterStats;  
    [SerializeField] protected Animator monsterAnimator;
    [SerializeField] protected NavMeshAgent _monsterAgent;    
    [Header("-----Target-----")]
    [SerializeField] protected Transform target;
    [Header("-----FOV-----")]   
    [SerializeField] protected float viewRadius = 15f; // Tầm nhìn tối đa
    [SerializeField] protected float viewAngle = 105f; // Góc nhìn của quái vật

    [Header("-----Attack & Patrol-----")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float patrolRadius;
    void Start()
    {      
        behaviorTree = CreateBehaviorTree();
        InvokeRepeating("EvaluateBehaviorTree", 2f, 2f);
    }
    private void Update()
    {
        float Speed = _monsterAgent.velocity.magnitude;
        SetAnimatorParameter(MonsterAnimatorHash.speedHash, Speed); // Cập nhật tốc độ mượt hơn
    }

    void EvaluateBehaviorTree()
    {
        behaviorTree.Evaluate();
    }

    protected abstract Node CreateBehaviorTree();    

    public float GetViewRadius() => viewRadius;
    public float GetViewAngle() => viewAngle;
    public Transform GetTarget() { return target; }
    public float GetAttackRange() => attackRange;
    public Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position; // Nếu tìm thấy vị trí hợp lệ trên NavMesh
        }

        return transform.position; // Nếu không tìm thấy, quay lại vị trí hiện tại
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
            default:
                monsterAnimator.SetTrigger(hash);
                break;
        }
    }
    public bool GetAnimatorParameter(int hash)
    {
        return monsterAnimator.GetBool(hash); // Lấy giá trị từ Animator
    }
}