using UnityEngine;


public class CheckLowHealthNode : Node
{ 
    private MonsterStats monsterStats;
    public CheckLowHealthNode(MonsterStats monsterStats)
    {
        this.monsterStats = monsterStats;
    }

    public override NodeState Evaluate()
    {
        if (monsterStats.GetMaxHealth()<0.3*monsterStats.GetMaxHealth())
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
