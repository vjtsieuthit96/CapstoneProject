using UnityEngine;

public class CheckPlayerInFOVNode : Node
{
    private MonsterAI monsterAI;
    private float memoryDuration = 5f; // AI sẽ nhớ vị trí người chơi trong 3 giây
    private float lastSeenTime = 0f; // Thời điểm cuối cùng thấy người chơi

    public CheckPlayerInFOVNode(MonsterAI monster) { this.monsterAI = monster; }

    public override NodeState Evaluate()
    {
        Transform player = monsterAI.GetTarget();
        if (player == null)
        {
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, false);
            Debug.Log("Không tìm thấy người chơi! Trở lại tuần tra.");
            return NodeState.FAILURE;
        }

        Vector3 directionToPlayer = (player.position - monsterAI.transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(monsterAI.transform.position, player.position);

        // Nếu AI thấy người chơi, cập nhật thời điểm cuối cùng thấy người chơi
        if (distanceToPlayer <= monsterAI.GetViewRadius() &&
            Vector3.Angle(monsterAI.transform.forward, directionToPlayer) <= monsterAI.GetViewAngle() / 2)
        {
            lastSeenTime = Time.time; // Cập nhật thời điểm nhìn thấy người chơi
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, true);
            Debug.Log("Monster nhìn thấy người chơi! Đuổi theo.");
            return NodeState.SUCCESS;
        }

        // Nếu AI mất dấu nhưng còn trong khoảng thời gian nhớ, tiếp tục đuổi theo
        if (Time.time - lastSeenTime < memoryDuration)
        {
            Debug.Log("Mất tầm nhìn nhưng vẫn theo dấu người chơi!");
            return NodeState.SUCCESS;
        }

        // Nếu đã quá thời gian nhớ, quay lại tuần tra
        monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, false);
        Debug.Log("Hoàn toàn mất dấu! Quay về tuần tra.");
        return NodeState.FAILURE;
    }
}