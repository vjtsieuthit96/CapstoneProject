using System.Collections.Generic;

public class OrcAI : MonsterAI
{
    protected override Node CreateBehaviorTree()
    {      

        return new Selector(new List<Node>
        {            
            new Sequence(new List<Node> // Nếu thấy Player thì Chase + Attack
            {
                new CheckPlayerInFOVNode(this),
                new ChaseNode(this,_monsterAgent),
                new MeleeAttackNode(this)
            }),
            //new PatrolNode(this,_monsterAgent) // Nếu không có gì, tuần tra
        });
    }
}