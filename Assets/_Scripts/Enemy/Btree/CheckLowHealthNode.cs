using UnityEngine;

public class CheckLowHealthNode : Node
{
    private MonsterAI monsterAI;
    private MonsterStats monsterStats;

    public CheckLowHealthNode(MonsterAI monsterAI, MonsterStats monsterStats)
    {
        this.monsterAI = monsterAI;
        this.monsterStats = monsterStats;
    }

    public override NodeState Evaluate()
    {
        if (monsterStats.GetCurrentHealth() < 0.3f * monsterStats.GetMaxHealth())
        {
            Debug.Log("Low HP");
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}