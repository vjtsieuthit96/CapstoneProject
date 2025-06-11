using System.Collections.Generic;
using UnityEngine;

public class OrcAI : MonsterAI
{
    protected override void Start()
    {
        base.Start();
        
    }
    protected override void Update()
    {
        base.Update();       
    }
    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
    {
        new Sequence(new List<Node> //  Nếu máu thấp, AI retreat rồi tiếp tục hành vi khác
        {
            new CheckRetreatNode(this, monsterStats),
            new RetreatNode(this, monsterAgent)
        }),
        new Sequence(new List<Node> //  Nếu thấy Player, AI chiến đấu
        {
            new CheckPlayerInFOVNode(this),
            new RoarNode(this, monsterAudio),
            new ChaseNode(this, monsterAgent),            
            new Selector(new List<Node> 
            {
                new SkillUsageNode(this, skillManager),
                new MeleeAttackNode(this)
            })
        }),
        new PatrolNode(this, monsterAgent) //  Nếu mất dấu Player hoàn toàn, AI tuần tra lại
    });
    }
}