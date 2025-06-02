using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;
    private Vector3 patrolTarget;

    public PatrolNode(MonsterAI monster,NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent; 
        SetNewPatrolTarget();
    }

    public override NodeState Evaluate()
    {
        if (monster.GetAnimatorParameter(MonsterAnimatorHash.isBattleHash))
        {
            Debug.Log("Dừng tuần tra! Chuyển sang trạng thái chiến đấu.");
            return NodeState.FAILURE; // Thoát khỏi tuần tra để vào chế độ đuổi theo
        }
        agent.speed = monster.GetBaseSpeed();
        if (Vector3.Distance(monster.transform.position, patrolTarget) < 2f)
        {
            SetNewPatrolTarget(); // Nếu đến vị trí tuần tra, chọn vị trí mới
        }

        agent.SetDestination(patrolTarget);        

        Debug.Log("Orc đang tuần tra...");
        return NodeState.RUNNING;
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = monster.GetRandomPatrolPoint(); // Chọn vị trí ngẫu nhiên trong bán kính tuần tra
        randomDirection += monster.transform.position; // Cộng với vị trí hiện tại để tính toán khu vực xung quanh

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, monster.GetPatrolRadius(), NavMesh.AllAreas))
        {
            patrolTarget = hit.position; // Nếu vị trí hợp lệ, dùng nó
            Debug.Log($"Chọn vị trí tuần tra hợp lệ: {patrolTarget}");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy vị trí hợp lệ! Chọn lại...");
            SetNewPatrolTarget(); // Nếu không hợp lệ, thử lại
        }
    }
}