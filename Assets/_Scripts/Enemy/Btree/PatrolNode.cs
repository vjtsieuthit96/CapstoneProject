using UnityEngine.AI;
using UnityEngine;

public class PatrolNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;
    private Vector3 patrolCenter;
    private Vector3 patrolTarget;
    private float patrolRadius = 10f;

    public PatrolNode(MonsterAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent;
        UpdatePatrolCenter();  
        SetNewPatrolTarget();
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

        if (Vector3.Distance(monster.transform.position, patrolTarget) < 2f)
        {
            SetNewPatrolTarget();
        }

        agent.SetDestination(patrolTarget);
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