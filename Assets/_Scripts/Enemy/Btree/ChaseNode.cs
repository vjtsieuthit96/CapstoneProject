using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;   
    private float chaseMultiplier = 1.5f; // Khi đuổi, tăng 50% tốc độ   

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

        agent.SetDestination(player.position); // Đặt mục tiêu cho NavMeshAgent       
       
        agent.speed = monster.GetBaseSpeed() * chaseMultiplier; // Khi đuổi, tăng 50% tốc độ

        if (Vector3.Distance(monster.transform.position, player.position) <= agent.stoppingDistance)
        {
            Debug.Log("Đã đến vị trí người chơi, khôi phục tốc độ.");
            agent.speed = monster.GetBaseSpeed(); // Đến gần, quay về tốc độ bình thường      
            return NodeState.SUCCESS;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            Debug.Log("Đang đuổi theo người chơi, tăng tốc độ!");
            return NodeState.RUNNING;
        }

        agent.speed = monster.GetBaseSpeed(); // Nếu không còn truy đuổi, khôi phục tốc độ mặc định
        return NodeState.FAILURE;
    }
}