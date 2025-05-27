using UnityEngine;

public class RageOrRetreatNode : Node
{
    private MonsterAI monsterAI;
    private MonsterStats monsterStats;

    public RageOrRetreatNode(MonsterAI monsterAI, MonsterStats monsterStats)
    {
        this.monsterAI = monsterAI;
        this.monsterStats = monsterStats;
    }

    public override NodeState Evaluate()
    {
        float timeSinceLastRage = Time.time - monsterAI.GetLastRageTime();
        // Nếu Rage kết thúc, quái trở lại trạng thái bình thường
        if (monsterAI.GetIsEnraged() && timeSinceLastRage >= monsterAI.GetRageDuration())
        {
            monsterAI.SetEnraged(false);
            Debug.Log("Enraged End");
            return NodeState.FAILURE;
        }
        // Nếu Rage cd xong, kích hoạt lại
        if (!monsterAI.GetIsEnraged() && timeSinceLastRage >= monsterAI.GetRageCooldown())
        {
            monsterAI.SetEnraged(true);
            monsterAI.SetLastRageTime(Time.time);
            Debug.Log("Rage State");
            //Logic của cuồng nộ
            return NodeState.SUCCESS;
        }
        // Rage chưa hồi skill thì chạy thôi
        // Logic rút lui
        return NodeState.FAILURE;
    }
}
