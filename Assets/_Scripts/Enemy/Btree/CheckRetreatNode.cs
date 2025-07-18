public class CheckRetreatNode : Node
{
    private MonsterAI monster;
    private MonsterStats stats;
    private float retreatThreshold; // Tham số rút lui

    public CheckRetreatNode(MonsterAI monster, MonsterStats stats, float retreatThreshold)
    {
        this.monster = monster;
        this.stats = stats;
        this.retreatThreshold = retreatThreshold;
    }

    public override NodeState Evaluate()
    {
        if (monster.GetHasRetreat()) return NodeState.FAILURE;

        if (stats.GetCurrentHealth() < stats.GetMaxHealth() * retreatThreshold)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}