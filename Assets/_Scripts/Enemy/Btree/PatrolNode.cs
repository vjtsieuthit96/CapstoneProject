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

        if (Vector3.Distance(monster.transform.position, patrolTarget) < 1f)
        {
            SetNewPatrolTarget(); // Nếu đến vị trí tuần tra, chọn vị trí mới
        }

        agent.SetDestination(patrolTarget);        

        Debug.Log("Orc đang tuần tra...");
        return NodeState.RUNNING;
    }

    private void SetNewPatrolTarget()
    {
        patrolTarget = monster.GetRandomPatrolPoint(); // Lấy điểm tuần tra ngẫu nhiên
    }
}