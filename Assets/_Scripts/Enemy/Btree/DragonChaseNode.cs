using UnityEngine;
using UnityEngine.AI;

public class DragonChaseNode : Node
{
    private DragonBossAI monster;
    private NavMeshAgent agent;
    public DragonChaseNode(DragonBossAI monster, NavMeshAgent agent)
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
            agent.speed = monster.GetBaseSpeed();
            return NodeState.FAILURE;
        }

        float distanceToPlayer = Vector3.Distance(monster.transform.position, player.position);
        Vector3 posA = monster.transform.position;
        Vector3 posB = monster.currenHoverPos;
        float flatDistance = Vector2.Distance(new Vector2(posA.x, posA.z), new Vector2(posB.x, posB.z));

        // Kiểm tra trạng thái bay
        if (monster.IsFlying())
        {

            // Nếu bay quá thời gian quy định thì chuyển sang đi bộ
            if (monster.flyTimer >= monster.maxFlyTime)
            {
                monster.SetFlying(false);
                monster.SetLanding(true);
                monster.SetAnimatorParameter(MonsterAnimatorHash.isLandingHash, true);
                monster.SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, false);
                agent.enabled = true;
                agent.speed = monster.GetBaseSpeed();
                monster.hasHoverTarget = false;
                Debug.Log("Rồng đã đáp xuống, chuyển sang đi bộ.");
                return NodeState.RUNNING;
            }
            if (!monster.hasHoverTarget || Vector3.Distance(monster.currenHoverPos, player.position) > monster.GetAttackRange()
                || monster.hoverTimer >= monster.maxHoverTime)
            {
                // Bay lượn quanh target ở khoảng cách ngẫu nhiên
                Vector3 offset = new Vector3(
                    Random.Range(-monster.flyHoverRadius, monster.flyHoverRadius),
                    0,
                    Random.Range(-monster.flyHoverRadius, monster.flyHoverRadius)
                );
                monster.currenHoverPos = player.position + offset;
                monster.hasHoverTarget = true;
                monster.hoverTimer = 0f;
            }
            agent.SetDestination(monster.currenHoverPos);
            agent.speed = monster.GetBaseSpeed() * monster.GetSpeedMultiplier();

            if (flatDistance <= agent.stoppingDistance)
            {
                monster.hasHoverTarget = false;
                monster.hoverTimer = 0f;
            }
            return NodeState.RUNNING;
        }
        else
        {
            monster.hasHoverTarget = false;            
        }
        if (monster.IsLanding())
        {
            Debug.Log(distanceToPlayer);
            // Nếu đang đi bộ
            agent.SetDestination(player.position);
            agent.speed = monster.GetBaseSpeed() * monster.GetSpeedMultiplier();
            if (distanceToPlayer <= agent.stoppingDistance)
            {
                return NodeState.SUCCESS;
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Debug.Log("Rồng đang đi bộ đuổi theo mục tiêu.");
                return NodeState.RUNNING;
            }
        }

        agent.speed = monster.GetBaseSpeed();
        return NodeState.FAILURE;
    }
}
