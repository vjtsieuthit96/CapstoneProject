using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;
    private Vector3 patrolCenter;
    private Vector3 patrolTarget;
    private float patrolRadius = 10f;

    private float stuckTimer = 0f;
    private Vector3 lastPosition;

    public PatrolNode(MonsterAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent;
        UpdatePatrolCenter();
        SetNewPatrolTarget();
        lastPosition = monster.transform.position;
    }

    public override NodeState Evaluate()
    {
        UpdatePatrolCenter();

        if (monster.GetBoolAnimatorParameter(MonsterAnimatorHash.isBattleHash))
        {
            Debug.Log("Dừng tuần tra! Chuyển sang trạng thái chiến đấu.");
            return NodeState.FAILURE;
        }

        agent.speed = monster.GetBaseSpeed();

        // Nếu đến gần mục tiêu → chọn mục tiêu mới
        if (Vector3.Distance(monster.transform.position, patrolTarget) < agent.stoppingDistance * 1.25f)
        {
            SetNewPatrolTarget();
        }

        agent.SetDestination(patrolTarget);

        // Kiểm tra nếu không có đường đi hợp lệ
        if (!agent.hasPath || agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Không có đường đi hợp lệ, đổi mục tiêu tuần tra.");
            SetNewPatrolTarget();
        }

        // Kiểm tra nếu bị kẹt (đứng yên quá lâu)
        if (Vector3.Distance(monster.transform.position, lastPosition) < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2f)
            {
                Debug.Log("Agent bị kẹt, đổi mục tiêu.");
                SetNewPatrolTarget();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = monster.transform.position;

        return NodeState.RUNNING;
    }

    private void UpdatePatrolCenter()
    {
        patrolCenter = monster.GetPatrolCenter();
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += patrolCenter;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
        }
        else
        {
            patrolTarget = patrolCenter;
        }
    }
}