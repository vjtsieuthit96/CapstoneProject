using System.Collections.Generic;

public class OrcAI : MonsterAI
{
    protected override Node CreateBehaviorTree()
    {      

        return new Selector(new List<Node>
        {
            new CheckLowHealthNode(this, monsterStats),
            new Sequence(new List<Node> // Nếu thấy Player thì Chase + Attack
            {
                new CheckPlayerInFOVNode(this),
                new ChaseNode(this, _monsterAgent),
                new Selector(new List<Node> // Chọn skill theo cooldown hoặc melee
                {
                    new SkillUsageNode(this, skillManager),
                    new MeleeAttackNode(this)
                })
            }),
            new PatrolNode(this, _monsterAgent) // Nếu không có gì, tuần tra
        });
    }
}