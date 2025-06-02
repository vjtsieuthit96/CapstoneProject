using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;
    private Vector3 patrolCenter; // Khu vực tuần tra chính
    private Vector3 patrolTarget;
    private float patrolRadius = 10f; // Bán kính tuần tra xung quanh trung tâm

    public PatrolNode(MonsterAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent;
        this.patrolCenter = monster.GetPatrolCenter(); // Lấy vị trí ban đầu làm trung tâm tuần tra
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
            SetNewPatrolTarget(); // Nếu đến vị trí tuần tra, chọn vị trí mới quanh `patrolCenter`
        }

        agent.SetDestination(patrolTarget);

        Debug.Log("Orc đang tuần tra quanh khu vực ban đầu...");
        return NodeState.RUNNING;
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // Chọn vị trí ngẫu nhiên trong phạm vi tuần tra
        randomDirection += patrolCenter; // Giữ AI tuần tra quanh trung tâm

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position; // Nếu vị trí hợp lệ, dùng nó
            Debug.Log($"Chọn vị trí tuần tra hợp lệ: {patrolTarget}");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy vị trí hợp lệ! Quay về trung tâm...");
            patrolTarget = patrolCenter; // Nếu không tìm thấy vị trí, AI tuần tra ngay trung tâm
        }
    }
}