public class CheckRetreatNode : Node
{
    private MonsterAI monster;
    private MonsterStats stats;   

    public CheckRetreatNode(MonsterAI monster, MonsterStats stats)
    {
        this.monster = monster;
        this.stats = stats;
    }

    public override NodeState Evaluate()
    {
        // Nếu AI đã retreat một lần, không retreat nữa
        if (monster.GetHasRetreat()) return NodeState.FAILURE;

        if (stats.GetCurrentHealth() < stats.GetMaxHealth() * 0.2f)
        {           
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }  
}