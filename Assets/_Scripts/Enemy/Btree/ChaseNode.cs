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
        if (player == null) return NodeState.FAILURE;

        agent.SetDestination(player.position); // Đặt mục tiêu cho NavMeshAgent        

        if (Vector3.Distance(monster.transform.position, player.position) <= agent.stoppingDistance)
        {
            return NodeState.SUCCESS; // Nếu đến gần người chơi, kết thúc đuổi
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            Debug.Log("Đang dùng NavMesh để đuổi theo người chơi!");
            return NodeState.RUNNING;
        }
       
        return NodeState.FAILURE;
    }
}