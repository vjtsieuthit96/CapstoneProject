using UnityEngine;

public class RageOrRetreatNode : Node
{
    private MonsterAI monsterAI;

    public RageOrRetreatNode(MonsterAI monsterAI)
    {
        this.monsterAI = monsterAI;
    }

    public override NodeState Evaluate()
    {
        float timeSinceLastRage = Time.time - monsterAI.GetLastRageTime();
        // Nếu Rage kết thúc, quái trở lại trạng thái bình thường
        if (monsterAI.GetisEnraged() && timeSinceLastRage >= monsterAI.GetRageDuration())
        {
            monsterAI.SetEnraged(false);
            Debug.Log("Enraged End");
            return NodeState.FAILURE;
        }
        // Nếu Rage cd xong, kích hoạt lại
        if (!monsterAI.GetisEnraged() && timeSinceLastRage >= monsterAI.GetRageCooldown())
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
