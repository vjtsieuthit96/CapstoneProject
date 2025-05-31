using UnityEngine;

public class RetreatNode : Node
{
    private MonsterStats monsterStats;
    public RetreatNode(MonsterStats monsterStats)
    {
        this.monsterStats = monsterStats;
    }

    public override NodeState Evaluate()
    {
        if (monsterStats.GetCurrentHealth() < monsterStats.GetMaxHealth() * 0.15f)
        {
            Debug.Log("Monster retreating!");
            // Logic rút lui (tìm nơi an toàn)
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}