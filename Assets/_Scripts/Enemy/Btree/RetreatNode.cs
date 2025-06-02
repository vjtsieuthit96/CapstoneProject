using UnityEngine;
using UnityEngine.AI;

public class RetreatNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;
    private float retreatSpeedMultiplier = 1.75f;
    private float safeDistance = 25f;
    private float stuckCheckTime = 1f;
    private float stuckDuration = 10f; // Chỉ dừng retreat nếu bị kẹt liên tục 10 giây
    private float lastCheckTime = 0f;
    private float stuckStartTime = 0f;
    private float lastDistance;

    private Vector3 escapeTarget;
    private bool isRetreating = false;
    private bool wasSafe = false;

    public RetreatNode(MonsterAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        Transform player = monster.GetTarget();
        if (player == null) return NodeState.FAILURE;

        if (escapeTarget == Vector3.zero || agent.remainingDistance < 2f)
        {
            SetEscapeTarget();
        }

        if (!isRetreating)
        {
            agent.speed *= retreatSpeedMultiplier;
            isRetreating = true;
        }

        float currentDistance = Vector3.Distance(monster.transform.position, player.position);

        // Nếu bị chặn khi retreat, AI quay lại trạng thái bình thường
        if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            ResetSpeed();
            return NodeState.SUCCESS;
        }

        // Nếu AI đã đạt khoảng cách an toàn nhưng Player vẫn tiếp cận, AI retreat tiếp
        if (currentDistance >= safeDistance)
        {
            wasSafe = true;
        }
        if (wasSafe && currentDistance < lastDistance)
        {
            agent.SetDestination(escapeTarget);
            return NodeState.RUNNING;
        }

        // Kiểm tra nếu AI bị kẹt nhưng chỉ dừng retreat nếu khoảng cách không đổi trong 10 giây
        if (Time.time - lastCheckTime > stuckCheckTime)
        {
            if (Mathf.Abs(currentDistance - lastDistance) < 0.5f)
            {
                if (stuckStartTime == 0f)
                {
                    stuckStartTime = Time.time;
                }

                if (Time.time - stuckStartTime >= stuckDuration)
                {
                    ResetSpeed();
                    return NodeState.SUCCESS;
                }
            }
            else
            {
                stuckStartTime = 0f; // Reset thời gian nếu AI vẫn thay đổi khoảng cách
            }

            lastCheckTime = Time.time;
            lastDistance = currentDistance;
        }

        agent.SetDestination(escapeTarget);
        return NodeState.RUNNING;
    }

    private void SetEscapeTarget()
    {
        Transform player = monster.GetTarget();
        Vector3 directionAway = (monster.transform.position - player.position).normalized;
        escapeTarget = monster.transform.position + directionAway * 15f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(escapeTarget, out hit, 15f, NavMesh.AllAreas))
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
        isRetreating = false;
        wasSafe = false;
        stuckStartTime = 0f;
    }
}