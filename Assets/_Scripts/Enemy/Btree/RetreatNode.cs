using UnityEngine;
using UnityEngine.AI;

public class RetreatNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;
    private float retreatSpeedMultiplier = 1.75f;
    private float safeDistance = 20f;
    private Vector3 escapeTarget;   

    public RetreatNode(MonsterAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        Transform player = monster.GetTarget();
        if (player == null) return NodeState.FAILURE;
        
        monster.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, false);       
        agent.speed *= retreatSpeedMultiplier;              
        
        if (escapeTarget == Vector3.zero || agent.remainingDistance < 2f)
        {
            SetEscapeTarget();
        }

        float currentDistance = Vector3.Distance(monster.transform.position, player.position);

        if (currentDistance >= safeDistance ) 
        {
            ResetSpeed();                
            monster.SetHasRetreat(true);
            monster.SetNewPatrolCenter(monster.transform.position);
            Debug.Log("AI đã retreat thành công! Đánh giá lại cây hành vi...");
            return NodeState.SUCCESS;
        }

        agent.SetDestination(escapeTarget);
        return NodeState.RUNNING;
    }

    private void SetEscapeTarget()
    {
        Transform player = monster.GetTarget();
        Vector3 directionAway = (monster.transform.position - player.position).normalized;
        escapeTarget = monster.transform.position + directionAway * 25f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(escapeTarget, out hit, 25f, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
        }
        else
        {
            escapeTarget = monster.transform.position;
        }
    }

    private void ResetSpeed()
    {
        agent.speed = monster.GetBaseSpeed();      
    }
}