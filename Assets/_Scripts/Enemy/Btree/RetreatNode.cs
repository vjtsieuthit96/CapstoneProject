using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RetreatNode : Node
{
    private MonsterAI monster;
    private NavMeshAgent agent;

    // Chuyển các giá trị số thành biến  
    private float safeDistance = 30f;
    private float escapeDistanceMultiplier = 1.25f; //  Tạo biến để nhân khoảng cách chạy
    private float navMeshSearchRadius = 25f; //  Biến điều chỉnh phạm vi tìm vị trí hợp lệ trên NavMesh
    private float minAllyDistance = 15f; // Khoảng cách tối thiểu để bỏ qua đồng minh quá gần 

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
        agent.speed *= monster.GetSpeedMultiplier();

        if (escapeTarget == Vector3.zero || agent.remainingDistance < 2f)
        {
            SetEscapeTarget();
        }

        float currentDistance = Vector3.Distance(monster.transform.position, player.position);

        if (currentDistance >= safeDistance)
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

        Transform farthestAlly = FindFarthestAlly();

        if (farthestAlly != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(farthestAlly.position, out hit, navMeshSearchRadius, NavMesh.AllAreas)) 
            {
                Debug.Log("Có đồng minh trong khoảng an toàn, rút lui về phía đó!");
                escapeTarget = hit.position;
            }
            else
            {
                Debug.Log("Đồng minh không phải vị trí hợp lệ, rút lui khỏi Player!");
                escapeTarget = monster.transform.position + directionAway * escapeDistanceMultiplier * safeDistance; 
            }
        }
        else
        {
            Debug.Log("Không có đồng minh phù hợp, rút lui khỏi Player!");
            escapeTarget = monster.transform.position + directionAway * escapeDistanceMultiplier * safeDistance; 
        }

        agent.SetDestination(escapeTarget);
    }

    

    private Transform FindFarthestAlly()
    {
        string allyTag = "Enemy";
        GameObject[] allies = GameObject.FindGameObjectsWithTag(allyTag);

        //  Chỉ chọn đồng minh trong khoảng an toàn **và** không đứng quá gần
        var validAllies = allies.Where(ally =>
            Vector3.Distance(monster.transform.position, ally.transform.position) <= safeDistance &&
            Vector3.Distance(monster.transform.position, ally.transform.position) >= minAllyDistance && //  Bỏ qua đồng minh gần hơn khoảng cách tối thiểu
            ally.transform != monster.transform).ToList();

        //  Nếu danh sách rỗng, trả về `null`
        if (validAllies.Count == 0) return null;

        //  Chọn đồng minh xa nhất từ danh sách hợp lệ
        return validAllies.OrderByDescending(ally =>
            Vector3.Distance(monster.transform.position, ally.transform.position))
            .FirstOrDefault()?.transform;
    }

    private void ResetSpeed()
    {
        agent.speed = monster.GetBaseSpeed();
    }
}