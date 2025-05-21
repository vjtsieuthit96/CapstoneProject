using UnityEngine;


public class CheckLowHealthNode : Node
{
    private MonsterAI monsterAI;
    public CheckLowHealthNode(MonsterAI monsterAI)
    {
        this.monsterAI = monsterAI;
    }

    public override NodeState Evaluate()
    {
        if (monsterAI.health < 0.3* monsterAI.maxHealth)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
