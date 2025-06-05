using UnityEngine;

public class CheckPlayerInFOVNode : Node
{
    private MonsterAI monsterAI;
    private float memoryDuration = 3f; // Giữ mục tiêu trong 3 giây nếu mất tầm nhìn
    private float lastSeenTime = float.MinValue; // Thời điểm cuối cùng thấy người chơi
    private float lastAttackedTime = float.MinValue; //  Lưu thời điểm AI bị tấn công
    private float attackMemoryDuration = 5f; //  AI nhớ kẻ tấn công trong 5 giây

    public CheckPlayerInFOVNode(MonsterAI monster) { this.monsterAI = monster; }    

    public void OnAttacked()
    {
        lastAttackedTime = Time.time; //  Cập nhật thời gian bị tấn công
        Debug.Log("AI bị tấn công! Ghi nhớ kẻ địch.");
        AlertNearbyAllies();
    }

    public override NodeState Evaluate()
    {
        Transform player = monsterAI.GetTarget();
        if (player == null) return NodeState.FAILURE;

        Vector3 directionToPlayer = (player.position - monsterAI.transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(monsterAI.transform.position, player.position);
        float angleToPlayer = Vector3.Angle(monsterAI.transform.forward, directionToPlayer);

        if (distanceToPlayer <= monsterAI.GetViewRadius() && angleToPlayer <= monsterAI.GetViewAngle() / 2)
        {
            lastSeenTime = Time.time;
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, true);
            Debug.Log("AI nhìn thấy người chơi! Đuổi theo.");
            return NodeState.SUCCESS;
        }

        //  Nếu AI bị tấn công gần đây, nó tiếp tục truy đuổi dù mất dấu
        if (Time.time - lastAttackedTime < attackMemoryDuration)
        {
            lastSeenTime = Time.time;
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, true);
            Debug.Log("AI vẫn nhớ kẻ tấn công! Tiếp tục đuổi theo.");
            return NodeState.SUCCESS;
        }

        //  Nếu đã quá `memoryDuration`, AI phải quay lại trạng thái tuần tra
        if (Time.time - lastSeenTime >= memoryDuration)
        {
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, false);
            Debug.Log("Hoàn toàn mất dấu! Quay về tuần tra.");
            return NodeState.FAILURE;
        }

        return NodeState.RUNNING;
    }
    private void AlertNearbyAllies()
    {
        string allyTag = "Enemy";
        float alertRadius = monsterAI.GetAlertRadius();

        GameObject[] allies = GameObject.FindGameObjectsWithTag(allyTag);

        foreach (GameObject ally in allies)
        {
            if (ally.transform != monsterAI.transform) // 🔥 Tránh cảnh báo chính nó
            {
                float distanceToAlly = Vector3.Distance(monsterAI.transform.position, ally.transform.position);

                if (distanceToAlly <= alertRadius)
                {
                    //Lấy MonsterAI từ đồng minh
                    MonsterAI allyMonsterAI = ally.GetComponent<MonsterAI>();

                    if (allyMonsterAI != null)
                    {
                        // 🔥 Tìm `CheckPlayerInFOVNode` trong cây hành vi của đồng minh
                        CheckPlayerInFOVNode allyFOVNode = allyMonsterAI.GetBehaviorNode<CheckPlayerInFOVNode>();

                        if (allyFOVNode != null)
                        {
                            allyFOVNode.lastSeenTime = Time.time; // 🔥 Ghi nhớ vị trí Player
                            allyMonsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, true); // 🔥 Chuyển sang trạng thái chiến đấu
                            Debug.Log($"Đồng minh {ally.name} nhận cảnh báo! Bắt đầu truy đuổi Player.");
                        }
                    }
                }
            }
        }
    }
}