using UnityEngine;

public class DarkTreeAttackNode : Node
{
    private DarkMagicTree tree;
    private MonsterStats stats;
    private Transform player;
    public EnemySpawner spawner;

    public DarkTreeAttackNode(DarkMagicTree tree, MonsterStats stats, Transform player, EnemySpawner spawner)
    {
        this.tree = tree;
        this.stats = stats;
        this.player = player;
        this.spawner = spawner;
    }

    public override NodeState Evaluate()
    {
        if (tree.isDefending)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float hpPercent = stats.GetCurrentHealth() / stats.GetMaxHealth();

        if (hpPercent > 0.5f)
        {
            tree.ActivateNearestCircleSkill();
            spawner.currentLevel = Random.Range(1, 3);
            Debug.Log($"DarkTree ATTACK spawn enemy level {spawner.currentLevel}");
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            tree.ActivateNearestCirclesSkill(2);
            spawner.currentLevel = Random.Range(3, 5);
            Debug.Log($"DarkTree ATTACK (low HP) spawn enemy level {spawner.currentLevel}");
            state = NodeState.SUCCESS;
            return state;
        }
    }
}
