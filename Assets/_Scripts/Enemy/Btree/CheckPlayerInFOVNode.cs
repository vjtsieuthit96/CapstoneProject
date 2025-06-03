using UnityEngine;

public class CheckPlayerInFOVNode : Node
{
    private MonsterAI monsterAI;
    private float memoryDuration = 3f; // Giữ mục tiêu trong 3 giây nếu mất tầm nhìn
    private float lastSeenTime = float.MinValue; // Thời điểm cuối cùng thấy người chơi

    public CheckPlayerInFOVNode(MonsterAI monster) { this.monsterAI = monster; }

    public override NodeState Evaluate()
    {
        Transform player = monsterAI.GetTarget();
        if (player == null) return NodeState.FAILURE;

        Vector3 directionToPlayer = (player.position - monsterAI.transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(monsterAI.transform.position, player.position);
        float angleToPlayer = Vector3.Angle(monsterAI.transform.forward, directionToPlayer);

        //  Nếu AI thấy Player, cập nhật lastSeenTime
        if (distanceToPlayer <= monsterAI.GetViewRadius() && angleToPlayer <= monsterAI.GetViewAngle() / 2)
        {
            lastSeenTime = Time.time; // Cập nhật thời điểm thấy Player lần cuối
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, true);            
            Debug.Log("AI nhìn thấy người chơi! Đuổi theo.");
            return NodeState.SUCCESS;
        }

        //  Nếu mất dấu nhưng còn trong thời gian nhớ, tiếp tục đuổi
        if (Time.time - lastSeenTime < memoryDuration)
        {
            Debug.Log("Mất tầm nhìn nhưng vẫn theo dấu người chơi!");
            return NodeState.SUCCESS;
        }

        //  Nếu đã quá memoryDuration, quay về tuần tra
        Debug.Log("Hoàn toàn mất dấu! Quay về tuần tra.");
        return NodeState.FAILURE;
    }
}