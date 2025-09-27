using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent; 
    public ChaseNode(MonsterAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent;  
    }

    public override NodeState Evaluate()
    {
        if (!agent.enabled) return NodeState.FAILURE;

        Transform player = monster.GetTarget();
        if (player == null)
        {
            agent.speed = monster.GetBaseSpeed(); // Nếu không có mục tiêu, khôi phục tốc độ ban đầu
            return NodeState.FAILURE;
        }
        float distanceToPlayer = Vector3.Distance(monster.transform.position, player.position);

        agent.SetDestination(player.position); // Đặt mục tiêu cho NavMeshAgent       
       
        agent.speed = monster.GetBaseSpeed() * monster.GetSpeedMultiplier(); // Khi đuổi, tăng tốc độ

        if (distanceToPlayer <= agent.stoppingDistance)
        {
            agent.speed = monster.GetBaseSpeed(); // Đến gần, quay về tốc độ bình thường
            monster.SetAnimatorParameter(MonsterAnimatorHash.nAttackHash, null); // Kích hoạt hành động tấn công
            return NodeState.SUCCESS;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            monster.SetState(MonsterAI.EnemyState.Chase);
            return NodeState.RUNNING;
        }

        agent.speed = monster.GetBaseSpeed(); // Nếu không còn truy đuổi, khôi phục tốc độ mặc định
        return NodeState.FAILURE;
    }
}